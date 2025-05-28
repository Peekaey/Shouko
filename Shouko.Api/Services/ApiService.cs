using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Shouko.Api.Interfaces;
using Shouko.Models.DTOs;
using Shouko.Models.Enums;
using Shouko.Models.Results;

namespace Shouko.Api.Services;

public class ApiService : IApiService
{
    private readonly ILogger<ApiService> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IApiManager _apiManager;
    
    public ApiService(ILogger<ApiService> logger, IHttpClientFactory httpClientFactory, IApiManager apiManager)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _apiManager = apiManager;
    }

    public async Task<ApiResult<ApiResponseDto>> SendRequest(CreateThreadDto requestDto)
    {
        using (var httpClient = PrepareHttpClient(requestDto.ApiType))
        {
            
            var requestBodyJson = _apiManager.PrepareGeminiRequestJson(requestDto.InputText);
            var content = new StringContent(requestBodyJson, System.Text.Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("", content);
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var contentString = await response.Content.ReadAsStringAsync();
                    var responseData = JsonSerializer.Deserialize<List<GeminiApiTextResponse>>(contentString);

                    var combinedResponse = string.Join("", responseData.Select(r => r.Candidates.FirstOrDefault().Content.Parts.FirstOrDefault().Text));
                    var apiResponse = new ApiResponseDto
                    {
                        ResponseContent = combinedResponse,
                        ResponseId = responseData.FirstOrDefault()?.ResponseId
                    };
                    
                    return ApiResult<ApiResponseDto>.AsSuccess(apiResponse);
                    
                }
                catch (Exception e)
                {
                    return ApiResult<ApiResponseDto>.AsFailure(e.Message);
                }
            }
            else
            {
                _logger.LogError("API request failed with status code {StatusCode} for {ApiType}", 
                    response.StatusCode, requestDto.ApiType);
                throw new HttpRequestException($"API request failed with status code {response.StatusCode}");
            }
        }
        
    }

    private HttpClient PrepareHttpClient(ApiType apiType)
    {
        var apiKey = _apiManager.GetApiKey(apiType);
        if (string.IsNullOrEmpty(apiKey))
        {
            _logger.LogError("API key is not configured for {ApiType}", apiType);
            throw new InvalidOperationException($"API key is not configured for {apiType}");
        }
        var apiModel = _apiManager.GetApiModel(apiType);
        var url = _apiManager.GetApiUrl(apiType);
        url = _apiManager.PrepareGeminiUrl(url, apiModel, apiKey);
        var client = _httpClientFactory.CreateClient(apiType.ToString());
        client.BaseAddress = new Uri(url);
        return client;

    }
    
}