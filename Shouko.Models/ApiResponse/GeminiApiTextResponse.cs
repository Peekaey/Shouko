

using System.Text.Json.Serialization;

public class GeminiApiTextResponse
{
    [JsonPropertyName("candidates")]
    public List<Candidate> Candidates { get; set; }

    [JsonPropertyName("usageMetadata")]
    public UsageMetadata UsageMetadata { get; set; }

    [JsonPropertyName("modelVersion")]
    public string ModelVersion { get; set; }

    [JsonPropertyName("responseId")]
    public string ResponseId { get; set; }
    
    
}

public class Candidate
{
    [JsonPropertyName("content")]
    public ContentContent Content { get; set; }

    // finishReason is only present on some candidates
    [JsonPropertyName("finishReason")]
    public string FinishReason { get; set; }
}

public class ContentContent
{
    [JsonPropertyName("parts")]
    public List<Part> Parts { get; set; }

    [JsonPropertyName("role")]
    public string Role { get; set; }
}

public class Part
{
    [JsonPropertyName("text")]
    public string Text { get; set; }
}

public class UsageMetadata
{
    [JsonPropertyName("promptTokenCount")]
    public int PromptTokenCount { get; set; }

    // this may be missing on some responses
    [JsonPropertyName("candidatesTokenCount")]
    public int? CandidatesTokenCount { get; set; }

    [JsonPropertyName("totalTokenCount")]
    public int TotalTokenCount { get; set; }

    [JsonPropertyName("promptTokensDetails")]
    public List<TokenDetail> PromptTokensDetails { get; set; }

    [JsonPropertyName("candidatesTokensDetails")]
    public List<TokenDetail> CandidatesTokensDetails { get; set; }
}

public class TokenDetail
{
    [JsonPropertyName("modality")]
    public string Modality { get; set; }

    [JsonPropertyName("tokenCount")]
    public int TokenCount { get; set; }
}
