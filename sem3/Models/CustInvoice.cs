namespace sem3.Models
{
    public class CustInvoice
    {
        public int InvoiceId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int CusOrderSuppId { get; set; }
        public int CustomerId { get; set; }
        public float VAT { get; set; }
        public string? ListRoom { get; set; }
        public bool Flag { get; set; }
    }
}
