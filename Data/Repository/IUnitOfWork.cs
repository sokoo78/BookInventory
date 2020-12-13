using System;
using System.Threading.Tasks;

namespace BookInventory.Data.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IBookRepository Book { get; }
        IAuthorRepository Author { get; }

        Task<bool> Save();
    }
}
