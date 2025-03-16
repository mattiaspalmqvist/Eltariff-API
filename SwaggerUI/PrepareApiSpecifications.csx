using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;

var args = Environment.GetCommandLineArgs()[2..];
string jsonDirectory = args[0];

JsonSerializerOptions jsonOptions = new()
{
    WriteIndented = true
};

foreach (string file in Directory.GetFiles(jsonDirectory, "*.json"))
{
    string jsonText = File.ReadAllText(file);
    if (JsonNode.Parse(jsonText) is JsonObject root)
    {
        string uiFile = Path.Combine(Path.GetDirectoryName(file), "ui", Path.GetFileName(file));
        File.WriteAllText(uiFile, EditJson(root).ToJsonString(jsonOptions));

        if (file.EndsWith("-latest.json"))
        {
            string version = root["info"]["version"].ToString();
            File.Move(file, file.Replace("-latest", $"-{version}"));
            File.Move(uiFile, uiFile.Replace("-latest", $"-{version}"));
        }
    }
    else
    {
        Console.WriteLine($"Skipping {file} - Not a valid JSON object.");
    }
}

static JsonObject EditJson(JsonObject obj)
{
    obj["security"] = new JsonArray();

    if (obj.TryGetPropertyValue("paths", out var paths) && paths is JsonObject pathsObj)
    {
        foreach (var path in pathsObj)
        {
            if (path.Value is JsonObject methods)
            {
                foreach (var method in methods)
                {
                    if (method.Value is JsonObject operation)
                    {
                        operation.Remove("security");
                    }
                }
            }
        }
    }

    if (obj.TryGetPropertyValue("components", out var components) && components is JsonObject componentsObj)
    {
        componentsObj.Remove("securitySchemes");
    }

    return obj;
}
