using NetCord.Rest;
using NetCord.Services.ApplicationCommands;
using Shouko.Api.Interfaces;
using Shouko.Helpers.Extensions;
using Shouko.Models.DTOs;
using Shouko.Models.Enums;

namespace Shouko.CommandModules.Interaction;

public class CreateApiThread : ApplicationCommandModule<SlashCommandContext>
{
    private readonly ILogger<CreateApiThread> _logger;
    private readonly ApplicationCommandService<SlashCommandContext> _commandService;
    private readonly IApiService _apiService;

    public CreateApiThread(ILogger<CreateApiThread> logger,
        ApplicationCommandService<SlashCommandContext> commandService, IApiService apiService)
    {
        _logger = logger;
        _commandService = commandService;
        _apiService = apiService;
    }

    [SlashCommand("createchat", "Creates a new conversation thread")]
    public async Task SendCreateApiThread(
        ApiType apiType ,
        [SlashCommandParameter(Name = "input", Description = "The input to send to the API")] 
        string textInput
    )
    {
        try
        {
            await Context.Interaction.SendResponseAsync(InteractionCallback.DeferredMessage());
            _logger.LogActionTraceStart(Context, "SendCreateApiThread");
            var response = await _apiService.SendRequest(new CreateThreadDto
            {
                ApiType = apiType,
                InputText = textInput
            });

            if (response.IsSuccess)
            {
                await Context.Interaction.SendFollowupMessageAsync(new InteractionMessageProperties
                {
                    Content = response.Response.ResponseContent
                });
            }
            else
            {
                _logger.LogError("API request failed with error: {ErrorMessage}", response.ErrorMessage);
                await Context.Interaction.SendFollowupMessageAsync(new InteractionMessageProperties
                {
                    Content = $"Unexpected error while calling {apiType} Api"
                });
            }
            
            
            _logger.LogActionTraceFinish(Context, "SendCreateApiThread");
        }
        catch (Exception e)
        {
            _logger.LogExceptionError(Context, "SendCreateApiThread", e);
            await Context.Interaction.SendFollowupMessageAsync(new InteractionMessageProperties
            {
                Content = "Unexpected error when executing api chat"
            });
        }
    }
}