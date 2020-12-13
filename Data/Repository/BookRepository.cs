using BookInventory.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BookInventory.Data.Repository
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        private readonly ApplicationDbContext _db;

        public BookRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<bool> UpdateAndSave(Book book)
        {
            var dbObject = await _db.Book.FirstOrDefaultAsync(b => b.Id == book.Id);
            if (dbObject == null) return false;

            dbObject.Title = book.Title;
            dbObject.PublishedYear = book.PublishedYear;
            dbObject.PageNumber = book.PageNumber;
            dbObject.ISBN = book.ISBN;
            dbObject.AgeLimit = book.AgeLimit;
            dbObject.AuthorId = book.AuthorId;
            dbObject.IsActive = book.IsActive;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActivateAndSave(int? id)
        {
            if (id == null) return false;
            return await Activate(_db.Book.Find(id));
        }

        public async Task<bool> Activate(Book book)
        {
            if (book == null) return false;
            book.IsActive = true;
            var result = await _db.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeactivateAndSave(int? id)
        {
            if (id == null) return false;
            return await Deactivate(_db.Book.Find(id));
        }

        public async Task<bool> Deactivate(Book book)
        {
            if (book == null) return false;
            book.IsActive = false;
            var result = await _db.SaveChangesAsync();
            return result > 0;
        }       
    }
}
