using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;

namespace LlmTest.Api.Controllers;

/// <summary>
/// 接收 Angular 傳來的自然語言問題，判斷要呼叫哪一個 MCP tool。
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AgentController : ControllerBase
{
    private static readonly Regex OrderIdRegex = new(
        @"\b[A-Za-z]\d{3,}\b",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    private readonly OrderMcpClient _orderMcpClient;

    /// <summary>
    /// 注入 MCP Client，讓 Agent API 可以透過 MCP 查詢訂單資料。
    /// </summary>
    /// <param name="orderMcpClient">訂單 MCP Client。</param>
    public AgentController(OrderMcpClient orderMcpClient)
    {
        _orderMcpClient = orderMcpClient;
    }

    /// <summary>
    /// Agent 主要入口：接收使用者訊息，解析訂單編號，呼叫 MCP Client，最後組成回答。
    /// </summary>
    /// <param name="request">Angular 傳入的使用者訊息，例如：幫我查訂單 A001。</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>Agent 回覆文字。</returns>
    [HttpPost("ask")]
    [ProducesResponseType(typeof(AgentAskResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(AgentAskResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AgentAskResponse>> Ask(
        [FromBody] AgentAskRequest request,
        CancellationToken cancellationToken)
    {
        // 檢查使用者是否有輸入內容，沒有內容就直接回傳錯誤訊息。
        if (string.IsNullOrWhiteSpace(request.Message))
        {
            return BadRequest(new AgentAskResponse("請輸入要查詢的內容。"));
        }

        // 從自然語言中找出訂單編號，例如從「幫我查訂單 A001」解析出 A001。
        var orderId = TryGetOrderId(request.Message);
        if (orderId is null)
        {
            return BadRequest(new AgentAskResponse("找不到訂單編號，請輸入例如：幫我查訂單 A001。"));
        }

        // 呼叫 MCP Client 的 GetOrder tool，取得訂單資料後組成給前端看的回答。
        var order = await _orderMcpClient.GetOrder(orderId, cancellationToken);
        return Ok(new AgentAskResponse(
            $"訂單 {order.OrderId}，{order.CustomerName}，{order.Amount}，{order.Status}"));
    }

    /// <summary>
    /// 從使用者輸入文字中擷取訂單編號。
    /// </summary>
    /// <param name="message">使用者輸入文字。</param>
    /// <returns>大寫訂單編號；如果找不到則回傳 null。</returns>
    private static string? TryGetOrderId(string message)
    {
        var match = OrderIdRegex.Match(message);
        return match.Success ? match.Value.ToUpperInvariant() : null;
    }
}

/// <summary>
/// Agent ask request payload.
/// </summary>
/// <param name="Message">使用者輸入文字。</param>
public record AgentAskRequest(string Message);

/// <summary>
/// Agent ask response payload.
/// </summary>
/// <param name="Answer">Agent 回覆文字。</param>
public record AgentAskResponse(string Answer);
