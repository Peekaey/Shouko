using Microsoft.Extensions.Logging;
using Shouko.Api.Interfaces;
using Shouko.BusinessService.Interfaces;
using Shouko.Helpers.Extensions;
using Shouko.Models.DatabaseModels;
using Shouko.Models.DTOs;
using Shouko.Models.Results;

namespace Shouko.BusinessService.Services;

public class DiscordInteractionsBusinessService : IDiscordInteractionsBusinessService
{
    private readonly ILogger<DiscordInteractionsBusinessService> _logger;
    private readonly IDiscordInteractionsService _discordInteractionsService;

    
    public DiscordInteractionsBusinessService(IDiscordInteractionsService discordInteractionsService,
        ILogger<DiscordInteractionsBusinessService> logger)
    {
        _logger = logger;
        _discordInteractionsService = discordInteractionsService;
    }
    
    public SaveResult SaveDiscordInteraction(ulong interactionId, ulong channelId, ulong guildId, ulong userId)
    {
        var interaction = new DiscordInteraction
        {
            InteractionId = interactionId,
            ChannelId = channelId,
            GuildId = guildId,
            UserId = userId,
            CreatedAtUtc = DateTime.UtcNow
        };
        var saveResult = _discordInteractionsService.SaveAndReturnId(interaction);
        return saveResult;
    }
}