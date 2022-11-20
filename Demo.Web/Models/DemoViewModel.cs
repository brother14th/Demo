using Demo.Shared.Model;

namespace Demo.Web.Models
{
    public class DemoViewModel
    {
        public List<OrderedProduct> TopProducts = new List<OrderedProduct>();
        public String? RetrieveTopProductsMessage { get; set; }
        public String? UpdateTopProductStockMessage { get; set; }
    }
}