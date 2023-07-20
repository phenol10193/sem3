﻿using System.ComponentModel.DataAnnotations;

namespace sem3.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? MiddleName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required(ErrorMessage = "Birth Date is required.")]
        public DateTime BirthDate { get; set; }
        [Required(ErrorMessage = "Gender is required.")]
        public string? Gender { get; set; }
        [Required(ErrorMessage = "Phone Number is required.")]
        public string? PhoneNumber { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Address is required.")]
        public string? Address { get; set; }
        public string? TypeCustomer { get; set; }
        public string? CLoginName { get; set; }
        public string? Password { get; set; }
        public bool Flag { get; set; }
    }
}
