using Microsoft.Extensions.Logging;
using Shouko.DataService.Interfaces;
using Shouko.Models.DatabaseModels;
using Shouko.Models.Enums;
using Shouko.Models.Results;

namespace Shouko.DataService.Services;

public class ApiRequestLimitCounterService : IApiRequestLimitCounterService
{
    private readonly ILogger<ApiRequestLimitCounterService> _logger;
    private readonly DataContext _dataContext;

    public ApiRequestLimitCounterService(ILogger<ApiRequestLimitCounterService> logger, DataContext dataContext)
    {
        _logger = logger;
        _dataContext = dataContext;
    }

    public SaveResult SaveAndReturnId(ApiRequestLimitCounter apiRequestLimitCounter)
    {
        try
        {
            using (var transaction = _dataContext.Database.BeginTransaction())
            {
                _dataContext.ApiRequestLimitCounters.Add(apiRequestLimitCounter);
                _dataContext.SaveChanges();
                transaction.Commit();
                return SaveResult.AsSuccess(apiRequestLimitCounter.Id);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error saving ApiRequestStartCounter with Id {apiRequestLimitCounter.Id}", apiRequestLimitCounter.Id);
            return SaveResult.AsFailure("Failed to save ApiRequestStartCounter.");
        }
    }

    public ServiceResult Archive(LimitCounterType apiRequestLimitCounterType, ApiPromptType apiPromptType, ApiType apiType)
    {
        try
        {
            using (var transaction = _dataContext.Database.BeginTransaction())
            {
                var counter = _dataContext.ApiRequestLimitCounters.FirstOrDefault(c => c.LimitCounterType == apiRequestLimitCounterType &&
                                c.ApiPromptType == apiPromptType &&
                                c.ApiType == apiType &&
                                c.IsArchived == false);

                if (counter != null)
                {
                    counter.IsArchived = true;
                    _dataContext.SaveChanges();
                    transaction.Commit();
                }
                else
                {
                    _logger.LogWarning($"No ApiRequestStartCounter found for CounterType {apiRequestLimitCounterType}");
                }

                return ServiceResult.AsSuccess();
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error deleting ApiRequestStartCounter with CounterType {apiRequestLimitCounterType}");
            return ServiceResult.AsFailure("Failed to delete ApiRequestStartCounter.");
        }
    }

    public ApiRequestLimitCounter? Get(LimitCounterType apiRequestLimitCounterType, ApiPromptType apiPromptType, ApiType apiType, bool includeArchived = false)
    {
        try 
        {
            var limitCounter = _dataContext.ApiRequestLimitCounters.FirstOrDefault(c => c.LimitCounterType == apiRequestLimitCounterType &&
                                                                        c.ApiPromptType == apiPromptType &&
                                                                        c.ApiType == apiType &&
                                                                        c.IsArchived == includeArchived);
            if (limitCounter == null)
            {
                _logger.LogWarning($"No ApiRequestStartCounter found for CounterType {apiRequestLimitCounterType}");
                return null;
            }
            return limitCounter;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error retrieving ApiRequestStartCounter with CounterType {apiRequestLimitCounterType}");
            return null;
        }
    }

    public ServiceResult Archive(ApiRequestLimitCounter apiRequestLimitCounter)
    {
        try
        {
            using (var transaction = _dataContext.Database.BeginTransaction())
            {
                apiRequestLimitCounter.IsArchived = true;
                _dataContext.ApiRequestLimitCounters.Update(apiRequestLimitCounter);
                _dataContext.SaveChanges();
                transaction.Commit();
                return ServiceResult.AsSuccess();
            }
        } catch (Exception e)
        {
            _logger.LogError(e, $"Error archiving ApiRequestLimitCounter with Id {apiRequestLimitCounter.Id}");
            return ServiceResult.AsFailure("Failed to archive ApiRequestLimitCounter.");
        }
    }
    
}