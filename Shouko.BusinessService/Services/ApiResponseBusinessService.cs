
// DB Entity Business Class

using Shouko.Models.DatabaseModels;
using Shouko.Models.Results;

public class ApiResponseBusinessService : IApiResponseBusinessService
{
    public ServiceResult Save(ApiResponse apiResponse)
    {
        return ServiceResult.AsSuccess();
    }
    
} 