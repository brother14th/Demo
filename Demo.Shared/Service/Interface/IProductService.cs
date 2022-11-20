using System.Threading.Tasks;

namespace Demo.Shared.Service.Interface
{
    public interface IProductService
    {
        Task UpdateStock(string merchantProductNo, int stock);
    }
}