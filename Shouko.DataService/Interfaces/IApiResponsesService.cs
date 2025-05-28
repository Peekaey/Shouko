
using Shouko.Models.DatabaseModels;
using Shouko.Models.Results;

public interface IApiResponsesService
{
    ServiceResult Save(ApiResponse apiResponse);
}