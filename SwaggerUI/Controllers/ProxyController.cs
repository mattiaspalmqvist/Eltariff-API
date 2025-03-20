using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;

namespace SwaggerUI.Controllers;

[Route("api/proxy")]
[ApiController]
public class ProxyController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly List<string> _validServers = [];

    public ProxyController(HttpClient httpClient)
    {
        _httpClient = httpClient;

        var filePath = Path.Combine("servers.json");
        var jsonNode = JsonNode.Parse(System.IO.File.ReadAllText(filePath));
        var servers = (jsonNode?["servers"]) ?? throw new FileNotFoundException($"Failed to get or read data from {filePath}");

        foreach (var item in servers.AsArray())
        {
            string? server = item?["url"]?.ToString();
            if (server != null)
            {
                _validServers.Add(server);
            }
            else
            {
                throw new JsonException($"Failed to get url json data from {filePath}");
            }
        }
    }

    [HttpGet, HttpPost]
    public async Task<IActionResult> ProxyRequest(
        [FromQuery] string server, [FromQuery] string version, [FromQuery] string endpoint)
    {
        if (string.IsNullOrEmpty(server) || string.IsNullOrEmpty(version) || string.IsNullOrEmpty(endpoint))
        {
            return BadRequest("Incorrect formatted request. Could not extract server, version and endpoint.");
        }

        if (!_validServers.Contains(server))
        {
            return BadRequest($"The provided server url {server} is not a valid server.");
        }

        var apiUrl = $"{server}/{version}/{endpoint}";
        using HttpRequestMessage requestMessage = await CreateRequestMessage(apiUrl);

        var response = await _httpClient.SendAsync(requestMessage);
        string responseContent = await response.Content.ReadAsStringAsync();

        var result = new ContentResult
        {
            Content = responseContent,
            StatusCode = (int)response.StatusCode,
            ContentType = response.Content.Headers.ContentType?.MediaType ?? "application/json"
        };
        AddResponseHeaders(response);

        return result;
    }

    private async Task<HttpRequestMessage> CreateRequestMessage(string apiUrl)
    {
        var method = new HttpMethod(Request.Method);
        var requestMessage = new HttpRequestMessage(method, apiUrl);
        AddRequestHeaders(requestMessage);
        await AddRequestBodyIfPresent(requestMessage);
        return requestMessage;
    }

    private async Task AddRequestBodyIfPresent(HttpRequestMessage requestMessage)
    {
        if (Request.ContentLength > 0)
        {
            var requestBody = new StreamReader(Request.Body);
            string content = await requestBody.ReadToEndAsync();
            requestMessage.Content = new StringContent(content, Encoding.UTF8, Request.ContentType ?? "application/json");
        }
    }

    private void AddRequestHeaders(HttpRequestMessage requestMessage)
    {
        string dotnetVersion = RuntimeInformation.FrameworkDescription;
        requestMessage.Headers.Add("User-Agent", $"{dotnetVersion} HttpClient");
        List<string> headersToForward = ["Accept", "Referer"];
        foreach (var header in Request.Headers)
        {
            if (!headersToForward.Contains(header.Key)) continue;
            requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToString());
        }
    }

    private void AddResponseHeaders(HttpResponseMessage response)
    {
        if (response.Content.Headers != null)
        {
            foreach (var header in response.Content.Headers)
            {
                Response.Headers[header.Key] = header.Value.ToArray();
            }
        }
    }
}
