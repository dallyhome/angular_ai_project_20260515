using Microsoft.AspNetCore.Mvc;

namespace LlmTest.Api.Controllers;

/// <summary>
/// 模擬 CompanyApi 的訂單資料來源。
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
    /// CompanyApi 訂單查詢入口：依照訂單編號回傳訂單明細。
    /// </summary>
    /// <param name="orderId">訂單編號。</param>
    /// <returns>訂單明細；找不到則回傳 404。</returns>
    [HttpGet("{orderId}")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<OrderDto> GetOrder(string orderId)
    {
        // 目前使用記憶體資料模擬資料庫或公司內部 API。
        return Orders.TryGetValue(orderId, out var order)
            ? Ok(order)
            : NotFound();
    }
}

/// <summary>
/// CompanyApi 回傳的訂單資料格式。
/// </summary>
/// <param name="OrderId">訂單編號。</param>
/// <param name="CustomerName">客戶姓名。</param>
/// <param name="Amount">訂單金額。</param>
/// <param name="Status">出貨狀態。</param>
public record OrderDto(string OrderId, string CustomerName, int Amount, string Status);
