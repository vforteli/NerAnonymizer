using System.Collections.Immutable;
using Microsoft.ML.OnnxRuntime;
using vforteli.WordPieceTokenizer;

namespace NerAnonymizer;

/// <summary>
/// Internal raw prediction with only score and index
/// </summary>
public record struct Prediction(string EntityGroup, float Score, int Start, int End);

public class NerModelRunner(
    Lazy<InferenceSession> inferenceSession,
    Lazy<WordPieceTokenizer> tokenizer,
    BertNerModelConfig config) : IDisposable
{
    private static readonly RunOptions RunOptions = new();

    /// <summary>
    /// Run classification and group results
    /// </summary>
    public IEnumerable<PredictionResult> RunClassification(string text) => RunClassification(text, true);


    /// <summary>
    /// Run classification and specify if results should be grouped, eg: B-Person + I-Person + I-Person -> Person
    /// </summary>
    public IEnumerable<PredictionResult> RunClassification(string text, bool groupResults)
    {
        const int stride = 100; // todo... some say 20%, which actually is where i ended up by testing
        const int chunkSize = 512;
        const int trimmedChunkSize = chunkSize - 2; // -2 for CLS and SEP

        var tokens = tokenizer.Value.Tokenize(text).ToImmutableList();
        var labels = config.IdToLabel.Values.ToImmutableArray();

        var results = GetWindowIndexesWithStride(tokens.Count, chunkSize, stride).SelectMany(c =>
        {
            List<Token> batch =
            [
                new Token(102, 0, 0), // CLS
                ..tokens.Skip(c).Take(trimmedChunkSize),
                new Token(103, 0, 0), // SEP
            ];

            var output = RunModel(inferenceSession.Value, batch.Select(o => (long)o.Id).ToArray());

            var predictions = GetTokenPredictions(labels, batch, [..output]).ToList()[1..^1];

            var trimmedPredictions = TrimPredictionOutput(predictions, stride, trimmedChunkSize, tokens.Count, c);

            return trimmedPredictions;
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

    /// <summary>
    /// Remove the stride tokens
    /// </summary>
    public static IEnumerable<Prediction> TrimPredictionOutput(IEnumerable<Prediction> predictions, int stride,
        int trimmedChunkSize, int tokensCount, int currentIndex) =>
        predictions
            .Skip(currentIndex == 0 ? 0 : stride / 2)
            .Take(currentIndex + trimmedChunkSize >= tokensCount
                ? trimmedChunkSize
                : trimmedChunkSize - (currentIndex == 0 ? stride / 2 : stride));


    /// <summary>
    /// Dispose
    /// </summary>
    public void Dispose()
    {
        GC.SuppressFinalize(this);

        if (inferenceSession.IsValueCreated)
        {
            inferenceSession.Value.Dispose();
        }
    }


    /// <summary> 
    /// Compute soft max... 
    /// </summary> 
    public static IReadOnlyList<float> ComputeSoftmax(ReadOnlySpan<float> input)
    {
        var expValues = new float[input.Length];

        float expSum = 0;
        for (var i = 0; i < input.Length; i++)
        {
            expValues[i] = MathF.Exp(input[i]);
            expSum += expValues[i];
        }

        for (var i = 0; i < expValues.Length; i++)
        {
            expValues[i] /= expSum;
        }

        return expValues;
    }


    /// <summary>
    /// Get predictions from model output
    /// </summary>   
    public static List<Prediction> GetTokenPredictions(ImmutableArray<string> labels, IReadOnlyList<Token> tokens,
        ImmutableArray<float> values)
    {
        return values
            .Chunk(labels.Length)
            .Zip(tokens, (chunk, token) =>
            {
                var (score, label) = ComputeSoftmax(chunk)
                    .Zip(labels)
                    .MaxBy(o => o.First);

                return new Prediction(label, score, token.Start, token.End);
            })
            .ToList();
    }


    /// <summary>
    /// Groups the raw token predictions into groups and drops unknown tokens
    /// Eg: B-Person + I-Person + I-Person into Person
    /// </summary>
    public static IEnumerable<PredictionResult> GetGroupedPredictions(IEnumerable<Prediction> predictions, string text)
    {
        var currentGroup = new List<Prediction>(10);
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
                EntityGroup = groupStartIndex.EntityGroup[2..], // trim the B- or I-
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
    public static float[] RunModel(InferenceSession session, ReadOnlySpan<long> tokenIds)
    {
        var shape = new long[] { 1, tokenIds.Length };

        var ones = new long[tokenIds.Length];
        Array.Fill(ones, 1);

        var zeros = new long[tokenIds.Length];
        Array.Fill(zeros, 0);

        using var inputs = OrtValue.CreateTensorValueFromMemory(tokenIds.ToArray(), shape);
        using var attentionMask = OrtValue.CreateTensorValueFromMemory(ones, shape);
        using var tokenTypeIds = OrtValue.CreateTensorValueFromMemory(zeros, shape);

        var ortValues = new Dictionary<string, OrtValue>
        {
            { "input_ids", inputs },
            { "attention_mask", attentionMask },
            { "token_type_ids", tokenTypeIds },
        };

        using var output = session.Run(RunOptions, ortValues, session.OutputNames);
        return output[0].GetTensorDataAsSpan<float>().ToArray();
    }
}