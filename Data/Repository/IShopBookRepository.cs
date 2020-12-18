using BookInventory.Models;
using System.Threading.Tasks;

namespace BookInventory.Data.Repository
{
    public interface IShopBookRepository : IRepository<ShopBook>
    {
        Task<bool> UpdateAndSave(ShopBook shopBook);
    }
}
