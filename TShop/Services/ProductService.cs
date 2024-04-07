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
        public List<Product> GetAllProducts()
        {
            return _context.Products.OrderBy(x => x.IdCategory).ToList(); ;
        }

        public List<Product> GetNewProducts()
        {
            return _context.Products.ToList();
        }

        public Product GetProductById(int idProductId)
        {
            return _context.Products.FirstOrDefault(x => x.IdProduct == idProductId);
        }

        public List<Product> GetProductsByIdBrand(int IdBrand)
        {
            return _context.Products.Where(x => x.IdBrands == IdBrand).ToList();
        }

        public List<Product> GetProductsByIdCategory(int IdCategory)
        {
            return _context.Products.Where(x => x.IdCategory == IdCategory).ToList();
        }

        public List<Product> SearchProductById(string name)
        {
            return _context.Products.Where(x => x.NameProduct.Contains(name)).ToList();
        }
    }

}
