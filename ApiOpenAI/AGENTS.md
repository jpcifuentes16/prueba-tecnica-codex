# AGENTS

## Descripción general
- API minimal en .NET 8 orientada a exponer endpoints con FastEndpoints y consumir el cliente `OpenAI` para tareas como resumen y clasificación de intención.
- Proyecto con nullability activa y `ImplicitUsings` habilitado.
- Swagger/OpenAPI se puede habilitar vía `FastEndpoints.Swagger` para exploración interactiva.

## Estructura y stack
- Raíz: `Program.cs` (bootstrap minimal API).
- `Endpoints/`: puntos de entrada FastEndpoints (p. ej. `ResumirTextoEndpoint.cs`, `ClasificarIntencionEndpoint.cs`).
- `Models/`: contratos de request/response (`ResumirRequest.cs`, `ClasificarRequest.cs`, `StandardResponse.cs`).
- `Services/`: capa para integrar con `OpenAI` u otras dependencias externas (pendiente de implementación).
- Stack: .NET 8, FastEndpoints 7.x, FastEndpoints.Swagger, SDK `OpenAI 2.*`.

## Setup, build y pruebas
- Restaurar dependencias: `dotnet restore`
- Compilar: `dotnet build`
- Ejecutar (perfil Development): `dotnet run`
- Tests (si existen proyectos de pruebas): `dotnet test`
- Variables típicas: configurar `OPENAI_API_KEY` en entorno o `appsettings.*` antes de llamar al servicio de OpenAI.

## Guías de estilo y convenciones
- C#: PascalCase para tipos/públicos; camelCase para variables locales/privadas; usa `readonly`/`const` cuando aplique.
- Nullability habilitada: evita `!`; valida input en endpoints y modelos.
- FastEndpoints: hereda de `Endpoint<TRequest, TResponse>` o equivalentes; define `Configure()` para rutas/verbo; usa `Validator` con FluentValidation cuando se pueda.
- DI: registra servicios en `Program.cs`; prefiere interfaces para servicios externos; no acoples lógica a endpoints.
- Manejo de errores: responde con tipos de resultado consistentes (`StandardResponse`) y códigos HTTP claros; loguea excepciones con `ILogger`.
- Formato: sigue editorconfig habitual de .NET; `using` ordenados; evita comentarios redundantes, documenta solo decisiones no obvias.

## Testing y calidad
- Pruebas unitarias con xUnit/NUnit/MSTest (elige una y sé consistente); aislar llamadas a OpenAI usando mocks o fakes.
- Valida endpoints con pruebas de integración usando `WebApplicationFactory` o similar.
- Añade validadores de entrada y casos límite (inputs vacíos, largos, caracteres especiales).
- Ejecuta `dotnet test` en CI/local antes de publicar; incluye linting/format (p. ej. `dotnet format`) para mantener estilo.
- Si se expone Swagger, verifica que los contratos y ejemplos reflejen los modelos reales.
