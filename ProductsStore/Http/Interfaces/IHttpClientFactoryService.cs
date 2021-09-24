using System.Threading.Tasks;

namespace ProductsStore.Http.Interfaces
{
    public interface IHttpClientFactoryService
    {
        Task<T> Execute<T>(string url);
    }
}
