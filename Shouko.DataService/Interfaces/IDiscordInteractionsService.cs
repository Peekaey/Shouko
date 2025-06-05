

using Shouko.Models.DatabaseModels;
using Shouko.Models.Results;

public interface IDiscordInteractionsService
{
    ulong? GetStoredInteraction(ulong interactionId);
    ServiceResult Save(DiscordInteraction interaction);
    SaveResult SaveAndReturnId(DiscordInteraction interaction);
}