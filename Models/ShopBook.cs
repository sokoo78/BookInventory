namespace BookInventory.Models
{
    public class ShopBook
    {       
        public int ShopId { get; set; }        
        public int BookId { get; set; }

        public virtual Shop Shop { get; set; }
        public virtual Book Book { get; set; }

        public int StockLevel { get; set; }
    }
}
