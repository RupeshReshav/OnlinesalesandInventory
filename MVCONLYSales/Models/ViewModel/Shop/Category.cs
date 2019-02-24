using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCONLYSales.Models.ViewModel.Shop
{
    public class Category
    {
        public Category()
        {

        }
        public Category(tblCategory category)
        {
            CategotyName = category.CategotyName;
            Id = category.Id;
            
            Slug = category.Slug;
            Sorting = category.Sorting; 
        }
        public int Id { get; set; }
        public string CategotyName { get; set; }
       
        public string Slug { get; set; }
        public Nullable<int> Sorting { get; set; }
    }
}