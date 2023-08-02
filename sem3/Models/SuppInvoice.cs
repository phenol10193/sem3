using System.ComponentModel.DataAnnotations;

namespace sem3.Models
{
    public class SuppInvoice
    {
        public int SuppInvoiceId { get; set; }
        [Required]
        public DateTime SuppInvoiceDate { get; set; }
        [Required]
        public int SupplierId { get; set; }
        [Required]
        public string? ListRoom { get; set; }
        [Required]
        public string? PersonInvoice { get; set; }
        public bool Flag { get; set; }
        public  Supplier? Supplier { get; set; }
    }
}
