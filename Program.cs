using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http.Resilience;
using Polly;

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
                    //.AddStandardResilienceHandler(); // https://learn.microsoft.com/en-us/dotnet/core/resilience/http-resilience
                    .AddResilienceHandler("example-handler", static builder =>
                    {
                        builder.AddRetry(new HttpRetryStrategyOptions
                        {
                            BackoffType = DelayBackoffType.Linear,
                            MaxRetryAttempts = 4,
                            UseJitter = true,
                            Delay = TimeSpan.FromSeconds(2),
                        });
                    });
            })
            .Build();

        host.Run();
    }
}