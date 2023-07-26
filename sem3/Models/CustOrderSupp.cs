namespace sem3.Models
{
    public class CustOrderSupp
    {
        public int CustOrderSuppId { get; set; }
        public int CustomerId { get; set; }
        public int SuppDetailId { get; set; }
        public DateTime DeliveryDate { get; set; }
        public float VAT { get; set; }
        public string? Status { get; set; }
        public int NumPeople { get; set; }
        public bool Flag { get; set; }
    }
}
