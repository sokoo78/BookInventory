using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookInventory.Models;
using BookInventory.Data.Repository;

namespace BookInventory.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public AuthorsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Authors
        public async Task<IActionResult> Index()
        {
            return View(await _unitOfWork.Author.GetAll());
        }

        // GET: Authors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var author = await _unitOfWork.Author.GetFirstOrDefault(filter: a => a.Id == id);
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
        public async Task<IActionResult> Create([Bind("Id,Name,BirthYear,Nationality")] Author author)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Author.Add(author);
                await _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        // GET: Authors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var author = await _unitOfWork.Author.Get(id);
            if (author == null) return NotFound();
            return View(author);
        }

        // POST: Authors/Edit/5       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,BirthYear,Nationality")] Author author)
        {
            if (id != author.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.Author.Update(author);
                    await _unitOfWork.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _unitOfWork.Author.Exists(author.Id)) return NotFound();
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
            var author = await _unitOfWork.Author.GetFirstOrDefault(a => a.Id == id);                
            if (author == null) return NotFound();
            return View(author);
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var author = await _unitOfWork.Author.Get(id);
            _unitOfWork.Author.Remove(author);
            await _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }        
    }
}
