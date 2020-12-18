namespace BookInventory.Models
{
    public class ShopBookViewModel
    {
        public int ShopId { get; set; }
        public int BookId { get; set; }
        public string ShopName { get; set; }
        public string AuthorName { get; set; }        
        public string BookTitle { get; set; }
        public int StockLevel { get; set; }
    }
}
