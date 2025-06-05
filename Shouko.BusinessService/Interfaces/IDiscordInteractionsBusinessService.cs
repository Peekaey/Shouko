using Shouko.Models.DTOs;
using Shouko.Models.Results;

namespace Shouko.BusinessService.Interfaces;

public interface IDiscordInteractionsBusinessService
{
    Task<InteractionResult> CreateApiTextThread<T>(CreateThreadDto createThreadDto) where T : class;
    SaveResult SaveDiscordInteraction(ulong interactionId, ulong channelId, ulong guildId, ulong userId);
    Task<InteractionResult> CreateApiImageThread<T>(CreateThreadDto createThreadDto) where T : class;

}