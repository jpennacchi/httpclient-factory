﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace HttpClientFactory_NamedClients
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
                    .AddJsonFile("appsettings.json", optional: true);

        private static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddHttpClient("githubClient", c =>
            {
                c.BaseAddress = new Uri("https://api.github.com/");
                c.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
                c.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample");
            });

            services.AddHostedService<MyHttpClientService>();
        }
    }
}
