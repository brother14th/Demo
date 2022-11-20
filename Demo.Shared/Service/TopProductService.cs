using Demo.Shared.Model;
using Demo.Shared.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Demo.Shared.Service
{
    public class TopProductService 
    {
        public List<OrderedProduct> GetTopProducts(int topK, List<OrderedProduct> orders)
        {

            var TopProducts = (from order in orders
                               group order by order.MerchantProductNo into g
                               select new OrderedProduct()
                               {
                                   GTIN = g.FirstOrDefault().GTIN ?? "",
                                   MerchantProductNo = g.FirstOrDefault().MerchantProductNo ?? "",
                                   ProductName = g.FirstOrDefault().ProductName ?? "",
                                   Quantity = g.Sum(p => p.Quantity)
                               }).ToList().OrderByDescending(p => p.Quantity).ThenBy(p => p.MerchantProductNo).Take(topK);

            return TopProducts.ToList();
        }
    }
}