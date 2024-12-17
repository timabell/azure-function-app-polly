using Microsoft.Azure.Functions.Worker.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class Program
{
    public static void Main(string[] args)
    {
        var host = new HostBuilder()
            .ConfigureFunctionsWebApplication()
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                // Register HttpClient
                services.AddHttpClient<HttpTriggerFunction>();

                // Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
                // services.AddApplicationInsightsTelemetryWorkerService();
                // services.ConfigureFunctionsApplicationInsights();
            })
            .Build();

        host.Run();
    }
}