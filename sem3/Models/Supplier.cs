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
        public string? UrlImage { get; set; }
        public IFormFile? ImageFile { get; set; }
        public int ACityId { get; set; }
        public string? SLoginName { get; set; }
        public string Password { get; set; }
        public List<Service>? Services { get; set; }
        //public bool Flag { get; set; }
    }
}
