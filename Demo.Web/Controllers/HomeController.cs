using Demo.Shared.Model;
using Demo.Shared.Service;
using Demo.Shared.Service.Interface;
using Demo.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Demo.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<HomeController> _logger;
        private readonly IOrderService _orderService;
        private readonly TopProductService _topProductService;
        private readonly IProductService _productService;

        public HomeController(ILogger<HomeController> logger,IConfiguration configuration, IOrderService orderService, TopProductService topProductService, IProductService productService)
        {
            _logger = logger;
            _orderService = orderService;
            _topProductService = topProductService;
            _productService = productService;
            _configuration= configuration;
        }

        public async Task<IActionResult> Index()
        {
            var demoViewModel = new DemoViewModel();
            List<OrderedProduct> inProgressOrderedProducts;
            try
            {
                inProgressOrderedProducts = await _orderService.GetInProgressOrderedProducts();
                var topProducts = _topProductService.GetTopProducts(int.Parse(_configuration["TopKProduct"] ?? ""), inProgressOrderedProducts);
                demoViewModel.TopProducts = topProducts;

                var topProduct = topProducts.FirstOrDefault();
                if (topProduct != null)
                {
                    try
                    {
                        int updateProductStock = int.Parse(_configuration["UpdateProductStock"] ?? ""); 
                        await _productService.UpdateStock(topProduct.MerchantProductNo, updateProductStock);
                        demoViewModel.UpdateTopProductStockMessage = $"Success updating stock to {updateProductStock} for {topProduct.ProductName}!";
                    }
                    catch (Exception)
                    {
                        demoViewModel.UpdateTopProductStockMessage = $"Error updating stock for {topProduct.ProductName}!";
                    }
                }
                else
                    demoViewModel.UpdateTopProductStockMessage = "No product to update stock!";
            }
            catch (Exception)
            {
                demoViewModel.RetrieveTopProductsMessage = "Error retrieving top products!";
            }
            return View(demoViewModel);
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