using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

public class HttpTriggerFunction
{
    private readonly ILogger _logger;
    private readonly HttpClient _httpClient;

    public HttpTriggerFunction(ILoggerFactory loggerFactory, IHttpClientFactory httpClientFactory)
    {
        _logger = loggerFactory.CreateLogger<HttpTriggerFunction>();
        _httpClient = httpClientFactory.CreateClient("HttpTriggerFunctionClient"); // Named client
    }

    [Function("HttpTriggerFunction")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
    {
        var response = req.CreateResponse(HttpStatusCode.OK);

        try
        {
            var url = "http://localhost:8899";
            var result = await _httpClient.GetAsync(url);
            await response.WriteStringAsync($"Request to {url} returned status code {result.StatusCode}");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error calling localhost:8899");
            await response.WriteStringAsync("Error calling localhost:8899.");
        }

        return response;
    }
}