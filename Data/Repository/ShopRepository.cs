using BookInventory.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BookInventory.Data.Repository
{
    public class ShopRepository : Repository<Shop>, IShopRepository
    {
        private readonly ApplicationDbContext _db;

        public ShopRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<bool> UpdateAndSave(Shop shop)
        {
            var dbObject = await _db.Shop.FirstOrDefaultAsync(s => s.Id == shop.Id);
            if (dbObject == null) return false;

            dbObject.Name = shop.Name;            

            await _db.SaveChangesAsync();
            return true;
        }
    }
}
