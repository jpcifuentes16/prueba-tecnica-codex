# API de ejemplo con FastEndpoints + OpenAI Responses

Implementación mínima en .NET 8 que expone endpoints para clasificar intención de usuario y resumir texto utilizando la API de Responses del cliente oficial de OpenAI.

## Requisitos previos
- .NET 8 SDK.
- Una API Key válida de OpenAI (`OPENAI_API_KEY`).

## Ejecución
1. Instala dependencias y compila:
   ```bash
   dotnet restore
   dotnet build
   ```
2. Ejecuta el proyecto (dentro de `ApiOpenAI`):
   ```bash
   dotnet run
   ```
3. Explora la API en Swagger: http://localhost:5000/swagger (redirección desde `/`).

## Configuración
- Variables de entorno recomendadas:
  - `OPENAI_API_KEY`: clave de OpenAI (tiene prioridad sobre `appsettings`).
- Configuración opcional en `appsettings.json` / `appsettings.Development.json`:
  ```json
  {
    "OpenAI": {
      "ApiKey": "",
      "DefaultModel": "gpt-4.1-mini",
      "MaxOutputTokens": 300
    }
  }
  ```

## Endpoints principales
- **POST** `/api/intencion/clasificar`
  - Body de ejemplo: `{ "inputUsuario": "Hola buenas tardes" }`
- **POST** `/api/texto/resumir`
  - Body de ejemplo: `{ "texto": "FastEndpoints permite definir endpoints de forma fluida y concisa.", "nivelDetalle": "breve" }`

Cada respuesta sigue el contrato `StandardResponse`:
```json
{
  "isSuccess": true,
  "response": "...contenido generado...",
  "dollarCost": 0.00012,
  "errorMessage": null
}
```

## Colección de Postman
Importa `ApiOpenAI/ApiOpenAI.postman_collection.json` para probar las rutas con ejemplos listos.
