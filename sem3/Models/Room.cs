using System.ComponentModel.DataAnnotations;

namespace sem3.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        [Required]
        public string? RoomName { get; set; }
        [Required]
        public int? Capacity { get; set; }
        [Required]
        public float? RoomPrice { get; set; }
        [Required]
        public int? ServiceId { get;set; }
        public bool Flag { get; set; }
    }
}
