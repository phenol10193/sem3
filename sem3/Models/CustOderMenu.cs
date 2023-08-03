using System.ComponentModel.DataAnnotations;

namespace sem3.Models
{
    public class CustOderMenu
    {
        public int CustOderMenuId { get; set; }
        [Required]
        public int? MenuItemId { get; set; }
        [Required]
        public int? RoomId { get; set; }
        [Required]
        public int? CustOderSuppId { get; set; }

        public bool? Flag { get; set; }

        public CustOderSupp? CustOderSupp { get; set; }

        public SuppMenu? SuppMenu { get; set; }

        public Room? Room { get; set; }
       
    }
}
