using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookInventory.Models
{
    public class Shop
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public ICollection<ShopBook> ShopBook { get; set; }
    }
}
