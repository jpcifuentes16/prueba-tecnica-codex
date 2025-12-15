using ApiOpenAI.Services;
using FastEndpoints;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<OpenAIOptions>().Bind(builder.Configuration.GetSection(OpenAIOptions.SectionName));
builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument();
builder.Services.AddSingleton<IOpenAIResponseService, OpenAIResponseService>();

var app = builder.Build();

app.UseDefaultExceptionHandler();
app.UseFastEndpoints();
app.UseSwaggerGen();

app.MapGet("/", () => "API de OpenAI Responses operativa");

app.Run();
