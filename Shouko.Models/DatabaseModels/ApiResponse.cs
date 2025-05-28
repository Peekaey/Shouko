using Shouko.Models.Enums;

namespace Shouko.Models.DatabaseModels;

public class ApiResponse
{
    public int Id { get; set; }
    public long InteractionId { get; set; }
    public DiscordInteraction Interaction { get; set; }
    public bool IsSuccess { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    // Config
    public ApiType ApiType { get; set; }
    public string Model { get; set; }
    
    // Input
    public string InputText { get; set; }
    
    // From API
    public string ResponseText { get; set; }
    public string ResponseId { get; set; }
    // Type of Prompt
    public ApiPromptType PromptTokenDetails { get; set; }
    public int CandidatesTokenCount { get; set; }
    public int PromptTokenCount { get; set; }
}