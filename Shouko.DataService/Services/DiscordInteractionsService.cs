
using Microsoft.Extensions.Logging;
using Shouko.DataService;
using Shouko.Models.DatabaseModels;
using Shouko.Models.Results;

public class DiscordInteractionService : IDiscordInteractionsService
{
    private readonly DataContext _dataContext;
    private readonly ILogger<DiscordInteractionService> _logger;
    
    public DiscordInteractionService(DataContext dataContext, ILogger<DiscordInteractionService> logger)
        {
            _dataContext = dataContext;
            _logger = logger;
        }


    public ServiceResult Save(DiscordInteraction interaction)
    {

        return ServiceResult.AsSuccess();
    }

    public long? GetStoredInteraction(long interactionId)
    {
        return _dataContext.DiscordInteractions
            .Where(di => di.InteractionId == interactionId)
            .Select(di => di.InteractionId)
            .FirstOrDefault();
    }
    
    
}