using System.ComponentModel.DataAnnotations;

namespace sem3.Models
{
    public class Service
    { 
        public int ServiceId { get; set; }
        [Required]
        public string? ServiceName { get; set; }
        [Required]
        public string? Description { get; set;}
        [Required]
        public int? SupplierId { get; set;}
        [Required]
        public bool Flag { get; set; }
        public  ICollection<Room>? Rooms { get; set; } 

        public  Supplier? Supplier { get; set; } 

    }
}
