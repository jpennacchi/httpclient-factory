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
            _client = clientFactory.CreateClient("localhostClient");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var httpResponseMessage = await GetDataFromHttpResponseMessage();

            //httpResponseMessage.EnsureSuccessStatusCode();

            if (httpResponseMessage.Content is object 
                && httpResponseMessage.Content.Headers.ContentType.MediaType == "application/json")
            {
                Console.WriteLine($"Results: {await httpResponseMessage.Content.ReadAsStringAsync()}");
            }
            else
            {
                Console.WriteLine("HTTP Response was invalid and cannot be deserialised.");
            }
        }

        public async Task<HttpResponseMessage> GetDataFromHttpResponseMessage()
            => await _client.GetAsync("/Edenred.Services/InvoiceQueue/Dequeue/ACCMM_AGENT/Rest");

    }
}
