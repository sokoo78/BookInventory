using BookInventory.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BookInventory.Data.Repository
{
    public class AuthorRepository : Repository<Author>, IAuthorRepository
    {
        private readonly ApplicationDbContext _db;

        public AuthorRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<bool> UpdateAndSave(Author author)
        {
            var dbObject = await _db.Author.FirstOrDefaultAsync(b => b.Id == author.Id);
            if (dbObject == null) return false;

            dbObject.Name = author.Name;
            dbObject.BirthYear = author.BirthYear;
            dbObject.Nationality = author.Nationality;
            dbObject.IsActive = author.IsActive;

            _db.SaveChanges();
            return true;
        }

        public async Task<bool> ActivateAndSave(int? id)
        {
            if (id == null) return false;
            return await ActivateAndSave(_db.Author.Find(id));
        }

        public async Task<bool> ActivateAndSave(Author author)
        {
            if (author == null) return false;
            author.IsActive = true;
            var result = await _db.SaveChangesAsync();
            return  result > 0;
        }

        public async Task<bool> DeactivateAndSave(int? id)
        {
            if (id == null) return false;
            return await DeactivateAndSave(_db.Author.Find(id));
        }

        public async Task<bool> DeactivateAndSave(Author author)
        {
            if (author == null) return false;
            author.IsActive = false;
            var result = await _db.SaveChangesAsync();
            return result > 0;
        }
    }
}
