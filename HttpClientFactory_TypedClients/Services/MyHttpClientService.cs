using HttpClientFactory_TypedClients.Clients;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HttpClientFactory_TypedClients.Services
{
    public class MyHttpClientService : BackgroundService, IDisposable
    {
        private readonly GitHubClient _gitHubClient;

        public MyHttpClientService(GitHubClient gitHubClient)
        {
            _gitHubClient = gitHubClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var response = await _gitHubClient.GetData();

            Console.WriteLine($"Results: {response}");
        }
    }
}
