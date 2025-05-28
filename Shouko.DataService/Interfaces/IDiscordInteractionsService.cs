

using Shouko.Models.DatabaseModels;
using Shouko.Models.Results;

public interface IDiscordInteractionsService
{
    long? GetStoredInteraction(long interactionId);
    ServiceResult Save(DiscordInteraction interaction);
}