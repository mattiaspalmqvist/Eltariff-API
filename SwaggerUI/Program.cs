using System.Text.Json.Nodes;
using Microsoft.OpenApi.Models;

const string SWAGGER_UI_ROOT = "swagger";
const string API_SPECIFICATION = "gridtariffapi-latest.json";

string apiVersion = GetVersion();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(apiVersion, new OpenApiInfo
    {
        Title = "Grid Tariff API Documentation",
        Version = apiVersion
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint(API_SPECIFICATION, $"v{apiVersion}");
    c.RoutePrefix = SWAGGER_UI_ROOT;
});

app.MapGet("/", () => Results.Redirect(SWAGGER_UI_ROOT));

app.UseStaticFiles();

app.Run();

static string GetVersion()
{
    var openApiFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", SWAGGER_UI_ROOT, API_SPECIFICATION);

    try
    {
        var openApiStream = File.ReadAllText(openApiFilePath);
        var document = JsonNode.Parse(openApiStream);
        string? apiVersion = document?["info"]?["version"]?.ToString();

        ArgumentException.ThrowIfNullOrEmpty(apiVersion, "info.version");

        return apiVersion;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"ERROR: Failed to read OpenAPI version from file {openApiFilePath}: {ex.Message}");
        throw;
    }
}
