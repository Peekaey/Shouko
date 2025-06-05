namespace Shouko.Models.Results;

public class ApiResponseSaveResult : ServiceResult
{
    public int? Id { get; set; }
    
    public ApiResponseSaveResult(bool isSuccess, int? id, string? errorMessage = null) : base(isSuccess, errorMessage)
    {
        Id = id;
    }
    
    public static ApiResponseSaveResult AsSuccess(int id)
    {
        return new ApiResponseSaveResult(true, id);
    }
    
    public static ApiResponseSaveResult AsFailure(string errorMessage)
    {
        return new ApiResponseSaveResult(false, null, errorMessage);
    }
    
    
    
}