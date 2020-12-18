using System;
using System.Threading.Tasks;

namespace BookInventory.Data.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IBookRepository Book { get; }
        IAuthorRepository Author { get; }
        IShopRepository Shop { get; }
        IShopBookRepository ShopBook { get; }

        Task<bool> Save();
    }
}
