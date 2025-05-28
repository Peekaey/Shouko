using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Shouko.Api.Interfaces;
using Shouko.Helpers;
using Shouko.Models;
using Shouko.Models.DTOs;
using Shouko.Models.Enums;

namespace Shouko.Api;

public class ApiManager : IApiManager
{
   private readonly ILogger<ApiManager> _logger;
   private readonly ApplicationConfigurationSettings _applicationConfigurationSettings;
   private readonly IHttpClientFactory _httpClientFactory;

   public ApiManager(ILogger<ApiManager> logger, ApplicationConfigurationSettings applicationConfigurationSettings, 
      IHttpClientFactory httpClientFactory)
   {
      _logger = logger;
      _applicationConfigurationSettings = applicationConfigurationSettings;
      _httpClientFactory = httpClientFactory;
   }

   public string GetApiKey(ApiType apiType)
   {
      switch (apiType)
      {
         case ApiType.Gemini:
            return _applicationConfigurationSettings.GeminiApiKey;
            break;
         case ApiType.DeepSeek:
            return _applicationConfigurationSettings.DeepSeekApiKey;
            break;
         default:
            return string.Empty;
            break;
      }
   }

   public string GetApiUrl(ApiType apiType)
   {
      switch (apiType)
      {
         case ApiType.Gemini:
            return _applicationConfigurationSettings.GeminiApiUrl;
            break;
         case ApiType.DeepSeek:
            return _applicationConfigurationSettings.DeepSeekApiUrl;
            return string.Empty;
            break;
         default:
            return string.Empty;
            break;
      }
   }

   public string GetApiModel(ApiType apiType)
   {
      switch (apiType)
      {
         case ApiType.Gemini:
            return _applicationConfigurationSettings.GeminiModel;
            break;
         case ApiType.DeepSeek:
            return _applicationConfigurationSettings.DeepSeekModel;
            break;
         default:
            return string.Empty;
            break;
      }
   }

   public string PrepareGeminiUrl(string url, string modelId, string apiKey)
   {
      var generateContentApi = "streamGenerateContent";
      var combinedUrl = url + modelId + ":" + generateContentApi
         + "?key=" + apiKey;
      return combinedUrl;
   }

   public string PrepareGeminiRequestJson(string textInput)
   {
      var requestJson = new
      {
         contents = new[]
         {
            new
            {
               role = "user",
               parts = new[]
               {
                  new
                  {
                     text = textInput
                  }
               }
            }
         },
         generationConfig = new
         {
            responseMimeType = "text/plain"
         }
      };

      return System.Text.Json.JsonSerializer.Serialize(requestJson);
   }
   
   
   
      
}