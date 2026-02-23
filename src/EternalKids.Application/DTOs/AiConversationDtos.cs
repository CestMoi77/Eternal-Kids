namespace EternalKids.Application.DTOs;

public sealed record AiUserMessage(
    Guid SessionId,
    string Message,
    string? EventType = null,
    string? PackageType = null,
    int? GuestCount = null,
    int Quantity = 1);

public sealed record AiConversationResult(
    Guid SessionId,
    string AssistantMessage,
    IReadOnlyCollection<AiUiAction> UiActions,
    int? TokensIn,
    int? TokensOut);

public sealed record AiUiAction(
    string Type,
    string Label,
    string? PayloadJson = null);

public sealed record AiToolCall(
    string Name,
    string ArgumentsJson);

public sealed record AiModelRequest(
    string SystemPrompt,
    string UserMessage,
    IReadOnlyCollection<AiToolDefinition> Tools,
    IReadOnlyCollection<AiToolExecutionResult>? ToolResults = null);

public sealed record AiToolDefinition(
    string Name,
    string Description,
    string JsonSchema);

public sealed record AiToolExecutionResult(
    string Name,
    string ResultJson);

public sealed record AiModelResponse(
    bool Succeeded,
    string Model,
    string AssistantMessage,
    IReadOnlyCollection<AiToolCall> ToolCalls,
    int? TokensIn,
    int? TokensOut,
    string? ErrorCode = null,
    string? ErrorMessage = null);
