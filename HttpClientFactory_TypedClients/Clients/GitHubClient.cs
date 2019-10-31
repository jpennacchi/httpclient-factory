using HttpClientFactory_TypedClients.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpClientFactory_TypedClients.Clients
{
    public class GitHubClient : ICommonClient
    {
        private readonly HttpClient _client;

        public GitHubClient(IHttpClientFactory clientFactory)
        {
            _client = clientFactory.CreateClient("githubClient");
        }

        public async Task<string> GetData() => await _client.GetStringAsync("/");
    }
}
