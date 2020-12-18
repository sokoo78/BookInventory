using BookInventory.Models;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace BookInventory.Services
{
    public interface IShopService : IDisposable
    {
        public Task<IEnumerable<Shop>> GetAllShops();
        public Task<bool> AddShop(Shop shop);

        public Task<ShopBook> GetShopBook(int? i);
        public Task<ShopBook> GetShopBook(int? shopId, int? bookId);
        public Task<ShopBook> GetShopBook(string shopName, string bookTitle);
        public Task<IEnumerable<ShopBookViewModel>> GetAllShopBooks();
        public Task<bool> AddBookToShop(ShopBook shopBook);
        public Task<bool> UpdateBookInShop(ShopBook shopBook);        
    }
}
