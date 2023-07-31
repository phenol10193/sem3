namespace sem3.Models
{
    public class Menu
    {
        public int MenuItemId { get; set; }
        public string ItemName { get; set; }
        public double Price { get; set; }
        public int SupplierId { get; set; }
     
        public string? UrlImage { get; set; }
        public int CategoryId { get; set; }
        public string? Name { get; set; }
    }

}
