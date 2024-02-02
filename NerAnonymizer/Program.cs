using System.Diagnostics;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.ML.OnnxRuntime;
using NerAnonymizer;

const string vocabPath = "../../finbert-ner-onnx/vocab.txt";
const string modelPath = "../../finbert-ner-onnx/model.onnx";
const string configPath = "../../finbert-ner-onnx/config.json";

var jsonSerializerOptions = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };

var vocabulary = await File.ReadAllTextAsync(vocabPath);
var config = await JsonSerializer.DeserializeAsync<BertNerModelConfig>(File.OpenRead(configPath)) ?? throw new ArgumentNullException("config");
var tokenizer = new WordPieceTokenizer(vocabulary);
var inferenceSession = new Lazy<InferenceSession>(() => new InferenceSession(modelPath));


// var input = "Helsingistä tuli Suomen suuriruhtinaskunnan pääkaupunki vuonna 1812.";
// var input = "Helsingistä tuli Suomen";
var input = TestStrings.LongTestString;
// var input = TestStrings.NewsString;
// var input = string.Join("", Enumerable.Repeat(TestStrings.LongTestString, 10));
// var input = "1234567-1";

using var runner = new NerModelRunner(inferenceSession, tokenizer, config);

var stopwatch = Stopwatch.StartNew();

var groupedResults = runner.RunClassification(input, true);


Console.WriteLine($"Done after {stopwatch.ElapsedMilliseconds}ms");
// Console.WriteLine(JsonSerializer.Serialize(groupedResults, jsonSerializerOptions));

var sb = new StringBuilder(input);
foreach (var entity in groupedResults.Where(o => o.EntityGroup == "PERSON").OrderByDescending(o => o.Start))
{
    sb.Remove(entity.Start, entity.End - entity.Start).Insert(entity.Start, "*****");
}

Console.WriteLine(sb.ToString());