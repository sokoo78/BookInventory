using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BookInventory.Models;
using BookInventory.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace BookInventory.Controllers
{
    [Authorize]
    public class StocksController : Controller
    {
        private readonly IDataService _dataService;
        private readonly IShopService _shopService;

        public StocksController(IDataService dataService, IShopService shopService)
        {           
            _dataService = dataService;
            _shopService = shopService;
        }

        // GET: Shops
        public async Task<IActionResult> Index()
        {
            var shopBooks = await _shopService.GetAllShopBooks();
            if (shopBooks == null)
            {
                return View(new ShopBookViewModel());
            }            
            return View(await _shopService.GetAllShopBooks());
        }

        // GET: Shops/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null) return NotFound();           
        //    //var shop = await _shopService.GetShop(id);
        //    //if (shop == null) return NotFound();
        //    //return View(shop);
        //}

        // GET: Shops/Create
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            ViewData["ShopId"] = new SelectList(await _shopService.GetAllShops(), "Id", "Name");
            ViewData["BookId"] = new SelectList(await _dataService.GetAllBooks(), "Id", "Title");
            return View();
        }

        // POST: Shops/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ShopId,BookId,StockLevel")] ShopBook shopBook)
        {
            if (ModelState.IsValid)
            {
                await _shopService.AddBookToShop(shopBook);
                return RedirectToAction(nameof(Index));
            }

            ViewData["ShopId"] = new SelectList(await _shopService.GetAllShops(), "Id", "Name", shopBook.ShopId);
            ViewData["BookId"] = new SelectList(await _dataService.GetAllBooks(), "Id", "Title", shopBook.BookId);
            return View(shopBook);
        }

        // GET: Shops/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? shopId, int? bookId)
        {
            if (shopId == null || bookId == null) return NotFound();
            var shopBook = await _shopService.GetShopBook(shopId, bookId);
            if (shopBook == null) return NotFound();

            ViewData["ShopId"] = new SelectList(await _shopService.GetAllShops(), "Id", "Name", shopBook.ShopId);
            ViewData["BookId"] = new SelectList(await _dataService.GetAllBooks(), "Id", "Title", shopBook.BookId);            
            return View(shopBook);
        }

        // POST: Shops/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? shopId, int? bookId, [Bind("ShopId,BookId,StockLevel")] ShopBook shopBook)
        {
            if (shopBook.ShopId != shopId || shopBook.BookId != bookId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _shopService.UpdateBookInShop(shopBook);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (shopBook == null) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["ShopId"] = new SelectList(await _shopService.GetAllShops(), "Id", "Name", shopBook.ShopId);
            ViewData["BookId"] = new SelectList(await _dataService.GetAllBooks(), "Id", "Title", shopBook.BookId);
            return View(shopBook);
        }

        //// GET: Shops/Delete/5
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null) return NotFound();                      
        //    var book = await _shopService.GetBook(id);
        //    if (book == null) return NotFound();
        //    return View(book);
        //}

        //// POST: Shops/Delete/5
        //[Authorize(Roles = "Admin")]
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{            
        //    await _shopService.DeactivateBook(id);
        //    return RedirectToAction(nameof(Index));
        //}
    }
}
