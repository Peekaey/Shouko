using Shouko.Models.Enums;
using Shouko.Models.Results;

namespace Shouko.BusinessService.Interfaces;

public interface IApiResponseBusinessService
{
    SaveResult SaveApiResponse(ApiType apiType, string model, string inputText, string responseContent,
        string responseId, ApiPromptType apiPromptType, int candidatesTokenCount, int promptTokenCount, 
        bool isSuccess, int? discordInteractionId);

    SaveResult ConvertApiResponseAndSave<T>(T apiResponseDto, ApiType apiType,
        ApiPromptType apiPromptType, string inputText, int? discordInteractionId);
}