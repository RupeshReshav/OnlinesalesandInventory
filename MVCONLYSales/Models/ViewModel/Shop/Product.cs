using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCONLYSales.Models.ViewModel.Shop
{
    public class Product
    {
        public Product()
        {

        }
        public Product(tblProduct product)
        {
            ProductId = product.ProductId;
            ProductName = product.ProductName;
            Description = product.Description;
            Quantity = product.Quantity;
            Price = product.Price;
            ImageUrl = product.ImageUrl;
            CategoryId = product.CategoryId;
            SupplierId = product.SupplierId;
            Slug = product.Slug;
        }
        public int ProductId { get; set; }
        [Required]
        [Display(Name ="Product Name")]
        public string ProductName { get; set; }
        [StringLength(1000, MinimumLength =5,ErrorMessage ="Description must be at least of five character")]
        public string Description { get; set; }
        [Required]
        public Nullable<int> Quantity { get; set; }
        [Required]
        public Nullable<decimal> Price { get; set; }
        [Display(Name = "Image")]
        public string ImageUrl { get; set; }
        [Display(Name = "Categories")]
        public Nullable<int> CategoryId { get; set; }
        [Display(Name = "Supplier")]
        public string SupplierId { get; set; }
        public string Slug { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<string> GalleryImage { get; set; }
        public IEnumerable<SelectListItem> Supplier { get; set; }
    }
}