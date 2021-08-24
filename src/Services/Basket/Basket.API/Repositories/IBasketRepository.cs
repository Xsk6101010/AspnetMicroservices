using System.Threading.Tasks;
using Basket.API.Entities;

namespace Basket.API.Repositories
{
    public interface IBasketRepository
    {
        Task<ShoppingCart> Get(string name);
        Task<ShoppingCart> Update(ShoppingCart basket);

        Task Delete(string name);
    }
}
