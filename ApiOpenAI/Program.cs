using ApiOpenAI.Models;
using ApiOpenAI.Services;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.Extensions.Options;
using OpenAI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<OpenAiOptions>(builder.Configuration.GetSection(OpenAiOptions.SectionName));

builder.Services.AddSingleton<OpenAIClient>(sp =>
{
    var options = sp.GetRequiredService<IOptions<OpenAiOptions>>().Value;
    var apiKey = options.ApiKey ?? Environment.GetEnvironmentVariable("OPENAI_API_KEY");

    if (string.IsNullOrWhiteSpace(apiKey))
    {
        throw new InvalidOperationException("La API key de OpenAI no está configurada. Usa OPENAI_API_KEY o appsettings.");
    }

    return new OpenAIClient(apiKey);
});

builder.Services.AddSingleton<IUsageCostCalculator, UsageCostCalculator>();
builder.Services.AddScoped<IOpenAIResponseService, OpenAIResponseService>();

builder.Services.AddFastEndpoints();
builder.Services.AddSwaggerDoc(s =>
{
    s.DocumentName = "v1";
    s.Title = "API OpenAI Responses";
    s.Version = "v1";
});

var app = builder.Build();

app.UseFastEndpoints();
app.UseSwaggerGen();

app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();
