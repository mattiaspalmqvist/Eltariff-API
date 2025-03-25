using Microsoft.AspNetCore.Mvc;
using GeneratedController;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Nodes;

namespace ExampleController;

public class ExampleControllerImplementation : ControllerBase, IGeneratedController
{
    public async Task<ActionResult<InfoResponse>> GetInfoAsync(string v)
    {
        JsonNode json = await JsonDataLoader.LoadApiSpecification("gridtariffapi-wip.json");
        string? apiName = json["info"]?["title"]?.ToString();
        string? apiVersion = json["info"]?["version"]?.ToString();
        string implementationRevision = "r1";

        var additionalProperties = new Dictionary<string, object>
        {
            { "metadata1", "Value1" },
            { "metadata2", "Value2" }
        };

        var info = new InfoResponse
        {
            Name = apiName,
            ApiVersion = apiVersion,
            ImplementationVersion = $"{apiVersion}-{implementationRevision}",
            Operator = "The Grid Company AB",
            LastUpdated = DateTime.Parse("2025-03-25"),
            AdditionalProperties = additionalProperties
        };
        return Ok(info);
    }

    public async Task<ActionResult<PricesResponse>> GetPricesAsync(string v, Guid componentId, string duration)
    {
        await Task.Delay(0);
        return StatusCode(StatusCodes.Status501NotImplemented, $"GET /{v}/prices/{{id}} is not implemented.");
    }

    public async Task<ActionResult<TariffResponse>> GetTariffByIdAsync(string v, Guid id)
    {
        var tariffsResponse = await JsonDataLoader.LoadResponseDataAsync<TariffsResponse>("tariffs.json");
        foreach (var tariff in tariffsResponse.Tariffs)
        {
            if (tariff.Id == id)
            {
                return new TariffResponse
                {
                    Tariff = tariff,
                };
            }
        }
        return NotFound($"Tariff with id {id} was not found.");
    }

    public async Task<ActionResult<TariffsResponse>> GetTariffsAsync(string v)
    {
        return await JsonDataLoader.LoadResponseDataAsync<TariffsResponse>("tariffs.json");
    }

    public async Task<ActionResult<TariffsSearchResponse>> SearchTariffsAsync(TariffsSearchRequest body, string v)
    {
        await Task.Delay(0);
        return StatusCode(StatusCodes.Status501NotImplemented, $"POST /{v}/tariffs/search is not implemented.");
    }
}