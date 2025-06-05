using Shouko.Models.Enums;

namespace Shouko.Helpers.Extensions;

public interface IApiResponseHelper
{
    List<string> FormatGeminiResponseToDiscordMessage(string responseContent);
    List<string>? GetApiReponseDiscordText<T>(T apiResponseDto, ApiType apiType);
    MemoryStream? ConvertBase64ToStream<T>(T apiResponseDto, ApiType apiType);

}