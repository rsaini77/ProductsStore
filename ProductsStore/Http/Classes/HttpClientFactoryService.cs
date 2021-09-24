using Newtonsoft.Json;
using ProductsStore.Http.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProductsStore.Http.Classes
{
    public class HttpClientFactoryService : IHttpClientFactoryService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpClientFactoryService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private async Task<T> GetCompaniesWithHttpClientFactory<T>(string url)
        {
            var httpClient = _httpClientFactory.CreateClient();

            using var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            var stream = response.Content.ReadAsStringAsync().Result;

            var companies = JsonConvert.DeserializeObject<T>(stream);
            return companies;

        }

        public async Task<T> Execute<T>(string url)
        {
            return await GetCompaniesWithHttpClientFactory<T>(url);
        }
    }
}


