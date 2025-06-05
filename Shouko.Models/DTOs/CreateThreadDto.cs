using Shouko.Models.Enums;

namespace Shouko.Models.DTOs;

public class CreateThreadDto
{
    public string InputText { get; set; }
    public ApiType ApiType { get; set; }
    public ApiPromptType ApiPromptType { get; set; }
    // Discord MetaData
    public ulong InteractionId { get; set; }
    public ulong ChannelId { get; set; }
    public ulong GuildId { get; set; }
    public ulong UserId { get; set; }
}