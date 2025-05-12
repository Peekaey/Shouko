using NetCord.Rest;
using NetCord.Services.ApplicationCommands;
using Shouko.Helpers.Extensions;

namespace Shouko.CommandModules.Interaction;

public class Ping : ApplicationCommandModule<SlashCommandContext>
{
    private readonly ILogger<Ping> _logger;
    private readonly ApplicationCommandService<SlashCommandContext> _commandService;

    public Ping(ILogger<Ping> logger, ApplicationCommandService<SlashCommandContext> commandService)
    {
        _logger = logger;
        _commandService = commandService;
    }

    [SlashCommand("ping", "Returns latency")]
    public async Task SendPing()
    {
        try
        {
            await Context.Interaction.SendResponseAsync(InteractionCallback.DeferredMessage());
            _logger.LogActionTraceStart(Context, "SendPing");

            var latency = Context.Client.Latency.Milliseconds;
            await Context.Interaction.SendFollowupMessageAsync(new InteractionMessageProperties
            {
                Content = $"Pong! Latency: {latency}ms"
            });
            _logger.LogActionTraceFinish(Context, "SendPing");
        }
        catch (Exception e)
        {
            _logger.LogExceptionError(Context, "SendPing", e);
            await Context.Interaction.SendFollowupMessageAsync(new InteractionMessageProperties
            {
                Content = "Unexpected error occured when running ping command"
            });
        }
    }


}