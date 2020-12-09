using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BookInventory.Data;
using BookInventory.Models;

namespace BookInventory.Views.Authors
{
    public class IndexModel : PageModel
    {
        private readonly BookInventory.Data.ApplicationDbContext _context;

        public IndexModel(BookInventory.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Author> Author { get;set; }

        public async Task OnGetAsync()
        {
            Author = await _context.Author.ToListAsync();
        }
    }
}
