using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookInventory.Models;
using BookInventory.Services;
using Microsoft.AspNetCore.Authorization;

namespace BookInventory.Controllers
{
    public class ShopsController : Controller
    {
        private readonly IDataService _dataService;
        private readonly IShopService _shopService;

        public ShopsController(IDataService dataService, IShopService shopService)
        {
            _dataService = dataService;
            _shopService = shopService;
        }

        // GET: Shops
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _shopService.GetAllShops());
        }

        // GET: Authors/Details/5
        //[Authorize(Roles = "User,Admin")]
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null) return NotFound();
        //    var author = await _dataService.GetAuthor(id);
        //    if (author == null) return NotFound();
        //    return View(author);
        //}

        // GET: Authors/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Authors/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Shop shop)
        {
            if (ModelState.IsValid)
            {
                await _shopService.AddShop(shop);
                return RedirectToAction(nameof(Index));
            }
            return View(shop);
        }

        // GET: Authors/Edit/5
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null) return NotFound();
        //    var author = await _dataService.GetAuthor(id);
        //    if (author == null) return NotFound();
        //    return View(author);
        //}

        // POST: Authors/Edit/5
        //[Authorize(Roles = "Admin")]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Name,BirthYear,Nationality,IsActive")] Author author)
        //{
        //    if (id != author.Id) return NotFound();

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            await _dataService.UpdateAuthor(author);
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!await _dataService.AuthorExists(author.Id)) return NotFound();
        //            else throw;
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(author);
        //}

        // GET: Authors/Delete/5
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null) return NotFound();
        //    var author = await _dataService.GetAuthor(id);
        //    if (author == null) return NotFound();
        //    return View(author);
        //}

        // POST: Authors/Delete/5
        //[Authorize(Roles = "Admin")]
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    await _dataService.DeactivateAuthor(id);
        //    return RedirectToAction(nameof(Index));
        //}        
    }
}
