using System.Threading.Tasks;
using BookInventory.Models;
using BookInventory.Data.Repository;
using System.Collections.Generic;

namespace BookInventory.Services
{
    public class ShopService : IShopService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShopService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Shop>> GetAllShops()
        {
            return await _unitOfWork.Shop.GetAll();
        }

        public async Task<ShopBook> GetShopBook(int? id)
        {
            return await _unitOfWork.ShopBook.Get(id);
        }

        public async Task<ShopBook> GetShopBook(int? shopId, int? bookId)
        {
            return await _unitOfWork.ShopBook.GetFirstOrDefault(s => s.ShopId == shopId && s.BookId == bookId);
        }

        public async Task<ShopBook> GetShopBook(string shopName, string bookTitle)
        {
            var shop = await _unitOfWork.Shop.GetFirstOrDefault(s => s.Name == shopName);
            var book = await _unitOfWork.Book.GetFirstOrDefault(b => b.Title == bookTitle);
            return await _unitOfWork.ShopBook.GetFirstOrDefault(s => s.ShopId == shop.Id && s.BookId == book.Id);
        }

        public async Task<bool> AddShop(Shop shop)
        {
            _unitOfWork.Shop.Add(shop);
            return await _unitOfWork.Save();
        }

        public async Task<IEnumerable<ShopBookViewModel>> GetAllShopBooks()
        {
            var shopBooks = await _unitOfWork.ShopBook.GetAll();
            if (shopBooks == null) return null;
           
            var shopBookViewModel = new List<ShopBookViewModel>();
            foreach (var shopBookItem in shopBooks)
            {
                var shopBook = new ShopBookViewModel()
                {
                    ShopId = (await _unitOfWork.Shop.GetFirstOrDefault(s => s.Id == shopBookItem.ShopId)).Id,
                    ShopName = (await _unitOfWork.Shop.GetFirstOrDefault(s => s.Id == shopBookItem.ShopId)).Name,
                    AuthorName = (await _unitOfWork.Book.GetFirstOrDefault(b => b.Id == shopBookItem.BookId, includeProperties: "Author")).Author.Name,
                    BookId = (await _unitOfWork.Book.GetFirstOrDefault(b => b.Id == shopBookItem.BookId)).Id,
                    BookTitle = (await _unitOfWork.Book.GetFirstOrDefault(b => b.Id == shopBookItem.BookId)).Title,
                    StockLevel = shopBookItem.StockLevel
                };
                shopBookViewModel.Add(shopBook);
            }
            return shopBookViewModel;
        }

        public async Task<bool> AddBookToShop(ShopBook shopBook)
        {            
            _unitOfWork.ShopBook.Add(shopBook);
            return await _unitOfWork.Save();
        }

        public async Task<bool> UpdateBookInShop(ShopBook shopBook)
        {            
            return await _unitOfWork.ShopBook.UpdateAndSave(shopBook);
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }       
    }
}
