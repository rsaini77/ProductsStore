using ProductsStore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductsStore.Service.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProducts();

        Task<Product> GetProductById(int id);
    }
}
