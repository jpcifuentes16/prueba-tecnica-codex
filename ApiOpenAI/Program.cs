using ApiOpenAI.Options;
using ApiOpenAI.Services;
using FastEndpoints;
using FastEndpoints.Swagger;
using OpenAI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints()
    .SwaggerDocument(o =>
    {
        o.DocumentSettings = s =>
        {
            s.Title = "API OpenAI Responses";
            s.Version = "v1";
        };
    });

builder.Services.Configure<OpenAIOptions>(builder.Configuration.GetSection(OpenAIOptions.SectionName));

var apiKey = builder.Configuration.GetSection(OpenAIOptions.SectionName).GetValue<string>(nameof(OpenAIOptions.ApiKey))
             ?? Environment.GetEnvironmentVariable("OPENAI_API_KEY")
             ?? string.Empty;

builder.Services.AddSingleton(new OpenAIClient(apiKey));
builder.Services.AddSingleton<IOpenAIResponseService, OpenAIResponseService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerGen();
}

app.UseHttpsRedirection();
app.UseFastEndpoints();
app.UseSwaggerUi();

app.Run();
