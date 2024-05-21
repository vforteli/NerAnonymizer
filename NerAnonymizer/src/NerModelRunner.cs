using System.Collections.Immutable;
using Microsoft.ML.OnnxRuntime;
using vforteli.WordPieceTokenizer;

namespace NerAnonymizer;

/// <summary>
/// Internal raw prediction with only score and index
/// </summary>
public record Prediction(string EntityGroup, float Score, int Start, int End);


public class NerModelRunner(Lazy<InferenceSession> inferenceSession, Lazy<WordPieceTokenizer> tokenizer, BertNerModelConfig config) : IDisposable
{
    /// <summary>
    /// Run classification and group results
    /// </summary>
    public IEnumerable<PredictionResult> RunClassification(string text) => RunClassification(text, true);


    /// <summary>
    /// Run classification and specify if results should be grouped, eg: B-Person + I-Person + I-Person -> Person
    /// </summary>
    public IEnumerable<PredictionResult> RunClassification(string text, bool groupResults)
    {
        const int stride = 100;   // todo... some say 20%, which actually is where i ended up by testing
        const int chunkSize = 512 - 2;  // -2 for CLS and SEP

        var tokens = tokenizer.Value.Tokenize(text).ToList();

        var results = GetWindowIndexesWithStride(tokens.Count, chunkSize, stride).SelectMany(c =>
        {
            List<Token> batch =
            [
                new Token(102, 0, 0),   // CLS
                ..tokens.Skip(c).Take(chunkSize),
                new Token(103, 0,0),    // SEP
            ];

            var output = RunClassification(inferenceSession.Value, batch.Select(o => (long)o.Id).ToArray());

            return GetTokenPredictions(config.IdToLabel, batch, output.ToImmutableArray())
                .Skip(c == 0 ? 0 : stride / 2) // pick everything from the start if this is the first window
                .Take(c + chunkSize >= tokens.Count ? chunkSize : chunkSize - stride / 2);  // pick everything until the end if this is the last window
        }).ToList();

        return groupResults
            ? GetGroupedPredictions(results, text)
            : results.Select(o => new PredictionResult
            {
                End = o.End,
                EntityGroup = o.EntityGroup,
                Score = o.Score,
                Start = o.Start,
                Word = text[o.Start..o.End]
            });
    }


    public void Dispose()
    {
        GC.SuppressFinalize(this);

        if (inferenceSession.IsValueCreated)
        {
            inferenceSession.Value.Dispose();
        }
    }


    /// <summary>
    /// Computer soft max...
    /// </summary>
    public static IReadOnlyCollection<float> ComputeSoftmax(float[] input)
    {
        var expValues = input.Select(MathF.Exp).ToArray();
        var expSum = expValues.Sum();

        for (var i = 0; i < expValues.Length; i++)
        {
            expValues[i] = expValues[i] / expSum;
        }

        return expValues;
    }


    /// <summary>
    /// Get predictions from model output
    /// </summary>   
    public static IEnumerable<Prediction> GetTokenPredictions(IReadOnlyDictionary<int, string> labels, IReadOnlyList<Token> tokens, ImmutableArray<float> values)
    {
        var chunks = values.Chunk(labels.Count);

        foreach (var (chunk, index) in chunks.Select((o, i) => (o, i)))
        {
            var scores = ComputeSoftmax(chunk).Zip(labels);

            // foreach (var (value, labels) in scores)
            // {
            //     Console.WriteLine($"{labels.Value}: {value}");
            // }

            var bestScore = scores.MaxBy(o => o.First);

            var token = tokens[index];

            yield return new Prediction(bestScore.Second.Value, bestScore.First, token.Start, token.End);
        }
    }


    /// <summary>
    /// Groups the raw token predictions into groups and drops unknown tokens
    /// Eg: B-Person + I-Person + I-Person into Person
    /// </summary>
    public static IEnumerable<PredictionResult> GetGroupedPredictions(IEnumerable<Prediction> predictions, string text)
    {
        var currentGroup = new List<Prediction>();
        var currentGroupName = "";

        foreach (var prediction in predictions)
        {
            if (prediction.EntityGroup == currentGroupName)
            {
                // continuing same group.. keep going
                currentGroup.Add(prediction);
                continue;
            }

            if (prediction.EntityGroup.StartsWith("B-") || prediction.EntityGroup == "O")
            {
                if (currentGroup.Count != 0)
                {
                    yield return GetPredictionResultFromGroup(text, currentGroup);
                    currentGroup.Clear();
                }

                if (prediction.EntityGroup.StartsWith("B-"))
                {
                    currentGroup.Add(prediction);
                    currentGroupName = "I-" + prediction.EntityGroup[2..];
                }
            }
        }

        // if the input doesnt end with O, make sure we also yield any final groups
        if (currentGroup.Count != 0)
        {
            yield return GetPredictionResultFromGroup(text, currentGroup);
            currentGroup.Clear();
        }

        static PredictionResult GetPredictionResultFromGroup(string text, IReadOnlyList<Prediction> currentGroup)
        {
            var groupStartIndex = currentGroup.First();
            var groupEndIndex = currentGroup.Last();

            return new PredictionResult
            {
                EntityGroup = groupStartIndex.EntityGroup[2..],   // trim the B- or I-
                Score = currentGroup.Average(o => o.Score),
                Start = groupStartIndex.Start,
                End = groupEndIndex.End,
                Word = text[groupStartIndex.Start..groupEndIndex.End],
            };
        }
    }


    /// <summary>
    /// Get start index of windows with stride, eg count = 30, size = 10, stride = 2:
    /// 012345678901234567890123456789
    /// [        ]
    ///         [        ]
    ///                 [        ]
    ///                         [    ]
    ///                         
    /// Returns [0, 8, 16, 24]
    /// </summary>
    public static IReadOnlyList<int> GetWindowIndexesWithStride(int count, int size, int stride)
    {
        if (stride >= size)
        {
            throw new ArgumentException("Stride must be less than size");
        }

        var index = 0;
        var list = new List<int> { 0 };
        while (index < (count - size))
        {
            index += size - stride;
            list.Add(index);
        }

        return list;
    }


    /// <summary>
    /// Run classification with some model
    /// </summary>
    public static ReadOnlySpan<float> RunClassification(InferenceSession session, ReadOnlySpan<long> tokenIds)
    {
        var shape = new long[] { 1, tokenIds.Length };

        var ortValues = new Dictionary<string, OrtValue>
        {
            { "input_ids",  OrtValue.CreateTensorValueFromMemory(tokenIds.ToArray(), shape) },
            { "attention_mask", OrtValue.CreateTensorValueFromMemory(Enumerable.Repeat((long)1, tokenIds.Length).ToArray(), shape)},
            { "token_type_ids", OrtValue.CreateTensorValueFromMemory(Enumerable.Repeat((long)0, tokenIds.Length).ToArray(), shape) },
        };

        using var output = session.Run(new RunOptions(), ortValues, session.OutputNames);

        return output[0].GetTensorDataAsSpan<float>();
    }
}
