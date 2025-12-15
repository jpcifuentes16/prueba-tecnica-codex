using ApiOpenAI.Models;

namespace ApiOpenAI.Services;

public interface IOpenAIResponseService
{
    Task<StandardResponse> ClasificarIntencionAsync(ClasificarRequest request, CancellationToken cancellationToken);

    Task<StandardResponse> ResumirTextoAsync(ResumirRequest request, CancellationToken cancellationToken);
}
