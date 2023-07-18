namespace sem3.Models
{
    public class SuppMenu
    {
        public int MenuItemId { get; set; }
        public string? ItemName { get; set; }
        public float Price { get; set; }
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }
        public bool Flag {get; set; }
    }
}
