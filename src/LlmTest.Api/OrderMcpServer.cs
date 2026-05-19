using LlmTest.Api.Controllers;

namespace LlmTest.Api;

/// <summary>
/// MCP Server 邊界，集中放置可被 Agent 呼叫的訂單 tools。
/// </summary>
public class OrderMcpServer
{
    private readonly CompanyApiClient _companyApiClient;

    /// <summary>
    /// 注入 CompanyApiClient，讓 MCP tool 可以取得公司訂單資料。
    /// </summary>
    /// <param name="companyApiClient">CompanyApi HTTP client。</param>
    public OrderMcpServer(CompanyApiClient companyApiClient)
    {
        _companyApiClient = companyApiClient;
    }

    /// <summary>
    /// MCP Tool：GetOrder(orderId)，負責依照訂單編號查詢 CompanyApi。
    /// </summary>
    /// <param name="orderId">訂單編號。</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>訂單明細。</returns>
    public Task<OrderDto> GetOrder(string orderId, CancellationToken cancellationToken)
    {
        return _companyApiClient.GetOrder(orderId, cancellationToken);
    }
}
