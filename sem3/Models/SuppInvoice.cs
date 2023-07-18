namespace sem3.Models
{
    public class SuppInvoice
    {
        public int SupInvoiceId { get; set; }
        public DateTime SupInvoiceDate { get; set; }
        public int SupplierId { get; set;}
        public string? ListRoom { get; set;}
        public string? PersonInvoice { get; set;}
        public bool Flag { get; set; }
    }
}
