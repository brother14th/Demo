using Demo.Shared.Model;
using Demo.Shared.Service;
using Demo.Shared.Service.Interface;
using Demo.Web.Controllers;
using Demo.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Demo.Web.Tests
{
    public class HomeControllerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Index_ReturnsAViewResult_WithAListOfOrderedProductsAsync()
        {
            //Setup 
       
            var myConfiguration = new Dictionary<string, string>
            {
                {"TopKProduct", "5"},
                {"UpdateProductStock", "25"}
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(initialData: myConfiguration)
                .Build();

            var mockOrderService = new Mock<IOrderService>();
            var orderedProducts = new List<OrderedProduct>()
            {
                new OrderedProduct() { GTIN = "1", MerchantProductNo = "001201-XS", ProductName = "T-shirt met lange mouw BASIC petrol: XS", Quantity =1 },
                new OrderedProduct() { GTIN = "2", MerchantProductNo = "001201-S", ProductName = "T-shirt met lange mouw BASIC petrol: S", Quantity =2 },
                new OrderedProduct() { GTIN = "3", MerchantProductNo = "001201-M", ProductName = "T-shirt met lange mouw BASIC petrol: M", Quantity =3 },
                new OrderedProduct() { GTIN = "4", MerchantProductNo = "001201-L", ProductName = "T-shirt met lange mouw BASIC petrol: L", Quantity =4 },
                new OrderedProduct() { GTIN = "5", MerchantProductNo = "001201-XL", ProductName = "T-shirt met lange mouw BASIC petrol: XL", Quantity =5 },
                new OrderedProduct() { GTIN = "6", MerchantProductNo = "001201-XXL", ProductName = "T-shirt met lange mouw BASIC petrol: XXL", Quantity =6 }

            };
            mockOrderService.Setup(p => p.GetInProgressOrderedProducts()).ReturnsAsync(orderedProducts);

            var mockProductService = new Mock<IProductService>();
            mockProductService.Setup(p => p.UpdateStock(It.IsAny<string>(), It.IsAny<int>()));

            var controller = new HomeController(null, configuration, mockOrderService.Object, new TopProductService(), mockProductService.Object);
            
            //Invoke
            var result = await controller.Index() as ViewResult;
            var topProducts = (result.Model as DemoViewModel).TopProducts;

            //Assert
            var expectedTopProducts = new List<OrderedProduct>()
            {
                new OrderedProduct() { GTIN = "2", MerchantProductNo = "001201-S", ProductName = "T-shirt met lange mouw BASIC petrol: S", Quantity =2 },
                new OrderedProduct() { GTIN = "3", MerchantProductNo = "001201-M", ProductName = "T-shirt met lange mouw BASIC petrol: M", Quantity =3 },
                new OrderedProduct() { GTIN = "4", MerchantProductNo = "001201-L", ProductName = "T-shirt met lange mouw BASIC petrol: L", Quantity =4 },
                new OrderedProduct() { GTIN = "5", MerchantProductNo = "001201-XL", ProductName = "T-shirt met lange mouw BASIC petrol: XL", Quantity =5 },
                new OrderedProduct() { GTIN = "6", MerchantProductNo = "001201-XXL", ProductName = "T-shirt met lange mouw BASIC petrol: XXL", Quantity =6 }
            }.OrderByDescending(p => p.Quantity).ThenBy(p => p.MerchantProductNo);

            Assert.That(JsonConvert.SerializeObject(topProducts), Is.EqualTo(JsonConvert.SerializeObject(expectedTopProducts)));
        }
    }
}