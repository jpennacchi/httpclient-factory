using System.Threading.Tasks;

namespace HttpClientFactory_TypedClients.Interfaces
{
    public interface ICommonClient
    {
        Task<string> GetData();
    }
}
