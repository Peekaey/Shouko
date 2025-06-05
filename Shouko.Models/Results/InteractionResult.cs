namespace Shouko.Models.Results;

public class InteractionResult : ServiceResult
{
    public List<string> Responses { get; set; }
    public ApiPromptType PromptType { get; set; }
    public MemoryStream Stream { get; set; }
    public InteractionResult(bool success, ApiPromptType? apiPromptType, string? errorMessage = null) : base(success, errorMessage)
    {
        Responses = new List<string>();
    }
    
    public static InteractionResult AsTextSuccess(List<string> responses, ApiPromptType apiPromptType)
    {
        return new InteractionResult(true, apiPromptType)
        {
            Responses = responses,
            PromptType = apiPromptType
        };
    }

    public new static InteractionResult AsTextFailure(string errorMessage, ApiPromptType apiPromptType)
    {
        return new InteractionResult(false, apiPromptType, errorMessage)
        {
            Responses = new List<string>(),
            PromptType = apiPromptType
        };
    }
    
    public static InteractionResult AsFileSuccess(MemoryStream memoryStream, ApiPromptType apiPromptType)
    {
        return new InteractionResult(true, apiPromptType)
        {
            Stream = memoryStream,
            Responses = new List<string>(),
            PromptType = apiPromptType
        };
    }
    
    public new static InteractionResult AsFileFailure(string errorMessage, ApiPromptType apiPromptType)
    {
        return new InteractionResult(false, apiPromptType, errorMessage)
        {
            Responses = new List<string>(),
            PromptType = apiPromptType
        };
    }
}