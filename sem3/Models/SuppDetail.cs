﻿using System.ComponentModel.DataAnnotations;

namespace sem3.Models
{
    public class SuppDetail
    {
        public int SuppDetailId { get; set; }
        [Required]
        public int SupplierId { get; set; }
        [Required]
        public string? NameDetail { get; set; }
        [Required]
        public int NumPeople { get; set; }
        [Required]
        public double CustomerCost { get; set; }
        [Required]
        public double SupplierCost { get; set; }
        public bool Flag { get; set; }
        public  Supplier? Supplier { get; set; }
    }
}
