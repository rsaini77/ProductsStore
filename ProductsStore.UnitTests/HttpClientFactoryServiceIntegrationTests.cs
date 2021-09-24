using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProductsStore.Http.Classes;
using ProductsStore.Models;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProductsStore.IntegrationTests
{
    [TestClass]
    public class HttpClientFactoryServiceIntegrationTests
    {
        public HttpClientFactoryServiceIntegrationTests()
        {
        }

        [TestMethod]
        public async Task TestGetProductsAsync()
        {
            // Arrange
            var request = new
            {
                Url = "/api/Products",
            };

            var myConfiguration = new Dictionary<string, string>
            {
                {"ProductsApiUrl", "https://fakestoreapi.com/products"},

            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();

            // Act
            var webHostBuilder =
            new WebHostBuilder()
            .UseEnvironment("Test") // You can set the environment you want (development, staging, production)
            .UseStartup<Startup>()
            .UseSerilog().UseConfiguration(configuration); // Startup class of your web app project

            using var server = new TestServer(webHostBuilder);
            using var client = server.CreateClient();
            var response = await client.GetAsync(request.Url);
            // Assert
            response.EnsureSuccessStatusCode();
        }

        [TestMethod]
        public async Task PassingExactUrlReturnsCorrectProduct()
        {
            // Arrange
            var url = "https://fakestoreapi.com/products/1";

            IServiceCollection services = new ServiceCollection(); // [1]
            services.AddHttpClient();

            IHttpClientFactory factory = services
                .BuildServiceProvider()
                .GetRequiredService<IHttpClientFactory>();

            // Act
            var service = new HttpClientFactoryService(factory);
            var result = await service.Execute<Product>(url);

            // Assert that product with id 1 is returned
            Assert.IsTrue(result != null && result.Id == 1);
        }

        [TestMethod]
        public async Task PassingExactUrlReturnsListOfProducts()
        {
            // Arrange
            var url = "https://fakestoreapi.com/products";

            IServiceCollection services = new ServiceCollection(); // [1]
            services.AddHttpClient();

            IHttpClientFactory factory = services
                .BuildServiceProvider()
                .GetRequiredService<IHttpClientFactory>();

            // Act
            var service = new HttpClientFactoryService(factory);
            var result = await service.Execute<IEnumerable<Product>>(url);

            // Assert that list of products are returned
            Assert.IsTrue(result != null && result.Count() > 0);
        }
    }
}
