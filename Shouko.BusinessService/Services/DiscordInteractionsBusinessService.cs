
using Microsoft.Extensions.Logging;
using Shouko.Api.Interfaces;
using Shouko.BusinessService.Interfaces;
using Shouko.Models.DatabaseModels;
using Shouko.Models.DTOs;
using Shouko.Models.Results;

public class DiscordInteractionsBusinessService : IDiscordInteractionsBusinessService
{
    private readonly ILogger<DiscordInteractionsBusinessService> _logger;
    private readonly IDiscordInteractionsService _discordInteractionsService;
    private readonly IApiService _apiService;
    

    public DiscordInteractionsBusinessService(IDiscordInteractionsService discordInteractionsService,
        ILogger<DiscordInteractionsBusinessService> logger, IApiService apiService)
    {
        _logger = logger;
        _discordInteractionsService = discordInteractionsService;
        _apiService = apiService;
    }
    
    
    
    public async Task<ApiResult<ApiResponseDto>> CreateApiThread(CreateThreadDto  createThreadDto)
    {
        var existingInteractionId = _discordInteractionsService.GetStoredInteraction(createThreadDto.InteractionId);

        if (existingInteractionId == null)
        {
            var apiResponse = await _apiService.SendRequest(createThreadDto);
            if (apiResponse.IsSuccess)
            {
                var saveResult = _discordInteractionsService.Save(new DiscordInteraction
                {
                    InteractionId = createThreadDto.InteractionId,
                    ChannelId = createThreadDto.ChannelId,
                    GuildId = createThreadDto.GuildId,
                    UserId = createThreadDto.UserId,
                });
                // TODO Add Transaction Scope Here
                // Convert to guard 
                if (saveResult.Success)
                { 
                    return ApiResult<ApiResponseDto>.AsSuccess(new ApiResponseDto
                    {
                    });
                }
                else
                {
                    _logger.LogError("Failed to save interaction: {Error}", saveResult.ErrorMessage);
                    return ApiResult<ApiResponseDto>.AsFailure(saveResult.ErrorMessage);
                }
            }
            else
            {
                _logger.LogError("API request failed: {Error}", apiResponse.ErrorMessage);
                return ApiResult<ApiResponseDto>.AsFailure(apiResponse.ErrorMessage);
            }

        }
        else
        {
            _logger.LogWarning("Interaction with ID {InteractionId} already exists.", existingInteractionId);
            return ApiResult<ApiResponseDto>.AsFailure("Interaction already exists.");
        }
        
    }
    
    public ServiceResult Save(DiscordInteraction interaction)
    {
        return _discordInteractionsService.Save(interaction);
    }
    
}