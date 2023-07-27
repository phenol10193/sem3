using System.ComponentModel.DataAnnotations;

namespace sem3.Models
{
    public class Acity
    {
        public int AcityId { get; set; }
        [Required]
        public string? CityName { get; set; }
        [Required]
        public int ParentId { get; set; }
        [Required]
        public bool Flag { get; set; }

    }
}
