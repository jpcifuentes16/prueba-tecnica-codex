using ApiOpenAI.Models;
using ApiOpenAI.Services;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.Extensions.Options;
using OpenAI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument();

builder.Services.Configure<OpenAIOptions>(builder.Configuration.GetSection(OpenAIOptions.SectionName));

builder.Services.AddSingleton<OpenAIClient>(sp =>
{
    var options = sp.GetRequiredService<IOptions<OpenAIOptions>>().Value;
    var apiKey = options.ApiKey ?? Environment.GetEnvironmentVariable("OPENAI_API_KEY");

    if (string.IsNullOrWhiteSpace(apiKey))
    {
        throw new InvalidOperationException("No se encontró la clave de OpenAI. Establece la variable de entorno OPENAI_API_KEY o el valor OpenAI:ApiKey en la configuración.");
    }

    return new OpenAIClient(apiKey);
});

builder.Services.AddScoped<IOpenAIResponseService, OpenAIResponseService>();

var app = builder.Build();

app.UseFastEndpoints();
app.UseSwaggerGen();
app.UseSwaggerUi3(options => options.ConfigureDefaults());

app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();
