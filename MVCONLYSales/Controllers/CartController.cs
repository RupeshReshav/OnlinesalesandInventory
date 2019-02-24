using MVCONLYSales.Models;
using MVCONLYSales.Models.ViewModel.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCONLYSales.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {
            //initialize cart list
            var cart = Session["Cart"] as List<CartVm> ?? new List<CartVm>();
            //check cart is empty
            if (cart.Count==0 || Session["Cart"]==null)
            {
                ViewBag.Message = "Your cart is empty";
                return View();
            }
            //calculate total and add to the cart.
            decimal total = 0m;
            foreach (var item in cart)
            {
                total += item.Total;
            }
            ViewBag.GrandTotal = total;
            //Return view with model.
            
            return View(cart);
        }
        public ActionResult CartPartial()
        {
            //Initialize cart view model
            CartVm model = new CartVm();
            //Initialize quantity and total price
            int qty = 0;
            decimal price = 0m;
            //check items in session
            if (Session["Cart"]!=null)
            {
                var list = (List<CartVm>)Session["Cart"];
                foreach (var item in list)
                {
                    qty += item.Quantity;
                    price += item.Price;
                }
                model.Quantity = qty;
                model.Price = price;
            }
            else
            {
                model.Quantity = 0;
                model.Price = 0m;
            }
            //return partial view
            return PartialView(model);

        }
        public ActionResult AddToCartPartial(int id)
        {
            //initialize cart list
            var cart = Session["Cart"] as List<CartVm> ?? new List<CartVm>();
            //initialize cartVm
            CartVm model = new CartVm();
            using (NewInventoryEntities db = new NewInventoryEntities())
            {
                //get product detail
                tblProduct product = db.tblProducts.Find(id);
                //check availability in stock.
                int availablequantity =(int) product.Quantity;
                if (availablequantity==0)
                {
                    ViewBag.Message = "Item is not available in stock.";
                    return View("Index","Cart");
                }
                //chack item in cart.
                var productincart = cart.FirstOrDefault(x => x.ProductId == id);
                int orderquantity=1;
                foreach (var item in cart)
                {
                    orderquantity += item.Quantity;
                }
                 
                if (availablequantity< orderquantity)
                {
                    ViewBag.Message = "Insufficient items in stock.";
                    return RedirectToAction("ProductDetails", "Shop");
                }
                else
                {
                    //chack item in cart.
                    //var productincart = cart.FirstOrDefault(x => x.ProductId == id);
                    //if not, add new
                    if (productincart == null)
                    {
                        cart.Add(new CartVm()
                        {
                            ProductId = product.ProductId,
                            ProductName=product.ProductName,
                            Quantity=1,
                            Price=(decimal)product.Price,
                            Image=product.ImageUrl

                        });
                    }
                    else
                    {
                        productincart.Quantity++;

                    }
                }

                //Get total quantity and price and add to model
                int qty = 0;
                decimal price = 0m;
                foreach (var item in cart)
                {
                    qty += item.Quantity;
                    price += item.Quantity * item.Price;
                }
                model.Quantity = qty;
                model.Price = price;
                Session["Cart"] = cart;

                //return redirect
                return PartialView(model);
            }
        }
    }
}