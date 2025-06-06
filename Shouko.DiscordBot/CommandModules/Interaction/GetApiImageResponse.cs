using NetCord.Rest;
using NetCord.Services.ApplicationCommands;
using Shouko.BusinessService.Interfaces;
using Shouko.Helpers.Extensions;
using Shouko.Models.DTOs;
using Shouko.Models.Enums;

namespace Shouko.CommandModules.Interaction;

public class GetApiImageResponse : ApplicationCommandModule<SlashCommandContext>
{
    private readonly ILogger<GetApiImageResponse> _logger;
    private readonly ApplicationCommandService<SlashCommandContext> _commandService;
    private readonly IApiServiceBusinessService _apiServiceBusinessService;

    public GetApiImageResponse(ILogger<GetApiImageResponse> logger,
        ApplicationCommandService<SlashCommandContext> commandService, IApiServiceBusinessService apiServiceBusinessService)
    {
        _logger = logger;
        _commandService = commandService;
        _apiServiceBusinessService = apiServiceBusinessService;
    }

    [SlashCommand("getapiimageresponse", "Queries the specified API with the provided text input and returns image response")]
    public async Task SendGetApiImageResponse(
        ApiType apiType,
        [SlashCommandParameter(Name = "input", Description = "The input to send to the API")]
        string textInput
    )
    {
        try
        {
            _logger.LogActionTraceStart(Context, "SendGetApiImageResponse");
            await Context.Interaction.SendResponseAsync((InteractionCallback.DeferredMessage()));
            var response = await _apiServiceBusinessService.CreateApiImageThread<object>
                           (new CreateThreadDto
                           {
                               ApiType = apiType,
                               InputText = textInput,
                               ApiPromptType = ApiPromptType.Image,
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
                var attachment = new AttachmentProperties("gemini-image.png", response.Stream);
                await Context.Interaction.SendFollowupMessageAsync(
                    (new InteractionMessageProperties
                    {
                        Attachments = new List<AttachmentProperties> { attachment }
                    }));
            }
            _logger.LogActionTraceFinish(Context, "SendGetApiImageResponse");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while processing the image thread creation.");
            await Context.Interaction.SendFollowupMessageAsync(new InteractionMessageProperties
            {
                Content = $"Unexpected error when executing create api image chat"
            });
        }
    }


}