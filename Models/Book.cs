using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookInventory.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }        
        public int PublishedYear { get; set; }
        public int PageNumber { get; set; }
        public string ISBN { get; set; }
        public int AgeLimit { get; set; }

        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }
}
