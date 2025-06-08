using Microsoft.Extensions.Logging;
using Shouko.Models.DatabaseModels;
using Shouko.Models.Enums;
using Shouko.Models.Results;

namespace Shouko.DataService.Interfaces;

public interface IApiRequestCountersService
{
    SaveResult SaveAndReturnId(ApiRequestCounter apiRequestCounter);

    IQueryable<ApiRequestCounter> GetAllByType(ApiPromptType apiPromptType, ApiType apiType,
        bool includeArchived = false);
    ServiceResult ArchiveList(List<ApiRequestCounter> apiRequestCounters);
}