using System.Text.Json.Serialization;

namespace NerAnonymizer;

public record PredictionResult
{
    [JsonPropertyName("entity_group")]
    public required string EntityGroup { get; init; }

    [JsonPropertyName("score")]
    public required float Score { get; init; }

    [JsonPropertyName("word")]
    public required string Word { get; init; }

    [JsonPropertyName("start")]
    public required int Start { get; init; }

    [JsonPropertyName("end")]
    public required int End { get; init; }
}