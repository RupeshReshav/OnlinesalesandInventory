using MVCONLYSales.Models;
using MVCONLYSales.Models.ViewModel.Shop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace MVCONLYSales.Areas.Admin.Controllers
{
    public class ShopController : Controller
    {
        // GET: Admin/Shop/Categories
        public ActionResult Category()
        {
            //declare category model
            List<Category> categorylist;
            //init the list
            using (NewInventoryEntities db= new NewInventoryEntities())
            {
                categorylist = db.tblCategories
                               .ToArray()
                               .OrderBy(x => x.Sorting)
                               .Select(x => new Category(x))
                               .ToList();
                       
            }
            // return redirect

            return View(categorylist);
        }
        [HttpPost]
        public string AddCategory(string catname)
        {
            //declare id
            string id;
            using (NewInventoryEntities db = new NewInventoryEntities())
            {
                //check category is unique
                if (db.tblCategories.Any(x=>x.CategotyName==catname))
                {
                    return "category not accepted";
                }
                //Intit Dto
                tblCategory tblcat = new tblCategory();
                tblcat.CategotyName = catname;
                tblcat.Slug = catname.ToLower();
                tblcat.Sorting = 100;
                //Save Dto
                db.tblCategories.Add(tblcat);
                db.SaveChanges();
                //set the id
                id = tblcat.Id.ToString();
            }
            //return id
            return id;



        }
        [HttpPost]
        public void ReorderCategory(int[] id)
        {
            using (NewInventoryEntities db = new NewInventoryEntities())
            {
                //set Initial count
                int count = 1;
                //Declare categoryDto
                tblCategory tblcat;
                //sorting pages
                foreach (var  catId in id)
                {
                    tblcat = db.tblCategories.Find(catId);
                    tblcat.Sorting = count;
                    db.SaveChanges();
                    count++;
                }
            }
        }

        
        public ActionResult DeleteCategory(int id)
        {
            using (NewInventoryEntities db = new NewInventoryEntities())
            {
                //get the categoryDto
                tblCategory cat = db.tblCategories.Find(id);
                //remove dto
                db.tblCategories.Remove(cat);
                //savechanges
                db.SaveChanges();
            }
            return RedirectToAction("Category", "Shop");
        }
        [HttpPost]
        public string RenameCategory(string catName, int id)
        {
            using (NewInventoryEntities db = new NewInventoryEntities())
            {
                //check category name is unique
                if (db.tblCategories.Any(x=>x.CategotyName==catName))
                
                    return "categorytaken";
                
                //get Dto
                tblCategory cat = db.tblCategories.Find(id);
                //Edit Dto
                cat.CategotyName = catName;
                cat.Slug = catName.ToLower();
                db.SaveChanges();
            }

            return "Ok";
        }

        // GET: Admin/Shop/AddProduct
        public ActionResult AddProduct()
        {
            //initialize model
            Product product = new Product();
            //add category and supplier
            using (NewInventoryEntities db = new NewInventoryEntities())
            {
                product.Categories = new SelectList(db.tblCategories.ToList(),"Id","CategotyName");
                product.Supplier = new SelectList(db.AspNetUsers.Where(x => x.RoleId == 1).ToList(),"Id","Name");
            }
            return View(product);
        }
        // POST: Admin/Shop/AddProduct
        [HttpPost]
       public ActionResult AddProduct(Product model, HttpPostedFileBase file )
        {
            //check model state
            if (!ModelState.IsValid)
            {
                using (NewInventoryEntities db = new NewInventoryEntities())
                {
                    model.Categories = new SelectList(db.tblCategories.ToList(), "Id", "CategotyName");
                    model.Supplier = new SelectList(db.AspNetUsers.Where(x => x.RoleId == 1).ToList(), "Id", "Name");
                    return View(model);
                }
                
            }
            using (NewInventoryEntities db = new NewInventoryEntities())
            {
                //Make sure product name is empty
                if (db.tblProducts.Any(x=>x.ProductName==model.ProductName))
                {
                    model.Categories = new SelectList(db.tblCategories.ToList(), "Id", "CategotyName");
                    model.Supplier = new SelectList(db.AspNetUsers.Where(x => x.RoleId == 1).ToList(), "Id", "Name");
                    ModelState.AddModelError("", "product Name already exist");
                    return View(model);
                }
            }
            int id;
            //Initialize dto and save the value
            using (NewInventoryEntities db = new NewInventoryEntities())
            {
                tblProduct product = new tblProduct();
                product.ProductName = model.ProductName;
                product.Slug = model.ProductName.ToLower();
                product.Quantity = model.Quantity;
                product.Price = model.Price;
                product.SupplierId = model.SupplierId;
                product.CategoryId = model.CategoryId;
                product.Description = model.Description;
                db.tblProducts.Add(product);
                db.SaveChanges();
                id = product.ProductId;
            }
            //set tempdaata message
            TempData["SM"] = "You have added a product.";
            #region Upload Image

            // Create necessary directories
            var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));

            var pathString1 = Path.Combine(originalDirectory.ToString(), "Products");
            var pathString2 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString());
            var pathString3 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Thumbs");
            var pathString4 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery");
            var pathString5 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery\\Thumbs");

            if (!Directory.Exists(pathString1))
                Directory.CreateDirectory(pathString1);

            if (!Directory.Exists(pathString2))
                Directory.CreateDirectory(pathString2);

            if (!Directory.Exists(pathString3))
                Directory.CreateDirectory(pathString3);

            if (!Directory.Exists(pathString4))
                Directory.CreateDirectory(pathString4);

            if (!Directory.Exists(pathString5))
                Directory.CreateDirectory(pathString5);

            // Check if a file was uploaded
            if (file != null && file.ContentLength > 0)
            {
                // Get file extension
                string ext = file.ContentType.ToLower();

                // Verify extension
                if (ext != "image/jpg" &&
                    ext != "image/jpeg" &&
                    ext != "image/pjpeg" &&
                    ext != "image/gif" &&
                    ext != "image/x-png" &&
                    ext != "image/png")
                {
                    using (NewInventoryEntities db = new NewInventoryEntities())
                    {
                        model.Categories = new SelectList(db.tblCategories.ToList(), "Id", "CategotyName");
                        model.Supplier = new SelectList(db.AspNetUsers.Where(x => x.RoleId == 1).ToList(), "Id", "Name");
                        ModelState.AddModelError("", "The image was not uploaded - wrong image extension.");
                        return View(model);
                    }
                }

                // Init image name
                string imageName = file.FileName;

                // Save image name to DTO
                using (NewInventoryEntities db = new NewInventoryEntities())
                {
                    tblProduct dto = db.tblProducts.Find(id);
                   
                    dto.ImageUrl = imageName;

                    db.SaveChanges();
                }

                // Set original and thumb image paths
                var path = string.Format("{0}\\{1}", pathString2, imageName);
                var path2 = string.Format("{0}\\{1}", pathString3, imageName);

                // Save original
                file.SaveAs(path);

                // Create and save thumb
                WebImage img = new WebImage(file.InputStream);
                img.Resize(200, 200);
                img.Save(path2);
            }

            #endregion
            //redirect
            return RedirectToAction("AddProduct", "shop");
        }


        // GET: Admin/Shop/Products
        //NOT USED 
        public ActionResult Products(/*int? page,*/ int? catId)
        {
            // Declare a list of ProductVM
            List<Product> listOfProductVM;

            // Set page number
            //var pageNumber = page ?? 1;

            using (NewInventoryEntities db = new NewInventoryEntities())
            {
                // Init the list
                listOfProductVM = db.tblProducts.ToArray()
                                  .Where(x => x.CategoryId == catId || catId == 0 || catId==null )
                                  .Select(x => new Product(x))
                                  .ToList();

                // Populate categories select list
                ViewBag.Categories = new SelectList(db.tblCategories.ToList(), "Id", "CategotyName");
                ViewBag.Supplier= new SelectList(db.AspNetUsers.Where(x => x.RoleId == 1).ToList(), "Id", "Name");

                // Set selected category
                ViewBag.SelectedCat = catId.ToString();
            }

            // Set pagination
            //var onePageOfProducts = listOfProductVM.ToPagedList(pageNumber, 3);
            //ViewBag.OnePageOfProducts = onePageOfProducts;

            // Return view with list
            return View(listOfProductVM);
        }
        [HttpGet]
        public ActionResult ProductList(string SearchBy, int? CategoryName, int? page)
        {
            // Declare a list of ProductVM
            List<Product> listOfProductVM;
            using (NewInventoryEntities db = new NewInventoryEntities())
            {
                ViewBag.Categories = new SelectList(db.tblCategories.ToList(), "Id", "CategotyName");
                ViewBag.Supplier = new SelectList(db.AspNetUsers.Where(x => x.RoleId == 1).ToList(), "Id", "Name");
                if (SearchBy== "Category")
                {
                   listOfProductVM = db.tblProducts.ToArray()
                                  .Where(x => x.CategoryId == CategoryName || CategoryName == 0 || CategoryName == null)
                                  .Select(x => new Product(x))
                                  .ToList();
                    return View(listOfProductVM.ToPagedList(page ?? 1,3));
                }
                else
                {
                    listOfProductVM = db.tblProducts.ToArray()
                                  //.Where(x => x.CategoryId == catId || catId == 0 || catId == null)
                                  .Select(x => new Product(x))
                                  .ToList();
                    return View(listOfProductVM.ToPagedList(page ?? 1, 3));
                }

                
            }
            
        }

        // GET: Admin/Shop/EditProducts
        [HttpGet]
        public ActionResult EditProduct(int id)
        {
            //declare product model
            Product model;
            using (NewInventoryEntities db = new NewInventoryEntities())
            {
                tblProduct pro = db.tblProducts.Find(id);
                if (pro == null)
                {
                    return Content("Product does not exist");
                }
                //initialize the model
                model = new Product(pro);
                //attach supplier and category list
                model.Categories = new SelectList(db.tblCategories.ToList(), "Id", "CategotyName");
                model.Supplier = new SelectList(db.AspNetUsers.Where(x => x.RoleId == 1).ToList(), "Id", "Name");
                //Get all gallery images
                model.GalleryImage = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallery/Thumbs"))
                    .Select(fn=>Path.GetFileName(fn));
            }
            return View(model);
        }
        [HttpPost]
        // POST: Admin/Shop/EditProducts
        public ActionResult EditProduct(Product model, HttpPostedFileBase file)
        {
            //get Id
            int id = model.ProductId;
            //populate categories, suppllier and image gallery.
            using (NewInventoryEntities db = new NewInventoryEntities())
            {
                model.Categories = new SelectList(db.tblCategories.ToList(), "Id", "CategotyName");
                model.Supplier = new SelectList(db.AspNetUsers.Where(x => x.RoleId == 1).ToList(), "Id", "Name");
                
            }
            model.GalleryImage = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallery/Thumbs"))
                     .Select(fn => Path.GetFileName(fn));
            //check model state
            if (!ModelState.IsValid)
            {
                return View(model);

            }
            //Make sure product name is unique.
            using (NewInventoryEntities db = new NewInventoryEntities())
            {
                //Make sure product name is empty
                
                if (db.tblProducts.Where(x=>x.ProductId!=id).Any(x => x.ProductName == model.ProductName))
                {
                    
                    ModelState.AddModelError("", "product Name already exist");
                    return View(model);
                }
            }
            //update product.
            using (NewInventoryEntities db = new NewInventoryEntities())
            {
                tblProduct product = db.tblProducts.Find(id);
                product.ProductName = model.ProductName;
                product.Slug = model.ProductName.ToLower();
                product.Quantity = model.Quantity;
                product.Price = model.Price;
                product.SupplierId = model.SupplierId;
                product.CategoryId = model.CategoryId;
                product.Description = model.Description;
                
                db.SaveChanges();
                id = product.ProductId;
            }
            //set tempdaata message
            TempData["SM"] = "You have Edited a product.";
            #region Upload Image
            // Check if a file was uploaded
            if (file != null && file.ContentLength > 0)
            {
                // Get file extension
                string ext = file.ContentType.ToLower();

                // Verify extension
                if (ext != "image/jpg" &&
                    ext != "image/jpeg" &&
                    ext != "image/pjpeg" &&
                    ext != "image/gif" &&
                    ext != "image/x-png" &&
                    ext != "image/png")
                {
                    using (NewInventoryEntities db = new NewInventoryEntities())
                    {
                        ModelState.AddModelError("", "The image was not uploaded - wrong image extension.");
                        return View(model);
                    }
                }
                //set upload directory path.
                var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));
                var pathString1 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString());
                var pathString2 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Thumbs");
                
                //Delete file from Directory
                DirectoryInfo di1 = new DirectoryInfo(pathString1);
                DirectoryInfo di2 = new DirectoryInfo(pathString2);
                foreach (FileInfo item1 in di1.GetFiles())
                {
                    item1.Delete();
                }
                foreach (FileInfo item2 in di2.GetFiles())
                {
                    item2.Delete();
                }
                // Init image name
                string imageName = file.FileName;

                // Save image name to DTO
                using (NewInventoryEntities db = new NewInventoryEntities())
                {
                    tblProduct dto = db.tblProducts.Find(id);

                    dto.ImageUrl = imageName;

                    db.SaveChanges();
                }

                // Set original and thumb image paths
                var path = string.Format("{0}\\{1}", pathString1, imageName);
                var path2 = string.Format("{0}\\{1}", pathString2, imageName);

                // Save original
                file.SaveAs(path);

                // Create and save thumb
                WebImage img = new WebImage(file.InputStream);
                img.Resize(200, 200);
                img.Save(path2);
            }
                
            #endregion
            //redirect
            return RedirectToAction("EditProduct", "shop");
        }

        // GET: Admin/Shop/DeleteProducts
        public ActionResult DeleteProduct(int id)
        {

            using (NewInventoryEntities db = new NewInventoryEntities())
            { 
                //delete product from database
                tblProduct product = db.tblProducts.Find(id);
                db.tblProducts.Remove(product);
                db.SaveChanges();
            }
            //delete product folder
            var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));
            var pathString1 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString());
            if (Directory.Exists(pathString1))
            {
                Directory.Delete(pathString1,true);
            }
            return RedirectToAction("ProductList", "Shop");
        }
        // POST: Admin/Shop/SaveGalleryImages
        [HttpPost]
        public void SaveGalleryImages(Product model)
        {
            int id = model.ProductId;        
            // Loop through files
            foreach (string fileName in Request.Files)
            {
                // Init the file
                HttpPostedFileBase file = Request.Files[fileName];

                // Check it's not null
                if (file != null && file.ContentLength > 0)
                {
                    // Set directory paths
                    var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));

                    string pathString1 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery");
                    string pathString2 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery\\Thumbs");

                    // Set image paths
                    var path = string.Format("{0}\\{1}", pathString1, file.FileName);
                    var path2 = string.Format("{0}\\{1}", pathString2, file.FileName);

                    // Save original and thumb

                    file.SaveAs(path);
                    WebImage img = new WebImage(file.InputStream);
                    img.Resize(200, 200);
                    img.Save(path2);
                }

            }

        }

    }
}