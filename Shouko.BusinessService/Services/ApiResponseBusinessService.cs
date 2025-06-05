
// DB Entity Business Class

using Microsoft.Extensions.Logging;
using Shouko.Api.Interfaces;
using Shouko.Models.DatabaseModels;
using Shouko.Models.Enums;
using Shouko.Models.Results;

public class ApiResponseBusinessService : IApiResponseBusinessService
{
    private readonly ILogger<ApiResponseBusinessService> _logger;
    private readonly IApiResponsesService _apiResponsesService;

    public ApiResponseBusinessService(ILogger<ApiResponseBusinessService> logger, IApiResponsesService apiResponsesService)
    {
        _logger = logger;
        _apiResponsesService = apiResponsesService;
    }
    
    public SaveResult SaveApiResponse(ApiType apiType, string model, string inputText, string responseContent, string responseId,
        ApiPromptType apiPromptType, int candidatesTokenCount, int promptTokenCount, bool isSuccess, int? discordInteractionId)
    {
        var apiResponse = new ApiResponse
        {
            DiscordInteractionId = discordInteractionId,
            IsSuccess = isSuccess,
            CreatedAtUtc = DateTime.UtcNow,
            ApiType = apiType,
            Model = model,
            InputText = inputText,
            ResponseId = responseId,
            ApiPromptType = apiPromptType,
            PromptTokenCount = promptTokenCount,
            CandidatesTokenCount = candidatesTokenCount,
        };
        if (apiPromptType == ApiPromptType.Text)
        {
            apiResponse.ResponseText = responseContent;
        }
        else
        {
            apiResponse.ResponseImageContent = responseContent;
        }
        var saveResult = _apiResponsesService.SaveAndReturnId(apiResponse);
        return saveResult;
    }

    public SaveResult ConvertApiResponseAndSave<T>(T apiResponseDto, ApiType apiType, ApiPromptType apiPromptType, string inputText, int? discordInteractionId)
    {
        switch (apiType)
        {
            case ApiType.Gemini:
                if (apiResponseDto is GeminiApiResponseDto geminiResponse)
                {
                    var apiResponse = new ApiResponse
                    {
                        DiscordInteractionId = discordInteractionId,
                        IsSuccess = geminiResponse.IsSuccess,
                        CreatedAtUtc = DateTime.UtcNow,
                        ApiType = apiType,
                        Model = geminiResponse.Model,
                        InputText = inputText,
                        ResponseText = geminiResponse.ResponseContent,
                        ResponseId = geminiResponse.ResponseId,
                        ApiPromptType = apiPromptType,
                        PromptTokenCount = geminiResponse.PromptTokenCount,
                        CandidatesTokenCount = geminiResponse.CandidatesTokenCount,
                    };
                    if (apiPromptType == ApiPromptType.Text)
                    {
                        apiResponse.ResponseText = geminiResponse.ResponseContent;
                    }
                    else
                    {
                        apiResponse.ResponseImageContent = geminiResponse.ResponseContent;
                    }
                    return _apiResponsesService.SaveAndReturnId(apiResponse);
                }
                return SaveResult.AsFailure($"Expected type GeminiApiResponseDto but received {typeof(T)}.");
            default:
                return SaveResult.AsFailure($"Unsupported API type: {apiType}. Cannot convert and save response.");
        }
    }
    
} 