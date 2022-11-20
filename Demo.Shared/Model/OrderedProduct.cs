namespace Demo.Shared.Model
{
    public class OrderedProduct
    {
        public OrderedProduct()
        {
        }
        public string MerchantProductNo { get; set; }
        public string GTIN { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }
}