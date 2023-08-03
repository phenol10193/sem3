namespace sem3.Models
{
    public class Supplier
    {
        public int SupplierId { get; set; }
        public string? SName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? SLevel { get; set; }
        public int ACityId { get; set; }
        public bool Flag { get; set; }
        public string? UrlImage { get; set; } 
        public string? SLogin { get; set; } 
        public string? Password { get; set; }
        public List<Acity>? Acity { get; set; }
        public ICollection<Service>? Service { get; set; }
        public  ICollection<Message>? Messages { get; set; }
        public  ICollection<CustomerFeedback>? CustomerFeedback { get; set; }
        public ICollection<SuppDetail>? SuppDetail { get; set; }
        public ICollection<SuppInvoice>? SuppInvoice { get; set; }
        public ICollection<SuppMenu>? SuppMenu { get; set; }

    }
}
