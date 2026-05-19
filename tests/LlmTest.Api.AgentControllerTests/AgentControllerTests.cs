using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using LlmTest.Api;
using LlmTest.Api.Controllers;
using Microsoft.AspNetCore.Mvc;

internal sealed class AgentControllerTests
{
    public async Task Ask_ReturnsOrderAnswer_WhenMessageHasOrderId()
    {
        // 測試主要成功流程：Angular 訊息 -> AgentController -> MCP Client -> CompanyApiClient -> 回覆訂單資訊。
        var controller = CreateController();

        var result = await controller.Ask(
            new AgentAskRequest("幫我查訂單 A001"),
            CancellationToken.None);

        var okResult = AssertIsType<OkObjectResult>(result.Result);
        var response = AssertIsType<AgentAskResponse>(okResult.Value);

        AssertEqual("訂單 A001，王小明，3500，已出貨", response.Answer);
    }

    public async Task Ask_ReturnsBadRequest_WhenMessageIsEmpty()
    {
        // 測試防呆流程：使用者沒有輸入內容時，應回傳 400。
        var controller = CreateController();

        var result = await controller.Ask(
            new AgentAskRequest("   "),
            CancellationToken.None);

        var badRequestResult = AssertIsType<BadRequestObjectResult>(result.Result);
        var response = AssertIsType<AgentAskResponse>(badRequestResult.Value);

        AssertEqual("請輸入要查詢的內容。", response.Answer);
    }

    public async Task Ask_ReturnsBadRequest_WhenMessageHasNoOrderId()
    {
        // 測試解析失敗流程：文字中沒有 A001 這類訂單編號時，應回傳 400。
        var controller = CreateController();

        var result = await controller.Ask(
            new AgentAskRequest("幫我查訂單"),
            CancellationToken.None);

        var badRequestResult = AssertIsType<BadRequestObjectResult>(result.Result);
        var response = AssertIsType<AgentAskResponse>(badRequestResult.Value);

        AssertEqual("找不到訂單編號，請輸入例如：幫我查訂單 A001。", response.Answer);
    }

    private static AgentController CreateController()
    {
        // 使用假的 HttpClientFactory，避免測試時真的打 localhost:5000。
        var httpClientFactory = new FakeHttpClientFactory();
        var companyApiClient = new CompanyApiClient(httpClientFactory);
        var orderMcpServer = new OrderMcpServer(companyApiClient);
        var orderMcpClient = new OrderMcpClient(orderMcpServer);

        return new AgentController(orderMcpClient);
    }

    private static T AssertIsType<T>(object? value)
    {
        if (value is T typedValue)
        {
            return typedValue;
        }

        throw new InvalidOperationException(
            $"Assert failed. Expected type {typeof(T).Name}, actual type {value?.GetType().Name ?? "null"}.");
    }

    private static void AssertEqual<T>(T expected, T actual)
    {
        if (!EqualityComparer<T>.Default.Equals(expected, actual))
        {
            throw new InvalidOperationException(
                $"Assert failed. Expected: {expected}; Actual: {actual}");
        }
    }
}

internal sealed class FakeHttpClientFactory : IHttpClientFactory
{
    public HttpClient CreateClient(string name)
    {
        return new HttpClient(new FakeCompanyApiHandler())
        {
            BaseAddress = new Uri("http://localhost:5000")
        };
    }
}

internal sealed class FakeCompanyApiHandler : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        // 模擬 CompanyApi /api/orders/A001 的回應。
        if (request.Method == HttpMethod.Get && request.RequestUri?.AbsolutePath == "/api/orders/A001")
        {
            var order = new OrderDto("A001", "王小明", 3500, "已出貨");
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(order, options: new JsonSerializerOptions(JsonSerializerDefaults.Web))
            });
        }

        return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
    }
}
