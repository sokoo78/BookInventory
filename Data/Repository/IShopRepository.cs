using BookInventory.Models;
using System.Threading.Tasks;

namespace BookInventory.Data.Repository
{
    public interface IShopRepository : IRepository<Shop>
    {
        Task<bool> UpdateAndSave(Shop shop);        
    }
}
