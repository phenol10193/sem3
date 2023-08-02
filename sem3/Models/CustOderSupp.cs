using System.ComponentModel.DataAnnotations;

namespace sem3.Models
{
    public class CustOderSupp
    {
        public int CustOderSuppId { get; set; }
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public int SuppDetailId { get; set; }
        [Required]
        public DateTime DeliveryDate { get; set; }
        [Required]
        public float VAT { get; set; }
        [Required]
        public string? Status { get; set; }
        [Required]
        public int NumPeople { get; set; }
        [Required]
        public bool Flag { get; set; }
        public  ICollection<CustInvoice>? CustInvoices { get; set; } 

        public  ICollection<CustOderMenu>? CustOderMenus { get; set; } 

        public  Customer? Customer { get; set; }

        public Room? Room { get; set; }
    }
}
