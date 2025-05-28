
using Shouko.Models.DatabaseModels;
using Shouko.Models.Results;

public interface IApiResponseBusinessService
{
    ServiceResult Save(ApiResponse apiResponse);
}