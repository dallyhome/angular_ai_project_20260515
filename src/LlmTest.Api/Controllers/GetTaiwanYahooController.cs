using System.Net;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;

namespace LlmTest.Api.Controllers;

/// <summary>
/// Provides Yahoo Taiwan news data.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class GetTaiwanYahooController : ControllerBase
{
    private const string YahooTaiwanNewsRssUrl = "https://tw.news.yahoo.com/rss";
    private readonly HttpClient _httpClient;

    public GetTaiwanYahooController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    /// <summary>
    /// Gets the latest Yahoo Taiwan news items from the public RSS feed.
    /// </summary>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>A list of Yahoo Taiwan news items.</returns>
    /// <response code="200">Returns the latest news items.</response>
    /// <response code="500">The RSS feed could not be loaded.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TaiwanYahooNewsItem>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

/// <summary>
/// Yahoo Taiwan news item.
/// </summary>
/// <param name="Title">News title.</param>
/// <param name="Link">News URL.</param>
/// <param name="PublishDate">News publish date.</param>
/// <param name="Summary">News summary.</param>
public record TaiwanYahooNewsItem(
    string Title,
    string Link,
    DateTime? PublishDate,
    string Summary);
