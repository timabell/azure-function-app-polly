using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;

public class Program
{
    public static void Main(string[] args)
    {
        var host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults()
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                services.AddHttpClient("HttpTriggerFunctionClient")
                    .AddPolicyHandler((serviceProvider, request) => 
                        GetRetryPolicy(serviceProvider.GetRequiredService<ILogger<Program>>()));
                services.AddScoped<HttpTriggerFunction>();
            })
            .Build();

        host.Run();
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(ILogger logger)
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(
                retryCount: 3,
                // sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(1), // constant delay http://backoffcalculator.com/?interval=1&attempts=3&rate=1
                // sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(1 * retryAttempt), // linear delay (1 sec, attempt, wait 2 secs, attempt, wait 3 secs, final attempt)
                sleepDurationProvider: retryAttempt => 0.5 * TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), // exponential - 1, 2, 4
                onRetry: (outcome, timespan, retryAttempt, context) =>
                {
                    logger.LogWarning($"Retry {retryAttempt} encountered an error: {outcome.Exception?.Message} {outcome.Result?.StatusCode}. Waiting {timespan} before next retry.");
                });
    }
}