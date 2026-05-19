using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;

namespace LlmTest.Api.Controllers;

/// <summary>
/// Handles natural-language agent requests from the client.
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

    public AgentController(OrderMcpClient orderMcpClient)
    {
        _orderMcpClient = orderMcpClient;
    }

    /// <summary>
    /// Asks the agent to answer a user prompt.
    /// </summary>
    /// <param name="request">The user prompt.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>The agent response.</returns>
    [HttpPost("ask")]
    [ProducesResponseType(typeof(AgentAskResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(AgentAskResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AgentAskResponse>> Ask(
        [FromBody] AgentAskRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Message))
        {
            return BadRequest(new AgentAskResponse("請輸入要查詢的內容。"));
        }

        var orderId = TryGetOrderId(request.Message);
        if (orderId is null)
        {
            return BadRequest(new AgentAskResponse("找不到訂單編號，請輸入例如：幫我查訂單 A001。"));
        }

        var order = await _orderMcpClient.GetOrder(orderId, cancellationToken);
        return Ok(new AgentAskResponse(
            $"訂單 {order.OrderId}，{order.CustomerName}，{order.Amount}，{order.Status}"));
    }

    private static string? TryGetOrderId(string message)
    {
        var match = OrderIdRegex.Match(message);
        return match.Success ? match.Value.ToUpperInvariant() : null;
    }
}

/// <summary>
/// Agent ask request payload.
/// </summary>
/// <param name="Message">The user prompt.</param>
public record AgentAskRequest(string Message);

/// <summary>
/// Agent ask response payload.
/// </summary>
/// <param name="Answer">The answer text.</param>
public record AgentAskResponse(string Answer);
