# API OpenAI Responses (.NET 8 + FastEndpoints)

API mínima que expone endpoints para clasificar intención de mensajes y resumir texto usando el SDK oficial de OpenAI. Incluye validación de entrada, manejo de errores en español y cálculo de costo aproximado por tokens.

## Requisitos previos
- .NET SDK 8 instalado
- Variable de entorno `OPENAI_API_KEY` con la clave de OpenAI (también puede configurarse en `appsettings.*`)

## Ejecución
```bash
dotnet restore
dotnet run
```
La API arranca por defecto en `http://localhost:5177` (configuración de Kestrel por defecto en Development).

## Endpoints
- **POST `/api/intencion/clasificar`**
  - Body: `{ "inputUsuario": "Hola buenas tardes" }`
  - Devuelve la intención estimada y el costo aproximado.
- **POST `/api/texto/resumir`**
  - Body: `{ "texto": "Contenido a resumir" }`
  - Devuelve un resumen breve y el costo aproximado.

Las respuestas siguen la forma:
```json
{
  "isSuccess": true,
  "response": "...",
  "dollarCost": 0.00002085,
  "errorMessage": null
}
```

## Swagger
La exploración vía Swagger está habilitada automáticamente en `http://localhost:5177/swagger` en ambiente Development.

## Postman
Se incluye `PostmanCollection.json` con ejemplos listos para importar. Ajusta la variable `baseUrl` según el puerto de ejecución.

## Notas de implementación
- Validación con FluentValidation integrada en FastEndpoints.
- Estimación de costo basada en tokens utilizados y tarifa `0.000015` USD por 1K tokens (ajustable en `OpenAIResponseService`).
