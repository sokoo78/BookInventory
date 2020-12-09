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
        }

        public IBookRepository Book { get; private set; }
        public IAuthorRepository Author { get; private set; }

        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }
        
        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
