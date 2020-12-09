using BookInventory.Models;
using System.Linq;

namespace BookInventory.Data.Repository
{
    public class AuthorRepository : Repository<Author>, IAuthorRepository
    {
        private readonly ApplicationDbContext _db;

        public AuthorRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public bool Update(Author author)
        {
            var dbObject = _db.Author.FirstOrDefault(b => b.Id == author.Id);
            if (dbObject == null) return false;

            dbObject.Name = author.Name;
            dbObject.BirthYear = author.BirthYear;
            dbObject.Nationality = author.Nationality;

            _db.SaveChanges();
            return true;
        }
    }
}
