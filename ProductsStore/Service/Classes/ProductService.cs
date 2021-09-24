using Microsoft.Extensions.Configuration;
using ProductsStore.Http.Interfaces;
using ProductsStore.Models;
using ProductsStore.Service.Interfaces;
using Serilog;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProductsStore.Service.Classes
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactoryService _httpClientFactoryService;
        private readonly IConfiguration _configuration;
        private const string ProductsApiUrl = "ProductsApiUrl";

        public ProductService(IHttpClientFactoryService httpClientFactoryService, IConfiguration configuration)
        {
            _httpClientFactoryService = httpClientFactoryService;
            _configuration = configuration;
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            var url = GenerateUrl();
            Log.Information("Get all the products from the store at url {store}", url);
            return await _httpClientFactoryService.Execute<IEnumerable<Product>>(url);
        }

        public async Task<Product> GetProductById(int id)
        {
            var url = GenerateUrl(id);
            Log.Information("Get a product with id {id} from the store at url {store}", id, url);
            return await _httpClientFactoryService.Execute<Product>(url);
        }

        private string GenerateUrl(int? id = null)
        {
            StringBuilder productApiUrl = new StringBuilder();
            productApiUrl.Append(_configuration[ProductsApiUrl]);

            if (id.HasValue)
            {
                productApiUrl.Append($"/{id}");
            }
            return productApiUrl.ToString();
        }
    }
}
