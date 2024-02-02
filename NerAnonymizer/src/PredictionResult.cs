using System.Text.Json.Serialization;

namespace NerAnonymizer;

public record PredictionResult
{
    [JsonPropertyName("entity_group")]
    required public string EntityGroup { get; set; }

    [JsonPropertyName("score")]
    required public double Score { get; set; }

    [JsonPropertyName("word")]
    required public string Word { get; set; }

    [JsonPropertyName("start")]
    required public int Start { get; set; }

    [JsonPropertyName("end")]
    required public int End { get; set; }
}