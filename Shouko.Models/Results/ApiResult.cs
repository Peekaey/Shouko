
namespace Shouko.Models.Results;

public class ApiResult<T>
{
    public bool IsSuccess { get; set; }
    public T Response { get; set; }
    public string? ErrorMessage { get; set; }
    public List<string> DiscordResponseMessage { get; set; }
    
    public ApiResult(bool isSuccess, T response, string? errorMessage = null)
    {
        IsSuccess = isSuccess;
        Response = response;
        ErrorMessage = errorMessage;
    }

    public static ApiResult<T> AsSuccess(T response)
    {
        return new ApiResult<T>(true, response);
    }
    
    public static ApiResult<T> AsFailure(string errorMessage)
    {
        return new ApiResult<T>(false, default!, errorMessage);
    }
    
    
}