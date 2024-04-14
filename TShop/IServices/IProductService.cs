using TShop.Models;
using TShop.ViewModels;

namespace TShop.IServices
{
    public interface IProductService
    {
        /// <summary>
        /// Get all product
        /// </summary>
        /// <returns></returns>
        List<Product> GetAllProducts();

        /// <summary>
        /// Get new product
        /// </summary>
        /// <returns></returns>
        List<Product> GetNewProducts();

        /// <summary>
        /// Get product by category
        /// </summary>
        /// <param name="IdCategory">id of category</param>
        /// <returns></returns>
        List<Product> GetProductsByIdCategory(int IdCategory);

        /// <summary>
        /// Get all product by brand
        /// </summary>
        /// <param name="idBrand"></param>
        /// <returns></returns>
        List<Product> GetProductsByIdBrand(int idBrand);

        /// <summary>
        /// Get product details by id of product
        /// </summary>
        /// <param name="idProductId"></param>
        /// <returns></returns>
        Product GetProductById(int idProductId);

        /// <summary>
        /// Find product by query
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        List<Product> SearchProductById(string name);
    }
}
