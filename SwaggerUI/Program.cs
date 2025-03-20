string wwwrootSpecificationDir = Path.Combine("wwwroot", "swagger", "specification");
Dictionary<string, string> swaggerUISpecifications = [];

string uiSpecificationDir = Path.Combine("wwwroot", "swagger", "specification", "ui");
foreach (var filePath in Directory.GetFiles(wwwrootSpecificationDir))
{
    string version = OpenApiTools.GetVersion(filePath);
    swaggerUISpecifications[version] = Path.GetFileName(filePath);
}

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseStaticFiles();
app.UseSwagger();
app.UseSwaggerUI(o =>
{
    bool isLatest = true;
    foreach (var (version, uiSpecificationFileName) in swaggerUISpecifications.OrderBy(x => x.Key).Reverse())
    {
        if (uiSpecificationFileName.Contains("-wip"))
        {
            o.SwaggerEndpoint($"specification/{uiSpecificationFileName}", $"{version} (wip)");
        }
        else if (isLatest)
        {
            o.SwaggerEndpoint($"specification/{uiSpecificationFileName}", $"{version} (latest)");
            isLatest = false;
        }
        else
        {
            o.SwaggerEndpoint($"specification/{uiSpecificationFileName}", version);
        }
    }

    o.RoutePrefix = "swagger";
    o.DocumentTitle = "Grid Tariff API";
    o.InjectStylesheet("ui/custom.css");
    o.InjectJavascript("ui/custom.js");
});

app.MapControllers();

app.Run();
