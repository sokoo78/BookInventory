using System.Threading.Tasks;

namespace BookInventory.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Book = new BookRepository(_db);
            Author = new AuthorRepository(_db);
            Shop = new ShopRepository(_db);
            ShopBook = new ShopBookRepository(_db);
        }

        public IBookRepository Book { get; private set; }
        public IAuthorRepository Author { get; private set; }
        public IShopRepository Shop { get; private set; }
        public IShopBookRepository ShopBook { get; private set; }

        public async Task<bool> Save()
        {
            var result = await _db.SaveChangesAsync();
            return result > 0;
        }
        
        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
