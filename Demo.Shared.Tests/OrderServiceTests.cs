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
    public class OrderServiceTests
    {
        
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public async Task Get_InProgressOrderedProducts_ShouldReturnResults()
        {
            //Setup
            var apiClientMock = new Mock<IChannelEngineApiClient>();
            var statuses = new List<OrderStatusView>() { OrderStatusView.IN_PROGRESS };
            var inProgressOrdersTestData = File.ReadAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, "Data", "InProgressOrders.json"));
            var inProgressOrdersResponse = JsonConvert.DeserializeObject<CollectionOfMerchantOrderResponse>(inProgressOrdersTestData);
            apiClientMock.Setup(p => p.OrderGetByFilterAsync(statuses, null, null, null, null, null, null, null, null, null, null,
                null, null, null, null, null, null, null, null)).ReturnsAsync(inProgressOrdersResponse);
            var orderService = new OrderService(apiClientMock.Object);
            
            //Invoke
            List<OrderedProduct> inProgressOrderedProducts = await orderService.GetInProgressOrderedProducts();
            var expectedOrderedProducts = new List<OrderedProduct>()
            {
                new OrderedProduct(){ GTIN="8719351029609", MerchantProductNo = "001201-M", ProductName = "T-shirt met lange mouw BASIC petrol: M", Quantity =2},
                new OrderedProduct(){ GTIN="8719351029609", MerchantProductNo = "001201-S", ProductName = "T-shirt met lange mouw BASIC petrol: S", Quantity =1},
                new OrderedProduct(){ GTIN="8719351029609", MerchantProductNo = "001201-XL", ProductName = "T-shirt met lange mouw BASIC petrol: XL", Quantity =3}
            }.OrderBy(p => p.MerchantProductNo).ThenBy(p => p.Quantity);
            
            //Assert
            Assert.AreEqual(JsonConvert.SerializeObject(expectedOrderedProducts), JsonConvert.SerializeObject(inProgressOrderedProducts));
        }
    }
}