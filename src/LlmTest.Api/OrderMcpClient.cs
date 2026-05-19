using LlmTest.Api.Controllers;

namespace LlmTest.Api;

/// <summary>
/// MCP client boundary used by the agent API.
/// </summary>
public class OrderMcpClient
{
    private readonly OrderMcpServer _orderMcpServer;

    public OrderMcpClient(OrderMcpServer orderMcpServer)
    {
        _orderMcpServer = orderMcpServer;
    }

    /// <summary>
    /// Calls MCP tool GetOrder(orderId).
    /// </summary>
    /// <param name="orderId">The order id.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>The order detail.</returns>
    public Task<OrderDto> GetOrder(string orderId, CancellationToken cancellationToken)
    {
        return _orderMcpServer.GetOrder(orderId, cancellationToken);
    }
}
