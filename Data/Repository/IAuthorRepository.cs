using BookInventory.Models;
using System.Threading.Tasks;

namespace BookInventory.Data.Repository
{
    public interface IAuthorRepository : IRepository<Author>
    {        
        Task<bool> UpdateAndSave(Author author);

        Task<bool> ActivateAndSave(int? id);

        Task<bool> ActivateAndSave(Author author);

        Task<bool> DeactivateAndSave(int? id);

        Task<bool> DeactivateAndSave(Author author);
    }
}
