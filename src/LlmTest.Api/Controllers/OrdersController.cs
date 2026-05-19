using Microsoft.AspNetCore.Mvc;

namespace LlmTest.Api.Controllers;

/// <summary>
/// Provides company order data.
/// </summary>
[ApiController]
[Route("api/orders")]
[Produces("application/json")]
public class OrdersController : ControllerBase
{
    private static readonly Dictionary<string, OrderDto> Orders = new(StringComparer.OrdinalIgnoreCase)
    {
        ["A001"] = new OrderDto("A001", "王小明", 3500, "已出貨")
    };

    /// <summary>
    /// Gets one order by order id.
    /// </summary>
    /// <param name="orderId">The order id.</param>
    /// <returns>The order detail.</returns>
    [HttpGet("{orderId}")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<OrderDto> GetOrder(string orderId)
    {
        return Orders.TryGetValue(orderId, out var order)
            ? Ok(order)
            : NotFound();
    }
}

/// <summary>
/// Company order data.
/// </summary>
/// <param name="OrderId">The order id.</param>
/// <param name="CustomerName">The customer name.</param>
/// <param name="Amount">The order amount.</param>
/// <param name="Status">The shipment status.</param>
public record OrderDto(string OrderId, string CustomerName, int Amount, string Status);
