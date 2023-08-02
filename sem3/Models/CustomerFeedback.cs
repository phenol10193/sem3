using System.ComponentModel.DataAnnotations;

namespace sem3.Models
{
    public class CustomerFeedback
    {
        public int FeedbackId { get; set; }
        [Required]
        public int SupplierId { get; set; }
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public string? Comment { get; set; }
        [Required]
        public DateTime? FeedbackDate { get; set; }
        public bool Flag { get; set; }
        public  Customer? Customer { get; set; } 

        public  Supplier? Supplier { get; set; }


    }
}
