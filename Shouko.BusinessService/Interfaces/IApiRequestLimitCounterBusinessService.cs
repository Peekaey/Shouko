using Shouko.Models.DatabaseModels;
using Shouko.Models.Enums;
using Shouko.Models.Results;

namespace Shouko.BusinessService.Interfaces;

public interface IApiRequestLimitCounterBusinessService
{
    SaveResult SaveAndReturnId(LimitCounterType apiRequestLimitCounterType, ApiPromptType apiPromptType,
        ApiType apiType);
    ServiceResult Archive(LimitCounterType apiRequestLimitCounterType, ApiPromptType apiPromptType, ApiType apiType);

    ApiRequestLimitCounter? Get(LimitCounterType apiRequestLimitCounterType, ApiPromptType apiPromptType, ApiType apiType, bool includeArchived = false);
    ApiRequestLimitCounter Reset(LimitCounterType apiRequestLimitCounterType, ApiPromptType apiPromptType,
        ApiType apiType);

    bool IsMinuteRequestLimitReached(ApiPromptType apiPromptType, ApiType apiType);
    bool IsDailyRequestLimitReached(ApiPromptType apiPromptType, ApiType apiType);
}