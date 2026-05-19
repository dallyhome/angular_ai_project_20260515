using LlmTest.Api.Controllers;

namespace LlmTest.Api;

/// <summary>
/// Agent API 使用的 MCP Client 邊界。
/// </summary>
public class OrderMcpClient
{
    private readonly OrderMcpServer _orderMcpServer;

    /// <summary>
    /// 注入 MCP Server，讓 Client 可以呼叫 server 上定義的 tool。
    /// </summary>
    /// <param name="orderMcpServer">訂單 MCP Server。</param>
    public OrderMcpClient(OrderMcpServer orderMcpServer)
    {
        _orderMcpServer = orderMcpServer;
    }

    /// <summary>
    /// MCP Client 對外方法：呼叫 GetOrder(orderId) tool 查詢訂單。
    /// </summary>
    /// <param name="orderId">訂單編號。</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>訂單明細。</returns>
    public Task<OrderDto> GetOrder(string orderId, CancellationToken cancellationToken)
    {
        return _orderMcpServer.GetOrder(orderId, cancellationToken);
    }
}
