using System.Net.Http.Json;
using System.Text.Json;
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
         default:
            return string.Empty;
            break;
      }
   }
   
   public string GetApiModel(ApiType apiType, ApiPromptType apiPromptType)
   {
      switch (apiType)
      {
         case ApiType.Gemini:
            if (apiPromptType == ApiPromptType.Text)
            {
               return _applicationConfigurationSettings.GeminiTextModel;
            }
            else
            {
               return _applicationConfigurationSettings.GeminiImageModel;
            }
         default:
            return string.Empty;
            break;
      }
   }

   public string PrepareRequestJson(ApiPromptType apiPromptType, ApiType apiType, string textInput)
   {
      switch (apiType)
      {
         case ApiType.Gemini:
            if (apiPromptType == ApiPromptType.Text)
            {
               return PrepareGeminiTextRequestJson(textInput);
            }
            else
            {
               return PrepareGeminiImageRequestJson(textInput);
            }
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

   public string PrepareGeminiTextRequestJson(string textInput)
   {
      var inputPrompt = _applicationConfigurationSettings.DefaultPrompt + "\n" + textInput;
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
                     text = inputPrompt
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

   public string PrepareGeminiImageRequestJson(string textInput)
   {
      var requestJson = new
      {
         contents = new[]
         {
            new
            {
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
            responseModalities = new[]
            {
               "TEXT",
               "IMAGE"
            }
         }
      };
      return JsonSerializer.Serialize(requestJson);
   }
}