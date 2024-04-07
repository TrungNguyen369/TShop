using TShop.Models;
using TShop.ViewModels;

namespace TShop.IServices
{
    public interface IProductService
    {
        List<Product> GetAllProducts();
        List<Product> GetNewProducts();
        List<Product> GetProductsByIdCategory(int IdCategory);
        List<Product> GetProductsByIdBrand(int idBrand);
        Product GetProductById(int idProductId);
        List<Product> SearchProductById(string name);
    }
}
