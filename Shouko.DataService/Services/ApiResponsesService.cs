
using Shouko.Models.DatabaseModels;
using Shouko.Models.Results;

public class ApiResponsesService : IApiResponsesService
{
    public ServiceResult Save(ApiResponse apiResponse)
    {
        return ServiceResult.AsSuccess();
    }
}