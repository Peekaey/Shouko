namespace Shouko.Models.Results;

public class SaveResult
{
    public bool Success { get; set; }
    public int? SavedEntityId { get; set; }
    public string? ErrorMessage { get; set; }
    
    public SaveResult(bool isSuccess, int? savedEntityId, string? errorMessage = null)
    {
        Success = isSuccess;
        SavedEntityId = savedEntityId;
        ErrorMessage = errorMessage;
    }
    
    public static SaveResult AsSuccess(int savedEntityId)
    {
        return new SaveResult(true, savedEntityId);
    }
    
    public static SaveResult AsFailure(string errorMessage)
    {
        return new SaveResult(false, null, errorMessage);
    }
}