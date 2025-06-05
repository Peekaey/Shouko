
using System.Transactions;
using Microsoft.Extensions.Logging;
using Shouko.DataService;
using Shouko.Models.DatabaseModels;
using Shouko.Models.Results;

public class DiscordInteractionsService : IDiscordInteractionsService
{
    private readonly DataContext _dataContext;
    private readonly ILogger<DiscordInteractionsService> _logger;
    
    public DiscordInteractionsService(DataContext dataContext, ILogger<DiscordInteractionsService> logger)
        {
            _dataContext = dataContext;
            _logger = logger;
        }


    public ServiceResult Save(DiscordInteraction interaction)
    {
        try
        {
            using (var transaction = _dataContext.Database.BeginTransaction())
            {
                _dataContext.DiscordInteractions.Add(interaction);
                _dataContext.SaveChanges();
                transaction.Commit();
                return ServiceResult.AsSuccess();
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error saving Discord interaction with ID {interaction.InteractionId}", interaction.InteractionId);
            return ServiceResult.AsFailure("Failed to save Discord interaction.");
            
        }
    }

    public SaveResult SaveAndReturnId(DiscordInteraction interaction)
    {
        try
        {
            using (var transaction = _dataContext.Database.BeginTransaction())
            {
                _dataContext.DiscordInteractions.Add(interaction);
                _dataContext.SaveChanges();
                transaction.Commit();
                return SaveResult.AsSuccess(interaction.Id);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error saving Discord interaction with ID {interaction.InteractionId}", interaction.InteractionId);
            return SaveResult.AsFailure("Failed to save Discord interaction.");
            
        }
    }

    public ulong? GetStoredInteraction(ulong interactionId)
    {
        return _dataContext.DiscordInteractions
            .Where(di => di.InteractionId == interactionId)
            .Select(di => di.InteractionId)
            .FirstOrDefault();
    }
    
    
}