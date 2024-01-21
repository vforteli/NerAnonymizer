using System.Text.Json;
using Microsoft.ML;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Microsoft.ML.Tokenizers;
using TorchSharp;


var tokenizer = new Tokenizer(new WordPieceModel());


// Define input for tokenization
// var input = "Helsingistä tuli Suomen suuriruhtinaskunnan pääkaupunki vuonna 1812.";
// var input = "Helsinki";
var input = "1234567-1";

var tokenizerEncodedResult = tokenizer.Encode(input);

Console.WriteLine(JsonSerializer.Serialize(tokenizerEncodedResult));

var decoded = tokenizer.Decode(tokenizerEncodedResult.Ids);
Console.WriteLine(decoded);

var inputs = new List<NamedOnnxValue>
{
    NamedOnnxValue.CreateFromTensor("input_ids", ConvertToTensor(tokenizerEncodedResult.Ids.ToArray(), tokenizerEncodedResult.Ids.Count)),
    NamedOnnxValue.CreateFromTensor("attention_mask", ConvertToTensorSingle(1, tokenizerEncodedResult.Ids.Count)),
    NamedOnnxValue.CreateFromTensor("token_type_ids", ConvertToTensorSingle(0, tokenizerEncodedResult.Ids.Count)),
};

using var session = new InferenceSession("../../finbert-ner-onnx/model.onnx");
var output = session.Run(inputs);

foreach (var item in output)
{
    DenseTensor<float> value = (DenseTensor<float>)item.Value;
    Console.WriteLine($"Found something");
    Console.WriteLine(JsonSerializer.Serialize(item));
    Console.WriteLine("---");
}

// [{'entity': 'B-GPE', 'score': 0.9347385, 'index': 1, 'word': 'Helsingistä', 'start': 0, 'end': 11}]


static Tensor<long> ConvertToTensor(int[] inputArray, int dimension)
{
    var input = new DenseTensor<long>(new[] { 1, dimension });

    for (var i = 0; i < inputArray.Length; i++)
    {
        input[0, i] = inputArray[i];
    }

    return input;
}


static Tensor<long> ConvertToTensorSingle(int value, int dimension)
{
    var input = new DenseTensor<long>(new[] { 1, dimension });

    for (var i = 0; i < dimension; i++)
    {
        input[0, i] = value;
    }

    return input;
}