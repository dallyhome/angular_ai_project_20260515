using System.Net.Http.Json;
using LlmTest.Api.Controllers;

namespace LlmTest.Api;

/// <summary>
/// 負責呼叫 CompanyApi 的 HTTP client。
/// </summary>
public class CompanyApiClient
{
    private readonly IHttpClientFactory _httpClientFactory;

    /// <summary>
    /// 注入 HttpClientFactory，建立對 CompanyApi 的 HTTP 呼叫。
    /// </summary>
    /// <param name="httpClientFactory">ASP.NET Core HttpClient factory。</param>
    public CompanyApiClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    /// <summary>
    /// 呼叫 CompanyApi /api/orders/{orderId}，取得指定訂單資料。
    /// </summary>
    /// <param name="orderId">訂單編號。</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>訂單明細。</returns>
    public async Task<OrderDto> GetOrder(string orderId, CancellationToken cancellationToken)
    {
        // 這裡模擬 MCP Server 對公司內部 API 的實際 HTTP 呼叫。
        var httpClient = _httpClientFactory.CreateClient();
        var order = await httpClient.GetFromJsonAsync<OrderDto>(
            $"http://localhost:5000/api/orders/{Uri.EscapeDataString(orderId)}",
            cancellationToken);

        return order ?? throw new InvalidOperationException($"Order {orderId} was not found.");
    }
}
