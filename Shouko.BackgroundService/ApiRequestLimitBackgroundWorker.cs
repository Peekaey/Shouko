using Microsoft.Extensions.Logging;
using Shouko.BusinessService.Interfaces;
using Shouko.Models;
using Shouko.Models.Enums;

namespace Shouko.BackgroundService;

public class ApiRequestLimitBackgroundWorker : Microsoft.Extensions.Hosting.BackgroundService
{
    private readonly ILogger<ApiRequestLimitBackgroundWorker> _logger;
    private readonly IApiRequestLimitCounterBusinessService _apiRequestLimitCounterBusinessService;
    private readonly ApplicationConfigurationSettings _applicationConfigurationSettings;
    

    public ApiRequestLimitBackgroundWorker(ILogger<ApiRequestLimitBackgroundWorker> logger, IApiRequestLimitCounterBusinessService apiRequestLimitCounterBusinessService,
        ApplicationConfigurationSettings applicationConfigurationSettings)
    {
        _logger = logger;
        _apiRequestLimitCounterBusinessService = apiRequestLimitCounterBusinessService;
        _applicationConfigurationSettings = applicationConfigurationSettings;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Generate Persisted Records
        if (_applicationConfigurationSettings.EnableGeminiTextModelDailyLimit)
        {
            var dailyGeminiTextModelCounter = _apiRequestLimitCounterBusinessService.Get(LimitCounterType.Daily, ApiPromptType.Text, ApiType.Gemini);
            if (dailyGeminiTextModelCounter == null)
            {
                _apiRequestLimitCounterBusinessService.SaveAndReturnId(LimitCounterType.Daily, ApiPromptType.Text, ApiType.Gemini);
                _logger.LogInformation("Creating initial Gemini Text model daily limit counter.");
            }
            else
            {
                _logger.LogInformation("Gemini Text model daily limit counter already exists.");
            }
        }
        if (_applicationConfigurationSettings.EnableGeminiTextModelMinuteLimit)
        {
            var minuteGeminiTextModelCounter = _apiRequestLimitCounterBusinessService.Get(LimitCounterType.Minute, ApiPromptType.Text, ApiType.Gemini);
            if (minuteGeminiTextModelCounter == null)
            {
                _apiRequestLimitCounterBusinessService.SaveAndReturnId(LimitCounterType.Minute, ApiPromptType.Text, ApiType.Gemini);
                _logger.LogInformation("Creating initial Gemini Text model minute limit counter.");
            }
            else
            {
                _logger.LogInformation("Gemini Text model minute limit counter already exists.");
            }
        }
        if (_applicationConfigurationSettings.EnableGeminiImageModelDailyLimit)
        {
            var dailyGeminiImageModelCounter = _apiRequestLimitCounterBusinessService.Get(LimitCounterType.Daily, ApiPromptType.Image, ApiType.Gemini);
            if (dailyGeminiImageModelCounter == null)
            {
                _apiRequestLimitCounterBusinessService.SaveAndReturnId(LimitCounterType.Daily, ApiPromptType.Image, ApiType.Gemini);
                _logger.LogInformation("Creating initial Gemini Image model daily limit counter.");
            }
            else
            {
                _logger.LogInformation("Gemini Image model daily limit counter already exists.");
            }
        }
        if (_applicationConfigurationSettings.EnableGeminiImageModelMinuteLimit)
        {
            var minuteGeminiImageModelCounter = _apiRequestLimitCounterBusinessService.Get(LimitCounterType.Minute, ApiPromptType.Image, ApiType.Gemini);
            if (minuteGeminiImageModelCounter == null)
            {
                _apiRequestLimitCounterBusinessService.SaveAndReturnId(LimitCounterType.Minute, ApiPromptType.Image, ApiType.Gemini);
                _logger.LogInformation("Creating initial Gemini Image model minute limit counter.");
            }
            else
            {
                _logger.LogInformation("Gemini Image model minute limit counter already exists.");
            }
        }
        
        
        if (_applicationConfigurationSettings.EnableGeminiImageModelDailyLimit == true)
        {
            var dailyCounterStart = _apiRequestLimitCounterBusinessService.Get(LimitCounterType.Daily, ApiPromptType.Image, ApiType.Gemini);
            
            if (dailyCounterStart != null)
            {
                var startDateTime = dailyCounterStart.StartDateTime;
                    
                // Compute next reset time
                var nextResetTime = startDateTime.Date.AddDays(1);
                if (nextResetTime <= DateTime.UtcNow)
                {
                    // If we've missed today's reset (e.g. process restarted), do it now
                    nextResetTime = DateTime.UtcNow;
                }
                    
                var delay = nextResetTime - DateTime.UtcNow;
                if (delay < TimeSpan.Zero)
                    delay = TimeSpan.Zero;
                
                _ = Task.Run(async () =>
                {
                    // Delay for the remaining time until the next execution
                    await Task.Delay(delay, stoppingToken);
                    
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        // Execute the task
                        _logger.LogInformation("Resetting Gemini Image model daily limit counter at: {time}", DateTimeOffset.Now);
                        var updatedImageDailyLimit = _apiRequestLimitCounterBusinessService.Reset(LimitCounterType.Daily, ApiPromptType.Image, ApiType.Gemini);
                        startDateTime = updatedImageDailyLimit.StartDateTime;
                        
                        await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
                    }
                }, stoppingToken);
            }
            else
            {
                _logger.LogError("Failed to retrieve Gemini Image model daily limit counter.");
            }
        }
        
        if (_applicationConfigurationSettings.EnableGeminiImageModelMinuteLimit == true)
        {
            var minuteCounterStart = _apiRequestLimitCounterBusinessService.Get(LimitCounterType.Minute, ApiPromptType.Image, ApiType.Gemini);
            
            if (minuteCounterStart != null)
            {
                var startDateTime = minuteCounterStart.StartDateTime;
                    
                // Compute next reset time
                var nextResetTime = startDateTime.AddMinutes(1);
                if (nextResetTime <= DateTime.UtcNow)
                {
                    // If we've missed this minute's reset (e.g. process restarted), do it now
                    nextResetTime = DateTime.UtcNow;
                }
                    
                var delay = nextResetTime - DateTime.UtcNow;
                if (delay < TimeSpan.Zero)
                    delay = TimeSpan.Zero;
                
                _ = Task.Run(async () =>
                {
                    // Delay for the remaining time until the next execution
                    await Task.Delay(delay, stoppingToken);
                    
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        // Execute the task
                        _logger.LogInformation("Resetting Gemini Image model minute limit counter at: {time}", DateTimeOffset.Now);
                        var updatedImageMinuteLimit = _apiRequestLimitCounterBusinessService.Reset(LimitCounterType.Minute, ApiPromptType.Image, ApiType.Gemini);
                        startDateTime = updatedImageMinuteLimit.StartDateTime;
                        
                        await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                    }
                }, stoppingToken);
            }
            else
            {
                _logger.LogError("Failed to retrieve Gemini Image model minute limit counter.");
            }
        }
        
        if (_applicationConfigurationSettings.EnableGeminiTextModelDailyLimit == true) 
        {
            var dailyCounterStart = _apiRequestLimitCounterBusinessService.Get(LimitCounterType.Daily, ApiPromptType.Text, ApiType.Gemini);
            
            if (dailyCounterStart != null)
            {
                var startDateTime = dailyCounterStart.StartDateTime;
                    
                // Compute next reset time
                var nextResetTime = startDateTime.Date.AddDays(1);
                if (nextResetTime <= DateTime.UtcNow)
                {
                    // If we've missed today's reset (e.g. process restarted), do it now
                    nextResetTime = DateTime.UtcNow;
                }
                    
                var delay = nextResetTime - DateTime.UtcNow;
                if (delay < TimeSpan.Zero)
                    delay = TimeSpan.Zero;
                
                _ = Task.Run(async () =>
                {
                    // Delay for the remaining time until the next execution
                    await Task.Delay(delay, stoppingToken);
                    
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        // Execute the task
                        _logger.LogInformation("Resetting Gemini Text model daily limit counter at: {time}", DateTimeOffset.Now);
                        var updatedTextDailyLimit = _apiRequestLimitCounterBusinessService.Reset(LimitCounterType.Daily, ApiPromptType.Text, ApiType.Gemini);
                        startDateTime = updatedTextDailyLimit.StartDateTime;
                        
                        await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
                    }
                }, stoppingToken);
            }
            else
            {
                _logger.LogError("Failed to retrieve Gemini Text model daily limit counter.");
            }
        }
        
        if (_applicationConfigurationSettings.EnableGeminiTextModelMinuteLimit == true) 
        {
            var minuteCounterStart = _apiRequestLimitCounterBusinessService.Get(LimitCounterType.Minute, ApiPromptType.Text, ApiType.Gemini);
            
            if (minuteCounterStart != null)
            {
                var startDateTime = minuteCounterStart.StartDateTime;
                    
                // Compute next reset time
                var nextResetTime = startDateTime.AddMinutes(1);
                if (nextResetTime <= DateTime.UtcNow)
                {
                    // If we've missed this minute's reset (e.g. process restarted), do it now
                    nextResetTime = DateTime.UtcNow;
                }
                    
                var delay = nextResetTime - DateTime.UtcNow;
                if (delay < TimeSpan.Zero)
                    delay = TimeSpan.Zero;
                
                _ = Task.Run(async () =>
                {
                    // Delay for the remaining time until the next execution
                    await Task.Delay(delay, stoppingToken);
                    
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        // Execute the task
                        _logger.LogInformation("Resetting Gemini Text model minute limit counter at: {time}", DateTimeOffset.Now);
                        var updatedTextMinuteLimit = _apiRequestLimitCounterBusinessService.Reset(LimitCounterType.Minute, ApiPromptType.Text, ApiType.Gemini);
                        startDateTime = updatedTextMinuteLimit.StartDateTime;
                        
                        await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                    }
                }, stoppingToken);
            }
            else
            {
                _logger.LogError("Failed to retrieve Gemini Text model minute limit counter.");
            }
        }
        
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}