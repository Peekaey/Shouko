using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Shouko.Api.Interfaces;
using Shouko.Helpers.Extensions;
using Shouko.Models.ApiResponse;
using Shouko.Models.DTOs;
using Shouko.Models.Enums;
using Shouko.Models.Results;

namespace Shouko.Api.Services;

public class ApiService : IApiService
{
    private readonly ILogger<ApiService> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IApiManager _apiManager;
    private readonly IApiResponseHelper _apiResponseHelper;
    
    public ApiService(ILogger<ApiService> logger, IHttpClientFactory httpClientFactory, IApiManager apiManager, IApiResponseHelper apiResponseHelper)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _apiManager = apiManager;
        _apiResponseHelper = apiResponseHelper;
    }

    public async Task<ApiResult<T>> SendRequest<T>(CreateThreadDto requestDto) where T : class
    {
        try {
            using (var httpClient = PrepareHttpClient(requestDto.ApiType, requestDto.ApiPromptType))
            {
                var requestBodyJson = _apiManager.PrepareRequestJson(requestDto.ApiPromptType, requestDto.ApiType, requestDto.InputText);
                var content = new StringContent(requestBodyJson, System.Text.Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync("", content);
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await PrepareApiResponse<T>(response, requestDto.ApiType, requestDto.ApiPromptType);
                    return ApiResult<T>.AsSuccess(apiResponse);
                }
                else
                {
                    _logger.LogError("API request failed with status code {StatusCode} for {ApiType}", 
                        response.StatusCode, requestDto.ApiType);
                    throw new HttpRequestException($"API request failed with status code {response.StatusCode}");
                }
            }
        }
        catch (Exception e)
        {
            _logger.LogError("An error occurred while sending API request: {ErrorMessage}", e.Message);
            return ApiResult<T>.AsFailure($"An error occurred while sending API request: {e.Message}");
        }
        
    }

    private HttpClient PrepareHttpClient(ApiType apiType, ApiPromptType apiPromptType)
    {
        var apiKey = _apiManager.GetApiKey(apiType);
        if (string.IsNullOrEmpty(apiKey))
        {
            _logger.LogError("API key is not configured for {ApiType}", apiType);
            throw new InvalidOperationException($"API key is not configured for {apiType}");
        }
        var apiModel = _apiManager.GetApiModel(apiType, apiPromptType);
        var url = _apiManager.GetApiUrl(apiType);
        url = _apiManager.PrepareGeminiUrl(url, apiModel, apiKey);
        var client = _httpClientFactory.CreateClient(apiType.ToString());
        client.BaseAddress = new Uri(url);
        return client;
    }

    private async Task<T> PrepareApiResponse<T>(HttpResponseMessage response, ApiType apiType, ApiPromptType apiPromptType) where T : class
    {

        switch (apiPromptType)
        {
            case ApiPromptType.Text:
                return await PrepareApiTextResponse<T>(response, apiType, apiPromptType);
                
            case ApiPromptType.Image:
                return await PrepareApiImageResponse<T>(response, apiType, apiPromptType);
            default:
                _logger.LogError("Unsupported ApriPrompt type: {ApiType}", apiType);
                throw new NotSupportedException($"Unsupported ApiPrompt type: {apiType}");
        }
    }

    private async Task<T> PrepareApiTextResponse<T>(HttpResponseMessage response, ApiType apiType, ApiPromptType apiPromptType) where T : class
    {
        var contentString = await response.Content.ReadAsStringAsync();

        switch (apiType)
        {
            case ApiType.Gemini:

                // TODO Refactor and tidy up
                var responseData = JsonSerializer.Deserialize<List<GeminiApiTextResponse>>(contentString);
                var combinedResponse = string.Join("", responseData.Select(r => r.Candidates.FirstOrDefault().Content.Parts.FirstOrDefault().Text));
                var apiResponse = new GeminiApiResponseDto
                {
                    ResponseContent = combinedResponse,
                    ResponseId = responseData.FirstOrDefault()?.ResponseId,
                    IsSuccess = true,
                    Model = _apiManager.GetApiModel(apiType, apiPromptType),
                    DiscordResponseMessage = _apiResponseHelper.FormatGeminiResponseToDiscordMessage(combinedResponse),
                };
                return apiResponse as T;
                
            default:
                _logger.LogError("Unsupported API type: {ApiType}", apiType);
                throw new NotSupportedException($"Unsupported API type: {apiType}");
                break;
        }
        
    }

    private async Task<T> PrepareApiImageResponse<T>(HttpResponseMessage response, ApiType apiType, ApiPromptType apiPromptType) where T : class
    {
        var contentString = await response.Content.ReadAsStringAsync();

        switch (apiType)
        {
            case ApiType.Gemini:
                // TODO Refactor and tidy up
                var responseData = JsonSerializer.Deserialize<List<GeminiApiImageResponse>>(contentString);
                var imageContent = string.Empty;
                var imageContentType = string.Empty;
                // Find the entry with base64 image data
                foreach (var r in responseData)
                {
                    var inlineData = r.Candidates.FirstOrDefault()?.Content.Parts?.FirstOrDefault()?.InlineData;
                    if (inlineData != null)
                    {
                        imageContent = inlineData.Data;
                        imageContentType = inlineData.MimeType;
                    }
                }
                var apiResponse = new GeminiApiResponseDto
                {
                    ResponseContent = imageContent,
                    ResponseId = responseData.FirstOrDefault()?.ResponseId,
                    IsSuccess = true,
                    Model = _apiManager.GetApiModel(apiType, apiPromptType),
                    MimeType = imageContentType,
                };
                return apiResponse as T;
            
            default:
                _logger.LogError("Unsupported API type: {ApiType}", apiType);
                throw new NotSupportedException($"Unsupported API type: {apiType}");
                break;
        }
        
    }
}