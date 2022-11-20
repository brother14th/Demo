
using Demo.Shared.ApiClient;
using Demo.Shared.Model;
using Demo.Shared.Service;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Shared.Tests
{
    public class ProductServiceTests
    {
        private ProductService productService;
        
        [SetUp]
        public void Setup()
        {
            var apiClientMock = new Mock<IChannelEngineApiClient>();
            
            var mockSuccessResponse = new SingleOfProductCreationResult() { Success = true };
            apiClientMock.Setup(p => p.ProductPatchAsync("123", It.Is<List<Operation>>(p => p.Count == 1 && p[0].Op == "Replace" && p[0].Path == "Stock" && (int)p[0].Value == 25))).ReturnsAsync(mockSuccessResponse);

            var mockFailedResponse = new SingleOfProductCreationResult() { Success = false };
            apiClientMock.Setup(p => p.ProductPatchAsync("456", It.Is<List<Operation>>(p => p.Count == 1 && p[0].Op == "Replace" && p[0].Path == "Stock" && (int)p[0].Value == 30))).ReturnsAsync(mockFailedResponse);

            productService = new ProductService(apiClientMock.Object);
        }

        [Test]
        public void UpdateProductQuantiy_ShouldNotThrowException()
        {
            Assert.DoesNotThrowAsync(async () => await productService.UpdateStock("123", 25));
        }

        [Test]
        public void UpdateProductQuantiy_ShouldThrowException()
        {
            var ex = Assert.ThrowsAsync<ApplicationException>(async () => await productService.UpdateStock("456", 30));
            Assert.That(ex.Message, Is.EqualTo("Error updating quantity!"));
        }
    }
}