using System.Net.Http.Json;
using LlmTest.Api.Controllers;

namespace LlmTest.Api;

/// <summary>
/// Calls the company order API.
/// </summary>
public class CompanyApiClient
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CompanyApiClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    /// <summary>
    /// Gets one order from CompanyApi.
    /// </summary>
    /// <param name="orderId">The order id.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>The order detail.</returns>
    public async Task<OrderDto> GetOrder(string orderId, CancellationToken cancellationToken)
    {
        var httpClient = _httpClientFactory.CreateClient();
        var order = await httpClient.GetFromJsonAsync<OrderDto>(
            $"http://localhost:5000/api/orders/{Uri.EscapeDataString(orderId)}",
            cancellationToken);

        return order ?? throw new InvalidOperationException($"Order {orderId} was not found.");
    }
}
