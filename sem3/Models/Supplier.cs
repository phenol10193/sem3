namespace sem3.Models
{
    public class Supplier
    {
        public int SupplierId { get; set; }
        public string?  SName { get; set;}
        public string? PhoneNumber { get; set;}
        public string? Address { get; set;}
        public string? Email { get; set;}
        public string? Slever { get; set;}
        public int ACityId { get; set; }
        public bool Flag { get; set; }
    }
}
