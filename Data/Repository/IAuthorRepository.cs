using BookInventory.Models;

namespace BookInventory.Data.Repository
{
    public interface IAuthorRepository : IRepository<Author>
    {
        bool Update(Author author);
    }
}
