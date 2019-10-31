using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace HttpClientFactory_Directly
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Working with HttpClient...");

            var host = CreateHostBuilder(args).UseConsoleLifetime().Build();

            host.Run();
        }

        static IHostBuilder CreateHostBuilder(string[] args)
        {
            return new HostBuilder()
                        .ConfigureAppConfiguration(AppConfiguration)
                        .ConfigureServices(ConfigureServices);
        }

        private static void AppConfiguration(HostBuilderContext hostContext, IConfigurationBuilder config)
            => config
                    .SetBasePath(Environment.CurrentDirectory)
                    .AddEnvironmentVariables()
                    .AddJsonFile("appsettings.json", optional: true);

        private static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddHostedService<MyHttpClientService>();
        }
    }

    public class MyHttpClientService : BackgroundService, IDisposable
    {
        private readonly HttpClient _client;

        public MyHttpClientService(IHttpClientFactory clientFactory)
        {
            _client = clientFactory.CreateClient();
            _client.BaseAddress = new Uri("http://api.github.com");
            _client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
            _client.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var response = await GetData();

            Console.WriteLine($"Results: {response}");
        }

        private async Task<string> GetData()
        {
            
            return await _client.GetStringAsync("/");
        }

    }
}
