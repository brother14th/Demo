using Demo.Shared.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Demo.Shared.ApiClient;
using Demo.Shared.Service.Interface;

namespace Demo.Shared.Service
{
    public class OrderService : IOrderService
    {
        private readonly IChannelEngineApiClient apiClient;

        public OrderService(IChannelEngineApiClient apiClient)
        {
            this.apiClient = apiClient;
        }
        public async Task<List<OrderedProduct>> GetInProgressOrderedProducts()
        {
            var statuses = new List<OrderStatusView>() { OrderStatusView.IN_PROGRESS };
            CollectionOfMerchantOrderResponse inProgressOrderResponse = null;
            List<OrderedProduct> inProgressOrders = null;
            try
            {
                inProgressOrderResponse = await apiClient.OrderGetByFilterAsync(statuses, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);

                if (!inProgressOrderResponse.Success)
                    throw new ApplicationException("Error retrieving orders!");

                inProgressOrders = inProgressOrderResponse.Content.SelectMany(p => p.Lines.Select(q => new OrderedProduct { MerchantProductNo = q.MerchantProductNo, GTIN = q.Gtin, ProductName = q.Description, Quantity = q.Quantity })).OrderBy(p => p.MerchantProductNo).ThenBy(p => p.Quantity).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return inProgressOrders;
        }

    }
}