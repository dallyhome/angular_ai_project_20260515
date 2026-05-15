using System.Net;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;

namespace LlmTest.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GetTaiwanYahooController : ControllerBase
{
    private const string YahooTaiwanNewsRssUrl = "https://tw.news.yahoo.com/rss";
    private readonly HttpClient _httpClient;

    public GetTaiwanYahooController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaiwanYahooNewsItem>>> GetTaiwanYahoo(
        CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, YahooTaiwanNewsRssUrl);
        request.Headers.UserAgent.ParseAdd("Mozilla/5.0 LlmTest.Api/1.0");

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        var document = await XDocument.LoadAsync(stream, LoadOptions.None, cancellationToken);

        var news = document
            .Descendants("item")
            .Take(3)
            .Select(item =>
            {
                var publishDateText = item.Element("pubDate")?.Value;
                var publishDate = DateTime.TryParse(publishDateText, out var parsedPublishDate)
                    ? parsedPublishDate
                    : (DateTime?)null;

                return new TaiwanYahooNewsItem(
                    WebUtility.HtmlDecode(item.Element("title")?.Value ?? string.Empty),
                    item.Element("link")?.Value ?? string.Empty,
                    publishDate,
                    WebUtility.HtmlDecode(item.Element("description")?.Value ?? string.Empty));
            })
            .ToArray();

        return Ok(news);
    }
}

public record TaiwanYahooNewsItem(
    string Title,
    string Link,
    DateTime? PublishDate,
    string Summary);
