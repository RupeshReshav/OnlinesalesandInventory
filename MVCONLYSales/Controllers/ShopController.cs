using MVCONLYSales.Models;
using MVCONLYSales.Models.ViewModel.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCONLYSales.Controllers
{
    public class ShopController : Controller
    {
        // GET: Shop
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CategoryMenuPartial()
        {
            //declare categoty model
            List<Category> model;
            using (NewInventoryEntities db=new NewInventoryEntities())
            {
                model = db.tblCategories
                               .ToArray()
                               .OrderBy(x => x.Sorting)
                               .Select(x => new Category(x))
                               .ToList();
            }
            //return partial view.
            return PartialView(model);
        }
        public ActionResult Category(int id)
        {
            //declare product model
            List<Product> model;
            using (NewInventoryEntities db = new NewInventoryEntities())
            {
                model = db.tblProducts
                            .ToArray()
                            .Where(x => x.CategoryId == id && x.Quantity!=0)
                            .Select(x => new Product(x))
                            .ToList();
                var productCat = db.tblCategories.SingleOrDefault(x => x.Id == id);
                ViewBag.CategoryName = productCat.CategotyName;
            }
            //return redirect
            return View(model);
        }
        // GET: Shop/ProductDetails/id
        [ActionName("product-detail")]
        public ActionResult ProductDetails(int id)
        {
            //Initialize model
            Product model;

            using (NewInventoryEntities db = new NewInventoryEntities())
            {
                tblProduct product = db.tblProducts.Find(id);
                model = new Product(product);
            }
            return View("ProductDetails", model);
        }
    }
}