using NetCord.Services.ApplicationCommands;

namespace Shouko.Models.Enums;

public enum ApiType
{
    [SlashCommandChoice("Gemini")]
    Gemini = 0,
    // [SlashCommandChoice("DeepSeek")]
    // DeepSeek = 1,
    // [SlashCommandChoice("OpenAI")]
    // OpenAi = 2,
}