﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookInventory.Models;
using BookInventory.Services;
using Microsoft.AspNetCore.Authorization;

namespace BookInventory.Controllers
{
    [Authorize(Policy = "Adult")]
    public class AdultBooksController : Controller
    {        
        private readonly IDataService _dataService;

        public AdultBooksController(IDataService dataService)
        {           
            _dataService = dataService;
        }

        // GET: Books
        public async Task<IActionResult> Index(string author, string title, int? year)
        {
            ViewData["AuthorFilter"] = author;
            if (!string.IsNullOrEmpty(author))
            {
                return View(await _dataService.GetAllBooksByAuthorName(author));
            }

            ViewData["TitleFilter"] = title;
            if (!string.IsNullOrEmpty(title))
            {
                return View(await _dataService.GetAllBooksByTitle(title));
            }

            ViewData["YearFilter"] = year;
            if (year != null)
            {
                return View(await _dataService.GetAllBooksByPublishedYear(year));
            }

            return View(await _dataService.GetAllAdultBooks());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();           
            var book = await _dataService.GetBook(id);
            if (book == null) return NotFound();
            return View(book);
        }

        // GET: Books/Create
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {            
            ViewData["AuthorId"] = new SelectList(await _dataService.GetAllAuthors(), "Id", "Name");
            return View();
        }

        // POST: Books/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,PublishedYear,PageNumber,ISBN,AgeLimit,AuthorId,IsActive")] Book book)
        {
            if (ModelState.IsValid)
            {
                await _dataService.AddBook(book);
                return RedirectToAction(nameof(Index));
            }          
            
            ViewData["AuthorId"] = new SelectList(await _dataService.GetAllAuthors(), "Id", "Name", book.AuthorId);
            return View(book);
        }

        // GET: Books/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();            
            var book = await _dataService.GetBook(id);
            if (book == null) return NotFound();            
            ViewData["AuthorId"] = new SelectList(await _dataService.GetAllAuthors(), "Id", "Name", book.AuthorId);
            return View(book);
        }

        // POST: Books/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,PublishedYear,PageNumber,ISBN,AgeLimit,AuthorId,IsActive")] Book book)
        {
            if (id != book.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {                    
                    await _dataService.UpdateBook(book);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _dataService.BookExists(id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["AuthorId"] = new SelectList(await _dataService.GetAllAuthors(), "Id", "Name", book.AuthorId);
            return View(book);
        }

        // GET: Books/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();                      
            var book = await _dataService.GetBook(id);
            if (book == null) return NotFound();
            return View(book);
        }

        // POST: Books/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {            
            await _dataService.DeactivateBook(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
