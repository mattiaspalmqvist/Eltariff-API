#nullable enable

using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;

public class OpenApiTools()
{
    public static string GetVersion(string openApiFilePath)
    {
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

    public static void EditFile(string filePath)
    {
        JsonSerializerOptions jsonOptions = new()
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        string jsonText = File.ReadAllText(filePath);
        if (JsonNode.Parse(jsonText) is JsonObject root)
        {
            File.Delete(filePath);
            File.WriteAllText(filePath, EditJson(root).ToJsonString(jsonOptions));

            if (filePath.EndsWith("-latest.json") && root.TryGetPropertyValue("info", out var info) && info is JsonObject infoObj)
            {
                infoObj.TryGetPropertyValue("version", out var version);
                File.Move(filePath, filePath.Replace("-latest", $"-v{version}"));
            }
        }
        else
        {
            Console.WriteLine($"Skipping {filePath} - Not a valid JSON object.");
        }
    }

    public static JsonObject EditJson(JsonObject root)
    {
        // AddServers(root);

        return root;
    }

    public static void AddServers(JsonObject root)
    {
        var servers = GetServers();
        if (servers != null)
        {
            root["servers"] = servers.DeepClone();
        }
    }

    public static JsonNode? GetServers()
    {
        string filePath = "servers.json";
        string jsonText = File.ReadAllText(filePath);
        var root = JsonNode.Parse(jsonText);
        return root?["servers"];
    }
}
