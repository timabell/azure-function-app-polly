using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Extensions.Http;

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
                services.AddHttpClient<HttpTriggerFunction>(client => { client.BaseAddress = new Uri("https://example.org/pollyftw"); })
                    .AddPolicyHandler(GetRetryPolicy());
            })
            .Build();

        host.Run();
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(retryAttempt*2));
    }
}