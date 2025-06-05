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
    private readonly IApiService _apiService;
    private readonly IApiResponseHelper _apiResponseHelper;
    private readonly IApiResponseBusinessService _apiResponseBusinessService;
    

    public DiscordInteractionsBusinessService(IDiscordInteractionsService discordInteractionsService,
        ILogger<DiscordInteractionsBusinessService> logger, IApiService apiService, IApiResponseHelper apiResponseHelper,
        IApiResponseBusinessService apiResponseBusinessService)
    {
        _logger = logger;
        _discordInteractionsService = discordInteractionsService;
        _apiService = apiService;
        _apiResponseHelper = apiResponseHelper;
        _apiResponseBusinessService = apiResponseBusinessService;
    }
    
    
    
    public async Task<InteractionResult> CreateApiTextThread<T>(CreateThreadDto  createThreadDto) where T : class
    {

            var requestResponse = await _apiService.SendRequest<T>(createThreadDto);
            if (!requestResponse.IsSuccess)
            {
                _logger.LogError($"API request failed with error: {requestResponse.ErrorMessage}", requestResponse.ErrorMessage);
                return InteractionResult.AsTextFailure(requestResponse.ErrorMessage,createThreadDto.ApiPromptType);
            }
            
            var interactionSaveResult = SaveDiscordInteraction(createThreadDto.InteractionId, createThreadDto.ChannelId, 
                createThreadDto.GuildId, createThreadDto.UserId);
            
            if (!interactionSaveResult.Success) {
                _logger.LogError($"Failed to save Discord interaction with error: {interactionSaveResult.ErrorMessage}", interactionSaveResult.ErrorMessage); 
            }
            var apiResponseSaveResult = _apiResponseBusinessService.ConvertApiResponseAndSave(
                requestResponse.Response, createThreadDto.ApiType, createThreadDto.ApiPromptType,
                createThreadDto.InputText, interactionSaveResult.Id);
            
            if (!apiResponseSaveResult.Success) {
                _logger.LogError($"Failed to save API response with error: {apiResponseSaveResult.ErrorMessage}", apiResponseSaveResult.ErrorMessage);
            }
            
            var responseMessages = _apiResponseHelper.GetApiReponseDiscordText(requestResponse.Response, createThreadDto.ApiType);
            if (responseMessages != null && responseMessages.Any()) {
                return InteractionResult.AsTextSuccess(responseMessages, createThreadDto.ApiPromptType);
            }
            
            _logger.LogError("API response conversion to Discord text failed, no messages returned.");
            return InteractionResult.AsTextFailure("Failed to convert API response to Discord text", createThreadDto.ApiPromptType);
    }

    public async Task<InteractionResult> CreateApiImageThread<T>(CreateThreadDto createThreadDto) where T : class
    {
            var requestResponse = await _apiService.SendRequest<T>(createThreadDto);
            
            if (!requestResponse.IsSuccess)
            {
                _logger.LogError($"API request failed with error: {requestResponse.ErrorMessage}", requestResponse.ErrorMessage);
                return InteractionResult.AsFileFailure(requestResponse.ErrorMessage, createThreadDto.ApiPromptType);
            }
            
            var interactionSaveResult = SaveDiscordInteraction(createThreadDto.InteractionId, createThreadDto.ChannelId, 
                createThreadDto.GuildId, createThreadDto.UserId);
            
            if (!interactionSaveResult.Success) {
                _logger.LogError($"Failed to save Discord interaction with error: {interactionSaveResult.ErrorMessage}", interactionSaveResult.ErrorMessage); 
            }
            
            var apiResponseSaveResult = _apiResponseBusinessService.ConvertApiResponseAndSave(
                requestResponse.Response, createThreadDto.ApiType, createThreadDto.ApiPromptType, 
                createThreadDto.InputText, interactionSaveResult.Id);
            
            if (!apiResponseSaveResult.Success) {
                _logger.LogError($"Failed to save API response with error: {apiResponseSaveResult.ErrorMessage}", apiResponseSaveResult.ErrorMessage);
            }
            
            var imageData = _apiResponseHelper.ConvertBase64ToStream(requestResponse.Response, createThreadDto.ApiType);
            
            if (imageData != null)
            {
                return InteractionResult.AsFileSuccess(imageData, createThreadDto.ApiPromptType);
            }
            _logger.LogError("Image data conversion failed, no image returned.");
            return InteractionResult.AsFileFailure("Failed to convert API response to image", createThreadDto.ApiPromptType);
    }    
    
    public DiscordInteractionSaveResult SaveDiscordInteraction(ulong interactionId, ulong channelId, ulong guildId, ulong userId)
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