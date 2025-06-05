using Shouko.Helpers.Extensions;
using Shouko.Models.ApiResponse;
using Shouko.Models.Enums;

namespace Shouko.Helpers;

public class ApiResponseHelper : IApiResponseHelper
{
    public List<string> FormatGeminiResponseToDiscordMessage(string responseContent)
    {
        // Split the responseContent into groups of 2000 characters max due to discords character limit
        List<string> discordMessages = new List<string>();
        if (string.IsNullOrEmpty(responseContent))
        {
            return discordMessages;
        }
        else
        {
            // Split by "Part" keyword
            var parts = responseContent.Split(new[] { "Part" }, StringSplitOptions.RemoveEmptyEntries);
            // Remove "\n\n" from the start of each part
            for (int i = 0; i < parts.Length; i++)
            {
                parts[i] = parts[i].TrimStart('\n', '\r', ' ');
            }
            
            for (int i = 0; i < parts.Length; i++)
            {
                var countText = "[Part: " + (i + 1) + " of " + parts.Length + "]";
                // Add the count text to the beginning of each part
                discordMessages.Add(countText + "\n" + parts[i]);
                
            }

            return discordMessages;
        }
    }
    
    public List<string>? GetApiReponseDiscordText<T>(T apiResponseDto, ApiType apiType)
    {
        switch (apiType)
        {
            case ApiType.Gemini:
                if (apiResponseDto is GeminiApiResponseDto geminiResponse)
                {
                    return geminiResponse.DiscordResponseMessage;
                }
                return null;
            default:
                return null;
        }
    }

    public MemoryStream? ConvertBase64ToStream<T>(T apiResponseDto, ApiType apiType)
    {
        try {
            string imageContent = string.Empty;

            switch (apiType)
            {
                case ApiType.Gemini:
                    if (apiResponseDto is GeminiApiResponseDto geminiResponse)
                    {
                        imageContent = geminiResponse.ResponseContent;
                    }

                    break;
                // TODO Not Implemented Yet
            }
        
            if (string.IsNullOrEmpty(imageContent))
            {
                return null;
            }
            // Remove the base64 header if it exists
            // Doesn't in the case of Geminis response but just in case
            if (imageContent.Contains("base64,"))
            {
                imageContent = imageContent.Split("base64,")[1];
            }
            byte[] imageBytes = Convert.FromBase64String(imageContent);
            MemoryStream imageStream = new MemoryStream(imageBytes);
            imageStream.Position = 0;
            return imageStream;
        
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error converting base64 to stream: {ex.Message}");
            return null;
        }
        
    }
    
}