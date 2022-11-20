using Demo.Shared.ApiClient;
using Demo.Shared.Service.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.Shared.Service
{
    public class ProductService : IProductService
    {
        private readonly IChannelEngineApiClient apiClient;

        public ProductService(IChannelEngineApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        public async Task UpdateStock(string merchantProductNo, int stock)
        {
            SingleOfProductCreationResult response;

            try
            {
                response = await apiClient.ProductPatchAsync(merchantProductNo, new List<Operation>() { new Operation() { Op = "Replace", Path = "Stock", Value = stock } });
                if (!response.Success)
                    throw new ApplicationException("Error updating quantity!");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}