using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookInventory.Models;
using BookInventory.Data.Repository;
using System.Collections.Generic;
using System;

namespace BookInventory.Controllers
{
    public class BooksController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BooksController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Books
        public async Task<IActionResult> Index(string author, string title, int? year)
        {
            ViewData["AuthorFilter"] = author;
            if (!string.IsNullOrEmpty(author))
            {
                return View(await _unitOfWork.Book.GetAll(filter: b => b.Author.Name.Contains(author),
                    orderBy: x => x.OrderBy(b => b.Title), includeProperties: "Author"));
            }

            ViewData["TitleFilter"] = title;
            if (!string.IsNullOrEmpty(title))
            {
                return View(await _unitOfWork.Book.GetAll(filter: b => b.Title.Contains(title),
                    orderBy: x => x.OrderBy(b => b.Title), includeProperties: "Author"));
            }

            ViewData["YearFilter"] = year;
            if (year != null)
            {
                return View(await _unitOfWork.Book.GetAllByPublishedYear(year));
            }

            return View(await _unitOfWork.Book.GetAll(orderBy: x => x.OrderBy(b => b.Title),
                includeProperties: "Author"));
        }
        
        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();           
            var book = await _unitOfWork.Book.GetFirstOrDefault(filter: b => b.Id == id, includeProperties: "Author");            
            if (book == null) return NotFound();
            return View(book);
        }

        // GET: Books/Create
        public async Task<IActionResult> Create()
        {            
            ViewData["AuthorId"] = new SelectList(await _unitOfWork.Author.GetAll(), "Id", "Name");
            return View();
        }

        // POST: Books/Create        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,PublishedYear,PageNumber,ISBN,AgeLimit,AuthorId")] Book book)
        {
            if (ModelState.IsValid)
            {                
                _unitOfWork.Book.Add(book);
                await _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }          
            
            ViewData["AuthorId"] = new SelectList(await _unitOfWork.Author.GetAll(), "Id", "Name", book.AuthorId);
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();            
            var book = await _unitOfWork.Book.Get(id);
            if (book == null) return NotFound();            
            ViewData["AuthorId"] = new SelectList(await _unitOfWork.Author.GetAll(), "Id", "Name", book.AuthorId);
            return View(book);
        }

        // POST: Books/Edit/5        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,PublishedYear,PageNumber,ISBN,AgeLimit,AuthorId")] Book book)
        {
            if (id != book.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {                    
                    await _unitOfWork.Book.Update(book);
                    await _unitOfWork.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _unitOfWork.Book.Exists(id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["AuthorId"] = new SelectList(await _unitOfWork.Author.GetAll(), "Id", "Name", book.AuthorId);
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();                      
            var book = await _unitOfWork.Book.GetFirstOrDefault(filter: m => m.Id == id, includeProperties: "Author");
            if (book == null) return NotFound();
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {            
            var book = await _unitOfWork.Book.Get(id);
            _unitOfWork.Book.Remove(book);
            await _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
    }
}
