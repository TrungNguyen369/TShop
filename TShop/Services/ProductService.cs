using AutoMapper;
using System;
using System.Collections.Generic;
using TShop.IServices;
using TShop.Models;

namespace TShop.Services
{
    public class ProductService : IProductService
    {
        private readonly TshopContext _context;

        public ProductService(TshopContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all product
        /// </summary>
        /// <returns></returns>
        public List<Product> GetAllProducts()
        {
            return _context.Products.OrderBy(x => x.IdCategory).ToList(); ;
        }

        /// <summary>
        /// Get new product
        /// </summary>
        /// <returns></returns>
        public List<Product> GetNewProducts()
        {
            return _context.Products.ToList();
        }

        /// <summary>
        /// Get product by id product
        /// </summary>
        /// <param name="idProductId"></param>
        /// <returns></returns>
        public Product GetProductById(int idProductId)
        {
            return _context.Products.FirstOrDefault(x => x.IdProduct == idProductId);
        }

        /// <summary>
        /// Get product by id brand
        /// </summary>
        /// <param name="IdBrand"></param>
        /// <returns></returns>
        public List<Product> GetProductsByIdBrand(int IdBrand)
        {
            return _context.Products.Where(x => x.IdBrands == IdBrand).ToList();
        }

        /// <summary>
        /// Get product by id category
        /// </summary>
        /// <param name="IdCategory"></param>
        /// <returns></returns>
        public List<Product> GetProductsByIdCategory(int IdCategory)
        {
            return _context.Products.Where(x => x.IdCategory == IdCategory).ToList();
        }

        /// <summary>
        /// Get product by query name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Product> SearchProductById(string name)
        {
            return _context.Products.Where(x => x.NameProduct.Contains(name)).ToList();
        }
    }

}
