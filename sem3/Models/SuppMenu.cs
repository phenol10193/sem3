using System.ComponentModel.DataAnnotations;

namespace sem3.Models
{
    public class SuppMenu
    {
        public int MenuItemId { get; set; }
        [Required]
        public string? ItemName { get; set; }
        [Required]
        public float? Price { get; set; }
        [Required]    
        public int CategoryId { get; set; }
        [Required]
        public int SupplierId { get; set; }
        [Required]
        public string? UrlImage { get; set; }
        public bool Flag { get; set; }
        public  Category? Category { get; set; }

        public  ICollection<CustOderMenu>? CustOderMenus { get; set; } 

        public  Supplier? Supplier { get; set; }
    }
}

