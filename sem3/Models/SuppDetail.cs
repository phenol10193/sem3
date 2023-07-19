namespace sem3.Models
{
    public class SuppDetail
    {
        public int SuppDetailId { get; set; }
        public int SupplierId { get; set; }
        public string? NameDetail { get; set; }
        public int NumPeople { get; set; }
        public float CustomerCost { get; set; }
        public float SupplierCost { get; set; }
        public bool Flag { get; set; }
    }
}
