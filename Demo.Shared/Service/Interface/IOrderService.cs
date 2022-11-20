using Demo.Shared.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.Shared.Service.Interface
{
    public interface IOrderService
    {
        Task<List<OrderedProduct>> GetInProgressOrderedProducts();
    }
}