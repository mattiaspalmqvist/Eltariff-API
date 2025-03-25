using Microsoft.AspNetCore.Mvc;
using GeneratedController;
using Microsoft.AspNetCore.Http;

namespace ExampleController;

public class ExampleControllerImplementation : ControllerBase, IGeneratedController
{
    public async Task<ActionResult<InfoResponse>> GetInfoAsync(string v)
    {
        await Task.Delay(0);
        return StatusCode(StatusCodes.Status501NotImplemented, $"GET /{v}/info is not implemented.");
    }

    public async Task<ActionResult<PricesResponse>> GetPricesAsync(string v, Guid componentId, string duration)
    {
        await Task.Delay(0);
        return StatusCode(StatusCodes.Status501NotImplemented, $"GET /{v}/prices/{{id}} is not implemented.");
    }

    public async Task<ActionResult<TariffResponse>> GetTariffByIdAsync(string v, Guid id)
    {
        await Task.Delay(0);
        return StatusCode(StatusCodes.Status501NotImplemented, $"GET /{v}/tariffs/{{id}} is not implemented.");
    }

    public async Task<ActionResult<TariffsResponse>> GetTariffsAsync(string v)
    {
        await Task.Delay(0);
        return StatusCode(StatusCodes.Status501NotImplemented, $"GET /{v}/tariffs is not implemented.");
    }

    public async Task<ActionResult<TariffsSearchResponse>> SearchTariffsAsync(TariffsSearchRequest body, string v)
    {
        await Task.Delay(0);
        return StatusCode(StatusCodes.Status501NotImplemented, $"POST /{v}/tariffs/search is not implemented.");
    }
}