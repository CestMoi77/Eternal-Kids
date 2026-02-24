namespace EternalKids.Domain.Entities;

public class AiRequestLog
{
    public Guid Id { get; set; }
    public Guid? ChatSessionId { get; set; }
    public string Model { get; set; } = string.Empty;
    public string PromptHash { get; set; } = string.Empty;
    public string? ToolCallsJson { get; set; }
    public string? ResponseHash { get; set; }
    public int? TokensIn { get; set; }
    public int? TokensOut { get; set; }
    public decimal? CostEstimate { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public bool Succeeded { get; set; }
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
}
