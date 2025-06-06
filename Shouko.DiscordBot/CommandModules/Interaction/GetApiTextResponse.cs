using NetCord.Rest;
using NetCord.Services.ApplicationCommands;
using Shouko.Api.Interfaces;
using Shouko.BusinessService.Interfaces;
using Shouko.Helpers.Extensions;
using Shouko.Models.DTOs;
using Shouko.Models.Enums;

namespace Shouko.CommandModules.Interaction;

public class GetApiTextResponse : ApplicationCommandModule<SlashCommandContext>
{
    private readonly ILogger<GetApiTextResponse> _logger;
    private readonly ApplicationCommandService<SlashCommandContext> _commandService;
    private readonly IApiServiceBusinessService _apiServiceBusinessService;


    public GetApiTextResponse(ILogger<GetApiTextResponse> logger,
        ApplicationCommandService<SlashCommandContext> commandService, IApiServiceBusinessService apiServiceBusinessService)
    {
        _logger = logger;
        _commandService = commandService;
        _apiServiceBusinessService = apiServiceBusinessService;
    }

    [SlashCommand("getapitextresponse", "Queries the specified API with the provided text input and returns text response")]
    public async Task SendGetApiTextResponse(
        ApiType apiType ,
        [SlashCommandParameter(Name = "input", Description = "The input to send to the API")] 
        string textInput
    )
    {
        try
        {
            _logger.LogActionTraceStart(Context, "SendGetApiTextResponse");
            await Context.Interaction.SendResponseAsync(InteractionCallback.DeferredMessage());
            var response = await _apiServiceBusinessService.CreateApiTextThread<object>(new CreateThreadDto
            {
                ApiType = apiType,
                InputText = textInput,
                ApiPromptType = ApiPromptType.Text,
                InteractionId = Context.Interaction.Id,
                ChannelId = Context.Channel.Id,
                GuildId = Context.Guild.Id,
                UserId = Context.User.Id
            });

            if (!response.Success)
            {
                _logger.LogError("API request failed with error: {ErrorMessage}", response.ErrorMessage);
                await Context.Interaction.SendFollowupMessageAsync(new InteractionMessageProperties
                {
                    Content = $"Unexpected error while calling {apiType} Api"
                });
            }
            else
            {
                foreach (var message in response.Responses)
                {
                    await Context.Interaction.SendFollowupMessageAsync(new InteractionMessageProperties
                    {
                        Content = message
                    });
                }
            }
            
            
            _logger.LogActionTraceFinish(Context, "SendGetApiTextResponse");
        }
        catch (Exception e)
        {
            _logger.LogExceptionError(Context, "SendGetApiTextResponse", e);
            await Context.Interaction.SendFollowupMessageAsync(new InteractionMessageProperties
            {
                Content = "Unexpected error when executing create api text chat"
            });
        }
    }
}