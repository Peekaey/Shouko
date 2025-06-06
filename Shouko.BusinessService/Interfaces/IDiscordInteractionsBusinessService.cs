using Shouko.Models.DTOs;
using Shouko.Models.Results;

namespace Shouko.BusinessService.Interfaces;

public interface IDiscordInteractionsBusinessService
{
    SaveResult SaveDiscordInteraction(ulong interactionId, ulong channelId, ulong guildId, ulong userId);
}