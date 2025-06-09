using Shouko.Models.DatabaseModels;
using Shouko.Models.Enums;
using Shouko.Models.Results;

namespace Shouko.BusinessService.Interfaces;

public interface IApiRequestCountersBusinessService
{
    SaveResult SaveAndReturnId(ApiPromptType apiPromptType, ApiType apiType);
    IQueryable<ApiRequestCounter> GetAllByType(ApiPromptType apiPromptType, ApiType apiType);
}