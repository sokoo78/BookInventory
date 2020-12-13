using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookInventory.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int BirthYear { get; set; }
        public string Nationality { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Book> Books { get; set; }
    }
}
