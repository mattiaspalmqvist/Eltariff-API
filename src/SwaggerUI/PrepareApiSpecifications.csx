#load "OpenApiTools.cs"

using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;

var args = Environment.GetCommandLineArgs()[2..];
string jsonDirectory = args[0];

foreach (string filePath in Directory.GetFiles(jsonDirectory, "*.json"))
{
    OpenApiTools.EditFile(filePath);
}
