using Microsoft.Extensions.Logging;
using Shouko.BusinessService.Interfaces;
using Shouko.DataService.Interfaces;
using Shouko.Models;
using Shouko.Models.DatabaseModels;
using Shouko.Models.Enums;
using Shouko.Models.Results;

namespace Shouko.BusinessService.Services;

public class ApiRequestLimitCounterBusinessService : IApiRequestLimitCounterBusinessService
{
    private readonly IApiRequestLimitCounterService _apiRequestLimitCounterService;
    private readonly IApiRequestCountersService _apiRequestCountersService;
    private readonly ILogger<ApiRequestLimitCounterBusinessService> _logger;
    private readonly ApplicationConfigurationSettings _applicationConfigurationSettings;
    
    public ApiRequestLimitCounterBusinessService(IApiRequestLimitCounterService apiRequestLimitCounterService, ILogger<ApiRequestLimitCounterBusinessService> logger,
        IApiRequestCountersService apiRequestCountersService, ApplicationConfigurationSettings applicationConfigurationSettings)
    {
        _apiRequestLimitCounterService = apiRequestLimitCounterService;
        _logger = logger;
        _apiRequestCountersService = apiRequestCountersService;
        _applicationConfigurationSettings = applicationConfigurationSettings;
    }

    public SaveResult SaveAndReturnId(LimitCounterType apiRequestLimitCounterType, ApiPromptType apiPromptType, ApiType apiType)
    {
        var requestCounter = new ApiRequestLimitCounter
        {
            LimitCounterType = apiRequestLimitCounterType,
            StartDateTime = DateTime.UtcNow,
            ApiPromptType = apiPromptType,
            ApiType = apiType,
        };
        return _apiRequestLimitCounterService.SaveAndReturnId(requestCounter);
    }

    public ServiceResult Archive(LimitCounterType apiRequestLimitCounterType, ApiPromptType apiPromptType, ApiType apiType)
    {
        return _apiRequestLimitCounterService.Archive(apiRequestLimitCounterType, apiPromptType, apiType);
    }
    
    public ApiRequestLimitCounter? Get(LimitCounterType apiRequestLimitCounterType, ApiPromptType apiPromptType, ApiType apiType, bool includeArchived = false)
    {
        return _apiRequestLimitCounterService.Get(apiRequestLimitCounterType, apiPromptType, apiType, includeArchived);
    }

    public ApiRequestLimitCounter Reset(LimitCounterType apiRequestLimitCounterType, ApiPromptType apiPromptType,
        ApiType apiType)
    {
        var apiRequestLimitCounter = Get(apiRequestLimitCounterType, apiPromptType, apiType);
        var apiRequestCounters = _apiRequestCountersService.GetAllByType(apiPromptType, apiType);
        var archiveResult = _apiRequestCountersService.ArchiveList(apiRequestCounters.ToList());
        var archiveLimitResult = Archive(apiRequestLimitCounter!);
        var newApiRequestLimitCounter = new ApiRequestLimitCounter
        {
            LimitCounterType = apiRequestLimitCounterType,
            StartDateTime = DateTime.UtcNow,
            ApiPromptType = apiPromptType,
            ApiType = apiType,
        };
        var saveResult = _apiRequestLimitCounterService.SaveAndReturnId(newApiRequestLimitCounter);
        return newApiRequestLimitCounter;
    }
    
    ServiceResult Archive(ApiRequestLimitCounter apiRequestLimitCounter)
    {
        return _apiRequestLimitCounterService.Archive(apiRequestLimitCounter);
    }

    public bool IsMinuteRequestLimitReached(ApiPromptType apiPromptType, ApiType apiType)
    {
        var minuteRequestLimitCounter = Get(LimitCounterType.Minute, apiPromptType, apiType);
        
        if (minuteRequestLimitCounter == null)
        {
            // Means that no minutely limit was set
            return false;
        }
        
        var counters = _apiRequestCountersService.GetAllByType(apiPromptType, apiType);
        
        if (apiPromptType == ApiPromptType.Text && apiType == ApiType.Gemini) 
        {
            if (counters.Count() >= _applicationConfigurationSettings.GeminiTextModelMinuteLimit)
            {
                _logger.LogWarning("Minutely request limit reached for Gemini Text API.");
                return true;
            }
        }
        
        if (apiPromptType == ApiPromptType.Image && apiType == ApiType.Gemini) 
        {
            if (counters.Count() >= _applicationConfigurationSettings.GeminiImageModelMinuteLimit)
            {
                _logger.LogWarning("Minute request limit reached for Gemini Image API.");
                return true;
            }
        }
        return false;
    }

    public bool IsDailyRequestLimitReached(ApiPromptType apiPromptType, ApiType apiType)
    {
        var dailyRequestLimitCounter = Get(LimitCounterType.Daily, apiPromptType, apiType);
        if (dailyRequestLimitCounter == null)
        {
            // Means that no daily limit was set
            return false;
        }
        var counters = _apiRequestCountersService.GetAllByType(apiPromptType, apiType);

        if (apiPromptType == ApiPromptType.Text && apiType == ApiType.Gemini) 
        {
            if (counters.Count() >= _applicationConfigurationSettings.GeminiTextModelDailyLimit)
            {
                _logger.LogWarning("Daily request limit reached for Gemini Text API.");
                return true;
            }
        }
        
        if (apiPromptType == ApiPromptType.Image && apiType == ApiType.Gemini) 
        {
            if (counters.Count() >= _applicationConfigurationSettings.GeminiImageModelDailyLimit)
            {
                _logger.LogWarning("Daily request limit reached for Gemini Image API.");
                return true;
            }
        }
        return false;
    }
}