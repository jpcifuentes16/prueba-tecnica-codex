# API OpenAI con FastEndpoints

API mínima en .NET 8 que expone endpoints para clasificar intención y resumir texto usando la API de Responses del cliente oficial de OpenAI.

## Requisitos previos
- .NET 8 SDK
- Variable de entorno `OPENAI_API_KEY` con la clave de OpenAI (o completa `OpenAI:ApiKey` en `appsettings.*.json`).

## Ejecución local
```bash
cd ApiOpenAI
# Restaurar dependencias y ejecutar
DOTNET_ENVIRONMENT=Development dotnet run
```

Swagger queda disponible en `http://localhost:5000/swagger` (o el puerto configurado).

## Endpoints
- **POST** `/api/intencion/clasificar`
  - Body: `{ "inputUsuario": "Hola buenas tardes" }`
- **POST** `/api/texto/resumir`
  - Body: `{ "texto": "FastEndpoints facilita la creación de APIs rápidas en .NET.", "nivelDetalle": "breve" }`

Todas las respuestas usan el contrato:
```json
{
  "isSuccess": true,
  "response": "...respuesta de OpenAI...",
  "dollarCost": 0.0001,
  "errorMessage": null
}
```

Los errores de validación se devuelven con `isSuccess=false` y un `errorMessage` en español.

## Configuración de costos
En `appsettings.json` puedes ajustar los precios aproximados por 1K tokens:
```json
"OpenAI": {
  "Model": "gpt-4.1-mini",
  "InputTokenPricePer1K": 0.00015,
  "OutputTokenPricePer1K": 0.0006
}
```

## Colección Postman
Revisa `postman/OpenAI-FastEndpoints.postman_collection.json` para ejemplos listos de invocación.
