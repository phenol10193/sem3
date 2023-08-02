using System.ComponentModel.DataAnnotations;

namespace sem3.Models
{
    public class CustInvoice
    {
        public int InvoiceId { get; set; }
        [Required]
        public DateTime? InvoiceDate { get; set; }
        [Required]
        public int? CustOderSuppId { get; set; }
        [Required]
        public int? CustomerId { get; set; }
        [Required]
        public int? RoomId { get; set; }
        [Required]
        public double? Vat { get; set; }
        [Required]
        public string? ListRoom { get; set; }
        
        public bool? Flag { get; set; }

        public  CustOderSupp? CustOderSupp { get; set; }

        public  Customer? Customer { get; set; }

        public  Room? Room { get; set; }
    }
}
