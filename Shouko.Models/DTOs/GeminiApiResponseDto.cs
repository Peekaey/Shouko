
public class GeminiApiResponseDto
{
    public List<string> DiscordResponseMessage { get; set; }
    public string ResponseContent { get; set; }
    public string MimeType { get; set; }
    public string ResponseId { get; set; }
    public bool IsSuccess { get; set; }
    public string Model { get; set; }
    public int CandidatesTokenCount { get; set; }
    public int PromptTokenCount { get; set; }
}