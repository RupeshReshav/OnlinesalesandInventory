using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCONLYSales.Models.ViewModel
{
    public class Product
    {
        
        public int ProductId { get; set; }
        [Required]
        [Display(Name ="Product Name")]
        public string ProductName { get; set; }
        [Display(Name = "Product Description")]
        public string Description { get; set; }
        [Required]
        public Nullable<int> Quantity { get; set; }
        [Required]
        [Display(Name = "Unit Price")]
        public Nullable<decimal> Price { get; set; }
        [Display(Name = "Product Image")]
        public string ImageUrl { get; set; }
        [Display(Name = "Category")]
        public Nullable<int> CategoryId { get; set; }
        [Display(Name = "Supplier Name")]
        public string SupplierId { get; set; }
    }
}