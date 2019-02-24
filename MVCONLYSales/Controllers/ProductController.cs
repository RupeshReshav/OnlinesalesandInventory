using MVCONLYSales.Models;
using MVCONLYSales.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCONLYSales.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult ProductIndex()
        {
            NewInventoryEntities db = new NewInventoryEntities();
            ViewBag.Supplier = new SelectList(db.AspNetUsers, "Id", "UserName");
            return View();
        }
        [HttpPost]
        public ActionResult AddProduct(Product model)
        {
            using (NewInventoryEntities db=new NewInventoryEntities())
            {
                tblProduct product = new tblProduct();
                product.ProductName = model.ProductName;
                product.Quantity = model.Quantity;
                product.Price = model.Price;
                product.Price = model.Price;
                product.ImageUrl = model.ImageUrl;
                product.SupplierId = model.SupplierId;
                product.CategoryId = model.CategoryId;
                product.Description = model.Description;
                db.tblProducts.Add(product);
                db.SaveChanges();
            }
            return Content("Product has been added");

        }
    }
}