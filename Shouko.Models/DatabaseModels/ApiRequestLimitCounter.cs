using Shouko.Models.Enums;

namespace Shouko.Models.DatabaseModels;

public class ApiRequestLimitCounter
{
    public int Id { get; set; }
    public DateTime StartDateTime { get; set; }
    public LimitCounterType LimitCounterType { get; set; }
    public ApiPromptType ApiPromptType { get; set; }
    public ApiType ApiType { get; set; }
    public bool IsArchived { get; set; }
}