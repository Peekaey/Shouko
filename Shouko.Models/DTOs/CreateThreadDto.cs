using Shouko.Models.Enums;

namespace Shouko.Models.DTOs;

public class CreateThreadDto
{
    public string InputText { get; set; }
    public ApiType ApiType { get; set; }
    // Discord MetaData
    public long InteractionId { get; set; }
    public long ChannelId { get; set; }
    public long GuildId { get; set; }
    public long UserId { get; set; }
}