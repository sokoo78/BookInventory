using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookInventory.Models;
using BookInventory.Services;

namespace BookInventory.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly IDataService _dataService;

        public AuthorsController(IDataService dataService)
        {
            _dataService = dataService;
        }

        // GET: Authors
        public async Task<IActionResult> Index()
        {
            return View(await _dataService.GetAllAuthors());
        }

        // GET: Authors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var author = await _dataService.GetAuthor(id);
            if (author == null) return NotFound();
            return View(author);
        }

        // GET: Authors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Authors/Create        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,BirthYear,Nationality,IsActive")] Author author)
        {
            if (ModelState.IsValid)
            {
                await _dataService.AddAuthor(author);
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        // GET: Authors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var author = await _dataService.GetAuthor(id);
            if (author == null) return NotFound();
            return View(author);
        }

        // POST: Authors/Edit/5       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,BirthYear,Nationality,IsActive")] Author author)
        {
            if (id != author.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _dataService.UpdateAuthor(author);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _dataService.AuthorExists(author.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        // GET: Authors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var author = await _dataService.GetAuthor(id);
            if (author == null) return NotFound();
            return View(author);
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _dataService.DeactivateAuthor(id);
            return RedirectToAction(nameof(Index));
        }        
    }
}
