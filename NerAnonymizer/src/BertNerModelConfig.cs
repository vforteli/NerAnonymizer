using System.Text.Json.Serialization;

namespace NerAnonymizer;

public record BertNerModelConfig
{
    [JsonPropertyName("_name_or_path")]
    required public string NameOrPath { get; init; }

    [JsonPropertyName("architectures")]
    required public List<string> Architectures { get; init; }

    [JsonPropertyName("attention_probs_dropout_prob")]
    required public double AttentionProbsDropoutProb { get; init; }

    [JsonPropertyName("classifier_dropout")]
    required public double ClassifierDropout { get; init; }

    [JsonPropertyName("hidden_act")]
    required public string HiddenActivation { get; init; }

    [JsonPropertyName("hidden_dropout_prob")]
    required public double HiddenDropoutProb { get; init; }

    [JsonPropertyName("hidden_size")]
    required public int HiddenSize { get; init; }

    [JsonPropertyName("id2label")]
    required public Dictionary<int, string> IdToLabel { get; init; }

    [JsonPropertyName("initializer_range")]
    required public double InitializerRange { get; init; }

    [JsonPropertyName("intermediate_size")]
    required public int IntermediateSize { get; init; }

    [JsonPropertyName("label2id")]
    required public Dictionary<string, int> LabelToId { get; init; }

    [JsonPropertyName("layer_norm_eps")]
    required public double LayerNormEps { get; init; }

    [JsonPropertyName("max_position_embeddings")]
    required public int MaxPositionEmbeddings { get; init; }

    [JsonPropertyName("model_type")]
    required public string ModelType { get; init; }

    [JsonPropertyName("num_attention_heads")]
    required public int NumAttentionHeads { get; init; }

    [JsonPropertyName("num_hidden_layers")]
    required public int NumHiddenLayers { get; init; }

    [JsonPropertyName("pad_token_id")]
    required public int PadTokenId { get; init; }

    [JsonPropertyName("position_embedding_type")]
    required public string PositionEmbeddingType { get; init; }

    [JsonPropertyName("transformers_version")]
    required public string TransformersVersion { get; init; }

    [JsonPropertyName("type_vocab_size")]
    required public int TypeVocabSize { get; init; }

    [JsonPropertyName("use_cache")]
    required public bool UseCache { get; init; }

    [JsonPropertyName("vocab_size")]
    required public int VocabSize { get; init; }
}
