using EternalKids.Application.DTOs;

namespace EternalKids.Application.Contracts;

public interface IAiModelClient
{
    Task<AiModelResponse> GenerateAsync(AiModelRequest request, CancellationToken ct = default);
}
