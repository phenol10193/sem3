namespace sem3.Models
{
    public class SuppInvoice
    {
        public int SuppInvoiceId { get; set; }
        public DateTime SuppInvoiceDate { get; set; }
        public int SupplierId { get; set; }
        public string? ListRoom { get; set; }
        public string? PersonInvoice { get; set; }
        public bool Flag { get; set; }
    }
}
