using MVCONLYSales.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCONLYSales.Controllers
{
    public class UserController : Controller
    {
        // GET: Signup
        public ActionResult SignUp()
        {
            NewInventoryEntities db = new NewInventoryEntities();
            ViewBag.Role = new SelectList(db.tblRoles, "RoleId", "Role");
            return View();
            
           
            
        }
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public int Login(string username)
        {
            using (NewInventoryEntities db = new NewInventoryEntities())
            {
                AspNetUser user = db.AspNetUsers.SingleOrDefault(x => x.UserName == username);
                int role = user.RoleId;
                string userName = user.UserName;
                Session["UserName"] = username;
                return role;
                //if (role==3)
                //{
                //   //return RedirectToAction("Index", "Dashboard", new { Area = "Admin" });
                //    return RedirectToAction("Index", "Home");
                //}
                //else
                //{
                //    return RedirectToAction("Index", "Home");
                //}
            }
        }
        // GET: GetAllEmployee
        public ActionResult GetAllEMployee()
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "User");
        }
    }
}