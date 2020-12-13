using BookInventory.Models;
using System.Threading.Tasks;

namespace BookInventory.Data.Repository
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<bool> UpdateAndSave(Book book);

        Task<bool> ActivateAndSave(int? id);

        Task<bool> Activate(Book book);

        Task<bool> DeactivateAndSave(int? id);

        Task<bool> Deactivate(Book book);
    }
}
