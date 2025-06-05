using Shouko.Models.Enums;

namespace Shouko.Api.Interfaces;

public interface IApiManager
{
    string GetApiKey(ApiType apiType);
    string GetApiModel(ApiType apiType, ApiPromptType apiPromptType);
    string GetApiUrl(ApiType apiType);
    string PrepareGeminiUrl(string url, string modelId, string apiKey);
    string PrepareGeminiTextRequestJson(string textInput);
    string PrepareGeminiImageRequestJson(string textInput);
    string PrepareRequestJson(ApiPromptType apiPromptType, ApiType apiType, string textInput);
}