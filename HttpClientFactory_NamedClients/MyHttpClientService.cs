using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HttpClientFactory_NamedClients
{
    public class MyHttpClientService : BackgroundService, IDisposable
    {
        private readonly HttpClient _client;

        public MyHttpClientService(IHttpClientFactory clientFactory)
        {
            _client = clientFactory.CreateClient("githubClient");
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
