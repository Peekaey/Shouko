using Shouko.Models.Enums;

namespace Shouko.Api.Interfaces;

public interface IApiManager
{
    string GetApiKey(ApiType apiType);
    string GetApiModel(ApiType apiType);
    string GetApiUrl(ApiType apiType);
    string PrepareGeminiUrl(string url, string modelId, string apiKey);
    string PrepareGeminiRequestJson(string textInput);
}