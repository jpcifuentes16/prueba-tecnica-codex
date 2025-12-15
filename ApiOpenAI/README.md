# API OpenAI Responses (.NET 8 + FastEndpoints)

API mínima en .NET 8 que expone endpoints para clasificar intención y resumir texto usando la API Responses del SDK oficial de OpenAI.

## Requisitos
- .NET 8 SDK instalado (se requiere la variable de entorno `DOTNET_ROOT`/`dotnet` en el PATH).
- Clave de OpenAI configurada en `OPENAI_API_KEY` o en `appsettings.json` dentro de la sección `OpenAI`.

## Configuración rápida
1. Copia el archivo `appsettings.json` y agrega tu clave:
   ```json
   {
     "OpenAI": {
       "ApiKey": "TU_API_KEY",
       "ClasificacionModel": "gpt-4.1-mini",
       "ResumenModel": "gpt-4.1-mini"
     }
   }
   ```
2. Restaura y ejecuta:
   ```bash
   dotnet restore
   dotnet run
   ```
3. Swagger disponible en `https://localhost:7199/swagger` (perfil HTTPS) o `http://localhost:5169/swagger` en desarrollo.

## Endpoints principales
- `POST /api/intencion/clasificar`
  - Body ejemplo:
    ```json
    { "inputUsuario": "Hola buenas tardes" }
    ```
- `POST /api/texto/resumir`
  - Body ejemplo:
    ```json
    {
      "texto": "FastEndpoints es un framework minimalista...",
      "maximoTokensRespuesta": 120
    }
    ```

Las respuestas devuelven `isSuccess`, `response`, `dollarCost` y `errorMessage` en un contrato estándar.

## Postman
Importa `PostmanCollection.json` para probar rápidamente los endpoints con ejemplos precargados.
