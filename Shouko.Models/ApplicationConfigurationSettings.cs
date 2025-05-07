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
}