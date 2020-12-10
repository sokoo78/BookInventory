using BookInventory.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<bool> Update(Book book)
        {
            var dbObject = await _db.Book.FirstOrDefaultAsync(b => b.Id == book.Id);
            if (dbObject == null) return false;

            dbObject.Title = book.Title;
            dbObject.PublishedYear = book.PublishedYear;
            dbObject.PageNumber = book.PageNumber;
            dbObject.ISBN = book.ISBN;
            dbObject.AgeLimit = book.AgeLimit;
            dbObject.AuthorId = book.AuthorId;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Book>> GetAllByPublishedYear(int? year)
        {
            if (year == null) return null;
            return await _db.Book.Where(b => b.PublishedYear == year).Include(nameof(Book.Author)).ToListAsync();
        }
    }
}
