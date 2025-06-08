using Microsoft.Extensions.Logging;
using Shouko.BusinessService.Interfaces;
using Shouko.DataService.Interfaces;
using Shouko.Models.DatabaseModels;
using Shouko.Models.Enums;
using Shouko.Models.Results;

namespace Shouko.BusinessService.Services;

public class ApiRequestCountersBusinessService : IApiRequestCountersBusinessService
{
    private readonly ILogger<ApiRequestCountersBusinessService> _logger;
    private readonly IApiRequestCountersService _apiRequestCountersService;
    
    public ApiRequestCountersBusinessService(IApiRequestCountersService apiRequestCountersService, ILogger<ApiRequestCountersBusinessService> logger)
    {
        _apiRequestCountersService = apiRequestCountersService;
        _logger = logger;
    }

    public SaveResult SaveAndReturnId(ApiPromptType apiPromptType, ApiType apiType)
    {
        var apiRequestCounter = new ApiRequestCounter
        {
            ApiPromptType = apiPromptType,
            ApiType = apiType,
            CreatedAtUtc = DateTime.UtcNow
        };
        return _apiRequestCountersService.SaveAndReturnId(apiRequestCounter);
    }

    public bool IsDailyApiLimitReached()
    {
        return false;
    }

    public bool IsMinuteApiLimitReached()
    {
        return false;
    }

    IQueryable<ApiRequestCounter> GetAllByType(ApiPromptType apiPromptType, ApiType apiType)
    {
        return _apiRequestCountersService.GetAllByType(apiPromptType, apiType);
    }
    
}