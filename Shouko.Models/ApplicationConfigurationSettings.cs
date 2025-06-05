namespace Shouko.Models;
using NetCord;

public class ApplicationConfigurationSettings
{
    public string DiscordToken { get; set; }
    public IEntityToken EntityToken => new BotToken(DiscordToken);
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string DatabaseUserName { get; set; }
    public string DatabasePassword { get; set; }
    public int DatabasePort { get; set; }
    public bool Debug { get; set; } = true;
    public string DeepSeekApiKey { get; set; }

    public string GeminiApiKey { get; set; }
    public string DeepSeekApiUrl { get; set; }
    public string GeminiApiUrl { get; set; }
    public string DeepSeekModel { get; set; }
    public string GeminiTextModel { get; set; }
    public string GeminiImageModel { get; set; }

    public readonly string DefaultPrompt = "When answering the prompt, can you start each section of the response " +
                                           "with \"Part\" and no number as well as not acknowledging " +
                                           "the instructions in the response";
}