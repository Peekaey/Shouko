namespace Shouko.Models.DatabaseModels;

public class DiscordInteraction
{
    public int Id { get; set; }
    public long InteractionId { get; set; }
    public long ChannelId { get; set; }
    public long GuildId { get; set; }
    public long UserId { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}