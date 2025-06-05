using NetCord.Services.ApplicationCommands;

public enum ApiPromptType
{
    [SlashCommandChoice("Text")]
    Text = 0,
    [SlashCommandChoice("Image")]
    Image = 1,
}