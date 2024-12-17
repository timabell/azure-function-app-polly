using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

public class HttpTriggerFunction
{
    private readonly ILogger _logger;
    private readonly HttpClient _httpClient;

    public HttpTriggerFunction(ILoggerFactory loggerFactory, HttpClient httpClient)
    {
        _logger = loggerFactory.CreateLogger<HttpTriggerFunction>();
        _httpClient = httpClient;
    }

    [Function("HttpTriggerFunction")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        var response = req.CreateResponse(HttpStatusCode.OK);

        try
        {
            var result = await _httpClient.GetAsync("http://localhost:8899");
            if (result.IsSuccessStatusCode)
            {
                await response.WriteStringAsync("Request to localhost:8899 was successful.");
            }
            else
            {
                await response.WriteStringAsync("Request to localhost:8899 failed.");
            }
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error calling localhost:8899");
            await response.WriteStringAsync("Error calling localhost:8899");
        }

        return response;
    }
}