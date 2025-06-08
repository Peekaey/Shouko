
using Shouko.Models.DatabaseModels;
using Shouko.Models.Enums;
using Shouko.Models.Results;

public interface IApiRequestLimitCounterService
{
    SaveResult SaveAndReturnId(ApiRequestLimitCounter apiRequestLimitCounter);
    ServiceResult Archive(LimitCounterType apiRequestLimitCounterType, ApiPromptType apiPromptType, ApiType apiType);

    ApiRequestLimitCounter? Get(LimitCounterType apiRequestLimitCounterType, ApiPromptType apiPromptType, ApiType apiType, bool includeArchived = false);
    ServiceResult Archive(ApiRequestLimitCounter apiRequestLimitCounter);
}