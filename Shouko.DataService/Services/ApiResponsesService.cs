
using Microsoft.Extensions.Logging;
using Shouko.DataService;
using Shouko.Models.DatabaseModels;
using Shouko.Models.Results;

public class ApiResponsesService : IApiResponsesService
{
    private readonly DataContext _dataContext;
    private readonly ILogger<DiscordInteractionsService> _logger;
    
    public ApiResponsesService(DataContext dataContext, ILogger<DiscordInteractionsService> logger)
    {
        _dataContext = dataContext;
        _logger = logger;
    }
    public ServiceResult Save(ApiResponse apiResponse)
    {
        try
        {
            using (var transaction = _dataContext.Database.BeginTransaction())
            {
                _dataContext.ApiResponses.Add(apiResponse);
                _dataContext.SaveChanges();
                transaction.Commit();
                return ServiceResult.AsSuccess();
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error saving ApiResponse with interaction Id {apiResponse.DiscordInteractionId}", apiResponse.DiscordInteractionId);
            return ServiceResult.AsFailure("Failed to save ApiResponse.");
        }
    }
    
    public ApiResponseSaveResult SaveAndReturnId(ApiResponse apiResponse)
    {
        try
        {
            using (var transaction = _dataContext.Database.BeginTransaction())
            {
                _dataContext.ApiResponses.Add(apiResponse);
                _dataContext.SaveChanges();
                transaction.Commit();
                return ApiResponseSaveResult.AsSuccess(apiResponse.Id);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error saving ApiResponse with interaction Id {apiResponse.DiscordInteractionId}", apiResponse.DiscordInteractionId);
            return ApiResponseSaveResult.AsFailure("Failed to save ApiResponse.");
        }
    }
    
}