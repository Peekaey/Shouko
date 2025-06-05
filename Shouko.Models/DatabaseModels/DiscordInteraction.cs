namespace Shouko.Models.DatabaseModels;

public class DiscordInteraction
{
    public int Id { get; set; }
    public ulong InteractionId { get; set; }
    public ulong ChannelId { get; set; }
    public ulong GuildId { get; set; }
    public ulong UserId { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}