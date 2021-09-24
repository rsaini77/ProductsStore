using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using NSubstitute;
using ProductsStore.Http.Classes;
using ProductsStore.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ProductsStore.UnitTests
{
    [TestClass]
    public class HttpClientFactoryServiceUnitTests
    {
        public HttpClientFactoryServiceUnitTests()
        {
        }

        [TestMethod]
        public async Task PassingCorrectUrlReturnsListOfProducts()
        {
            // Arrange
            // Just create 2 products using faker
            var product1 = A.Fake<Product>();
            var product2 = A.Fake<Product>();
            var products = new List<Product>
            {
                product1,
                product2
            };
            //Create mock
            var httpClientFactoryMock = Substitute.For<IHttpClientFactory>();
            var url = "http://good.uri";
            var fakeHttpMessageHandler = new FakeHttpMessageHandler(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(products), Encoding.UTF8, "application/json")
            });
            var fakeHttpClient = new HttpClient(fakeHttpMessageHandler);
            httpClientFactoryMock.CreateClient().Returns(fakeHttpClient);

            // Act
            var service = new HttpClientFactoryService(httpClientFactoryMock);
            var result = await service.Execute<IEnumerable<Product>>(url);
            
            // Assert that 2 products are returned
            Assert.IsTrue(result.Count() == 2);
        }

        [TestMethod]
        public async Task PassingBadUrlReturnsNull()
        {
            // Arrange
            // Create mock
            var httpClientFactoryMock = Substitute.For<IHttpClientFactory>();
            var url = "http://bad.uri";
            var fakeHttpMessageHandler = new FakeHttpMessageHandler(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound
            });
            var fakeHttpClient = new HttpClient(fakeHttpMessageHandler);
            httpClientFactoryMock.CreateClient().Returns(fakeHttpClient);

            // Act
            var service = new HttpClientFactoryService(httpClientFactoryMock);
            // Assert
            // that the service throws an exception
            await Assert.ThrowsExceptionAsync<HttpRequestException>(() => service.Execute<Product>(url));
        }
    }
}
