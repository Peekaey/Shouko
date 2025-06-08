using Shouko.Models.Enums;
using Shouko.Models.Results;

namespace Shouko.BusinessService.Interfaces;

public interface IApiRequestCountersBusinessService
{
    SaveResult SaveAndReturnId(ApiPromptType apiPromptType, ApiType apiType);
}