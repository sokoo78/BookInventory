using BookInventory.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BookInventory.Data.Repository
{
    public class ShopBookRepository : Repository<ShopBook>, IShopBookRepository
    {
        private readonly ApplicationDbContext _db;

        public ShopBookRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<bool> UpdateAndSave(ShopBook shopBook)
        {
            var dbObject = await _db.ShopBook.FirstOrDefaultAsync(s => s.ShopId == shopBook.ShopId && s.BookId == shopBook.BookId);
            if (dbObject == null) return false;
           
            dbObject.StockLevel = shopBook.StockLevel;

            await _db.SaveChangesAsync();
            return true;
        }
    }
}
