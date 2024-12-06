using System.Diagnostics;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.ML.OnnxRuntime;
using NerAnonymizer;
using vforteli.WordPieceTokenizer;

const string vocabPath = "../../../../finbert-ner-onnx/vocab.txt";
const string modelPath = "../../../../finbert-ner-onnx/model.onnx";
const string configPath = "../../../../finbert-ner-onnx/config.json";

var jsonSerializerOptions = new JsonSerializerOptions
    { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };

var vocabulary = await File.ReadAllTextAsync(vocabPath);
var config =
    await JsonSerializer.DeserializeAsync<BertNerModelConfig>(File.OpenRead(configPath),
        SourceGenerationContext.Default.BertNerModelConfig) ??
    throw new ArgumentNullException("config");
var tokenizer = new Lazy<WordPieceTokenizer>(() => new WordPieceTokenizer(vocabulary));
var inferenceSession = new Lazy<InferenceSession>(() => new InferenceSession(modelPath));

// var input = "Helsingistä tuli Suomen suuriruhtinaskunnan pääkaupunki vuonna 1812.";
// var input = "Helsingistä tuli Suomen";
// var input = TestStrings.LongTestString;
// var input = TestStrings.NewsString;
var input = string.Join("", Enumerable.Repeat(TestStrings.LongTestString, 1));
// var input = "1234567-1";

using var runner = new NerModelRunner(inferenceSession, tokenizer, config);

var stopwatch = Stopwatch.StartNew();
var allocations = GC.GetTotalAllocatedBytes();

for (var i = 0; i < 300; i++)
{
    Console.WriteLine("Sup " + i);
    var groupedResults = runner.RunClassification(input).ToList();
    Console.WriteLine(GC.GetTotalAllocatedBytes() - allocations);

    // Console.WriteLine(Utils.Anonymize(input, groupedResults));
}

Console.WriteLine(GC.GetTotalAllocatedBytes() - allocations);
Console.WriteLine($"Done after {stopwatch.ElapsedMilliseconds}ms");


// Console.WriteLine(JsonSerializer.Serialize(groupedResults, jsonSerializerOptions));
// Console.WriteLine(Utils.Anonymize(input, groupedResults));

// 16822552


[JsonSerializable(typeof(BertNerModelConfig))]
internal partial class SourceGenerationContext : JsonSerializerContext
{
}