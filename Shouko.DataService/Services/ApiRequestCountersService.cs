using Microsoft.Extensions.Logging;
using Shouko.DataService.Interfaces;
using Shouko.Models.DatabaseModels;
using Shouko.Models.Enums;
using Shouko.Models.Results;

namespace Shouko.DataService.Services;

public class ApiRequestCountersService : IApiRequestCountersService
{
    private readonly ILogger<ApiRequestCountersService> _logger;
    private readonly DataContext _dataContext;
    
    public ApiRequestCountersService(DataContext dataContext, ILogger<ApiRequestCountersService> logger)
    {
        _dataContext = dataContext;
        _logger = logger;
    }
    
    public SaveResult SaveAndReturnId(ApiRequestCounter apiRequestCounter)
    {
        try
        {
            using (var transaction = _dataContext.Database.BeginTransaction())
            {
                _dataContext.ApiRequestCounters.Add(apiRequestCounter);
                _dataContext.SaveChanges();
                transaction.Commit();
                return SaveResult.AsSuccess(apiRequestCounter.Id);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error saving ApiRequestCounter with Id {apiRequestCounter.Id}", apiRequestCounter.Id);
            return SaveResult.AsFailure("Failed to save ApiRequestCounter.");
        }
    }

    public IQueryable<ApiRequestCounter> GetAllByType(ApiPromptType apiPromptType, ApiType apiType, bool includeArchived = false)
    {
        var counters =
            _dataContext.ApiRequestCounters.Where(arc => arc.ApiPromptType == apiPromptType && arc.ApiType == apiType
            && arc.IsArchived == includeArchived);
        return counters;
    }

    public ServiceResult ArchiveList(List<ApiRequestCounter> apiRequestCounters)
    {
        try
        {
            using (var transaction = _dataContext.Database.BeginTransaction())
            {
                var list = apiRequestCounters.ToList();
                foreach (var counter in list)
                {
                    counter.IsArchived = true;
                }
                _dataContext.ApiRequestCounters.UpdateRange(list);
                _dataContext.SaveChanges();
                transaction.Commit();
                return ServiceResult.AsSuccess();
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error archiving ApiRequestCounters");
            return ServiceResult.AsFailure("Failed to archive ApiRequestCounters.");
        }
        
    }
    
    
    

}