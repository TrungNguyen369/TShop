using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using TShop.IServices;
using TShop.Models;
using TShop.Services;
using X.PagedList;

namespace TShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly TshopContext _context;
        private readonly IProductService _productService;

        public ProductController(TshopContext context, IProductService productService)
        {
            _context = context;
            _productService = productService;
        }

        public IActionResult Index(int? page, int? categoryId, int? brandId, string? nameQuery)
        {
            //Define size page
            var pageSize = 9;
            var pagenumber = page == null || page < 0 ? 1 : page.Value;

            //Define list data return
            var list = new List<Product>();

            //Get data by Id Catelory or Brand or nameQuery: This is query search
            //Defaut get data all products
            if (categoryId != null)
            {
                list = _productService.GetProductsByIdCategory((int)categoryId);
            }
            else if (brandId != null)
            {
                list = _productService.GetProductsByIdBrand((int)brandId);
            }
            else if (nameQuery != null)
            {
                list = _productService.SearchProductById(nameQuery);

                //return page not found
                if (list == null)
                {
                    return Redirect("/404");
                }
            }
            else
            {
                //Get all product
                list = _productService.GetAllProducts();
            }

            //Set page list
            PagedList<Product> listPage = new PagedList<Product>(list, pagenumber, pageSize);

            return View(listPage);
        }

        public IActionResult ProductDetail(int id)
        {
            //get product by id and relate products
            var product = _productService.GetProductById(id);
            var relateProducts = _productService.GetProductsByIdCategory(product.IdCategory);

            //define tuple return data
            (Product, List<Product>) tuple = (product, relateProducts);

            return View(tuple);
        }

        [Route("/404")]
        public IActionResult PageNotFound()
        {
            return View();
        }
    }
}
