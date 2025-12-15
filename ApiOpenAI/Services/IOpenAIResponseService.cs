using ApiOpenAI.Models;

namespace ApiOpenAI.Services;

public interface IOpenAIResponseService
{
    Task<StandardResponse> ClasificarIntencionAsync(string inputUsuario, CancellationToken cancellationToken);

    Task<StandardResponse> ResumirTextoAsync(string texto, string? nivelDetalle, CancellationToken cancellationToken);
}
