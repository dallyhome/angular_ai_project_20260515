using LlmTest.Api.Controllers;

namespace LlmTest.Api;

/// <summary>
/// MCP server boundary for order tools.
/// </summary>
public class OrderMcpServer
{
    private readonly CompanyApiClient _companyApiClient;

    public OrderMcpServer(CompanyApiClient companyApiClient)
    {
        _companyApiClient = companyApiClient;
    }

    /// <summary>
    /// Tool: GetOrder(orderId).
    /// </summary>
    /// <param name="orderId">The order id.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>The order detail.</returns>
    public Task<OrderDto> GetOrder(string orderId, CancellationToken cancellationToken)
    {
        return _companyApiClient.GetOrder(orderId, cancellationToken);
    }
}
