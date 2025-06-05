
using Shouko.Models.DatabaseModels;
using Shouko.Models.Enums;
using Shouko.Models.Results;

public interface IApiResponseBusinessService
{
    ServiceResult SaveApiResponse(ApiType apiType, string model, string inputText, string responseContent,
        string responseId, ApiPromptType apiPromptType, int candidatesTokenCount, int promptTokenCount, 
        bool isSuccess, int? discordInteractionId);

    ServiceResult ConvertApiResponseAndSave<T>(T apiResponseDto, ApiType apiType,
        ApiPromptType apiPromptType, string inputText, int? discordInteractionId);
}