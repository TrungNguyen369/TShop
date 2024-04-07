using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using TShop.IServices;
using TShop.Models;
using TShop.Services;
using TShop.ViewModels;

namespace TShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly IBrandService _brands;

        public HomeController(ILogger<HomeController> logger, IProductService productService, IBrandService brands)
        {
            _logger = logger;
            _productService = productService;
            _brands = brands;
        }

        public IActionResult Index()
        {
            //Get list new product and product hot
            var listAllProduct = _productService.GetAllProducts().Take(5).ToList();
            var listNewProduct = _productService.GetNewProducts().Take(5).ToList();

            //Merge product lists together
            List<List<Product>> listProductVM = new List<List<Product>>
            {
                listAllProduct,
                listNewProduct
            };

            //get list brands
            var brands = _brands.GetAllBrand().Take(7).ToList();

            //Define tuple list product and brands
            (List<List<Product>>, List<Brand>) tuple = (listProductVM, brands);
         
            return View(tuple);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
