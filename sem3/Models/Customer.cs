using System.ComponentModel.DataAnnotations;

namespace sem3.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        [Required]
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required(ErrorMessage = "Birth Date is required.")]
        public DateTime BirthOfDate { get; set; }
        [Required(ErrorMessage = "Gender is required.")]
        public string? Gender { get; set; }
        [Required(ErrorMessage = "Phone Number is required.")]
        public string? PhoneNumber { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? TypeCustomer { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string? UrlImage { get; set; }
        public string? CLoginName { get; set; }
        public string? Password { get; set; }
        public bool? Flag { get; set; }
        public ICollection<CustInvoice>? CustInvoice { get; set; }
        public ICollection<CustOderSupp>? CustOderSupp { get;set; }
        public ICollection<CustomerFeedback>? CustomerFeedback { get; set; }
    }
}
