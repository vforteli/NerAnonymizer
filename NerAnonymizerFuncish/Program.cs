using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.ML.OnnxRuntime;
using NerAnonymizer;
using vforteli.WordPieceTokenizer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


const string vocabPath = "finbert-ner-onnx/vocab.txt";
const string modelPath = "finbert-ner-onnx/model.onnx";
const string configPath = "finbert-ner-onnx/config.json";

var jsonSerializerOptions = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };

var vocabulary = await File.ReadAllTextAsync(vocabPath);
var config = await JsonSerializer.DeserializeAsync(File.OpenRead(configPath), SourceGenerationContext.Default.BertNerModelConfig) ?? throw new ArgumentNullException("config");
var tokenizer = new Lazy<WordPieceTokenizer>(() => new WordPieceTokenizer(vocabulary));
var inferenceSession = new Lazy<InferenceSession>(() => new InferenceSession(modelPath));

using var runner = new NerModelRunner(inferenceSession, tokenizer, config);


app.MapHealthChecks("/health").WithOpenApi();

app.MapGet("/api/anonymize", (string text) =>
{
    var groupedResults = runner.RunClassification(text);
    return Utils.Anonymize(text, groupedResults);
})
.WithOpenApi();


app.Run();



[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(BertNerModelConfig))]
internal partial class SourceGenerationContext : JsonSerializerContext
{
}