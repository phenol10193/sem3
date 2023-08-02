using System.ComponentModel.DataAnnotations;

namespace sem3.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public int ParentId { get; set; }
        public bool? Flag { get; set; }
        public  ICollection<SuppMenu>? SuppMenus { get; set; } 
}
