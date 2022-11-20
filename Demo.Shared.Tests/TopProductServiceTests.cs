using Demo.Shared.ApiClient;
using Demo.Shared.Model;
using Demo.Shared.Service;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Shared.Tests
{
    public class TopProductServiceTests
    {
        private int topK = 5;
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void GetTop5Products_ShouldReturn5Products()
        {
            List<OrderedProduct> orders = new List<OrderedProduct>()
            {
                new OrderedProduct() { GTIN = "1", MerchantProductNo = "001201-S", ProductName = "T-shirt met lange mouw BASIC petrol: S", Quantity =10 },
                new OrderedProduct() { GTIN = "1", MerchantProductNo = "001201-S", ProductName = "T-shirt met lange mouw BASIC petrol: S", Quantity =11 },
                new OrderedProduct() { GTIN = "2", MerchantProductNo = "001201-M", ProductName = "T-shirt met lange mouw BASIC petrol: M", Quantity =12 },
                new OrderedProduct() { GTIN = "2", MerchantProductNo = "001201-M", ProductName = "T-shirt met lange mouw BASIC petrol: M", Quantity =13 },
                new OrderedProduct() { GTIN = "3", MerchantProductNo = "001201-L", ProductName = "T-shirt met lange mouw BASIC petrol: L", Quantity =14 },
                new OrderedProduct() { GTIN = "4", MerchantProductNo = "001201-XL", ProductName = "T-shirt met lange mouw BASIC petrol: XL", Quantity =15 },
                new OrderedProduct() { GTIN = "5", MerchantProductNo = "001201-XXL", ProductName = "T-shirt met lange mouw BASIC petrol: XXL", Quantity =15 },
                new OrderedProduct() { GTIN = "6", MerchantProductNo = "001201-XXXL", ProductName = "T-shirt met lange mouw BASIC petrol: XXXL", Quantity =16 }
            };
            var expectedTop5Products = new List<OrderedProduct>()
            {
                new OrderedProduct() { GTIN = "2", MerchantProductNo = "001201-M", ProductName = "T-shirt met lange mouw BASIC petrol: M", Quantity = 25 },
                new OrderedProduct() { GTIN = "1", MerchantProductNo = "001201-S", ProductName = "T-shirt met lange mouw BASIC petrol: S", Quantity = 21 },
                new OrderedProduct() { GTIN = "6",  MerchantProductNo = "001201-XXXL",ProductName = "T-shirt met lange mouw BASIC petrol: XXXL", Quantity = 16 },
                new OrderedProduct() { GTIN = "4",  MerchantProductNo = "001201-XL",ProductName = "T-shirt met lange mouw BASIC petrol: XL", Quantity = 15 },
                new OrderedProduct() { GTIN = "5", MerchantProductNo = "001201-XXL", ProductName = "T-shirt met lange mouw BASIC petrol: XXL", Quantity = 15 }
            }.OrderByDescending(p => p.Quantity).ThenBy(p => p.MerchantProductNo);

            TopProductService productService = new TopProductService();
            var actualTop5Products = productService.GetTopProducts(topK, orders);
            Assert.AreEqual(JsonConvert.SerializeObject(expectedTop5Products), JsonConvert.SerializeObject(actualTop5Products));  
        }

        [Test]
        public void GetTop5Products_ShouldReturn3Products()
        {
            List<OrderedProduct> orders = new List<OrderedProduct>()
            {
                new OrderedProduct() { GTIN = "1", MerchantProductNo = "001201-S", ProductName = "T-shirt met lange mouw BASIC petrol: S", Quantity =10 },
                new OrderedProduct() { GTIN = "2", MerchantProductNo = "001201-M", ProductName = "T-shirt met lange mouw BASIC petrol: M", Quantity =11 },
                new OrderedProduct() { GTIN = "3", MerchantProductNo = "001201-L", ProductName = "T-shirt met lange mouw BASIC petrol: L", Quantity =12 }
            };
            var expectedTop5Products = new List<OrderedProduct>()
            {
                new OrderedProduct() { GTIN = "3",  MerchantProductNo = "001201-L",ProductName = "T-shirt met lange mouw BASIC petrol: L", Quantity = 12 },
                new OrderedProduct() { GTIN = "2", MerchantProductNo = "001201-M", ProductName = "T-shirt met lange mouw BASIC petrol: M", Quantity = 11 },
                new OrderedProduct() { GTIN = "1", MerchantProductNo = "001201-S", ProductName = "T-shirt met lange mouw BASIC petrol: S", Quantity = 10 }
            }.OrderByDescending(p => p.Quantity).ThenBy(p => p.MerchantProductNo);

            TopProductService productService = new TopProductService();
            var actualTop5Products = productService.GetTopProducts(topK, orders);
            Assert.AreEqual(JsonConvert.SerializeObject(expectedTop5Products), JsonConvert.SerializeObject(actualTop5Products));
        }

        [Test]
        public void GetTop5Products_ShouldReturnNoProducts()
        {
            List<OrderedProduct> orders = new List<OrderedProduct>();
            var expectedTop5Products = new List<OrderedProduct>();
            TopProductService productService = new TopProductService();
            var actualTop5Products = productService.GetTopProducts(topK, orders);

            Assert.AreEqual(JsonConvert.SerializeObject(expectedTop5Products), JsonConvert.SerializeObject(actualTop5Products));
        }
    }
}