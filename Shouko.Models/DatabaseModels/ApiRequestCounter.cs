using Shouko.Models.Enums;

namespace Shouko.Models.DatabaseModels;

public class ApiRequestCounter
{
    public int Id { get; set; }
    public ApiPromptType ApiPromptType { get; set; }
    public ApiType ApiType { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public bool IsArchived { get; set; }
}