using ApiOpenAI.Models;

namespace ApiOpenAI.Services;

public interface IOpenAIResponseService
{
    Task<StandardResponse> ClasificarAsync(ClasificarRequest request, CancellationToken cancellationToken = default);

    Task<StandardResponse> ResumirAsync(ResumirRequest request, CancellationToken cancellationToken = default);
}
