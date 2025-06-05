namespace Shouko.Models.Results;

public class DiscordInteractionSaveResult : ServiceResult
{
    public int? Id { get; set; }
    
    public DiscordInteractionSaveResult(bool isSuccess, int? id, string? errorMessage = null) : base(isSuccess, errorMessage)
    {
        Id = id;
    }
    
    public static DiscordInteractionSaveResult AsSuccess(int id)
    {
        return new DiscordInteractionSaveResult(true, id);
    }
    
    public static DiscordInteractionSaveResult AsFailure(string errorMessage)
    {
        return new DiscordInteractionSaveResult(false, null, errorMessage);
    }
}