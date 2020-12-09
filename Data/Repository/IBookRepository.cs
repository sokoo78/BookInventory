using BookInventory.Models;
using System.Threading.Tasks;

namespace BookInventory.Data.Repository
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<bool> Update(Book book);        
    }
}
