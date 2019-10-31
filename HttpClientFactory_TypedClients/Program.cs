using HttpClientFactory_TypedClients.Clients;
using HttpClientFactory_TypedClients.Interfaces;
using HttpClientFactory_TypedClients.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;

namespace HttpClientFactory_TypedClients
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Working with HttpClient Named Client...");

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
                    .AddJsonFile("appsettings.json", optional: false);

        private static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {

            var settings = hostContext.Configuration.GetSection(nameof(GitHubClientSettings)).Get<GitHubClientSettings>();

            services.AddHttpClient();
            services.AddHttpClient(settings.ClientName, c =>
            {
                c.BaseAddress = new Uri(settings.BaseAddress);
                foreach (var header in settings.Headers)
                {
                    c.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            });

            services.AddSingleton<GitHubClient>();

            services.AddHostedService<MyHttpClientService>();
        }
    }

    public class GitHubClientSettings
    {
        public string ClientName { get; set; }
        public string BaseAddress { get; set; }
        public Dictionary<string, string> Headers { get; set; }
    }
}
