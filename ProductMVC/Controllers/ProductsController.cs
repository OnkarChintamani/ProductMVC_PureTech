using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductMVC.Models;
using ProductMVC.Service;

namespace ProductMVC.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db;
        ProductDAL dal;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment env;
        // GET: ProductsController
        public ProductsController(ApplicationDbContext db, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            this.db = db;
            dal = new ProductDAL(this.db);
            this.env = env; 

        }
        public ActionResult Index()
        {
            var model = dal.GetProducts();
            return View(model);
        }

        // GET: ProductsController/Details/5
        public ActionResult Details(int id)
        {
            var model = dal.GetProductById(id);
            return View(model);
        }

        // GET: ProductsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product,IFormFile file)
        {
            try
            {
                using (var fs = new FileStream(env.WebRootPath + "\\Images\\" + file.FileName, FileMode.Create, FileAccess.Write))
                {
                    file.CopyTo(fs);
                }

                product.ImageFileName = "~/Images/" + file.FileName;
                int result = dal.AddProduct(product);
                if (result >= 1)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.ErrorMsg = "Something went wrong";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message;
                return View();
            }
        }

        // GET: ProductsController/Edit/5
        public ActionResult Edit(int id)
        {
            var model = dal.GetProductById(id);
            HttpContext.Session.SetString("oldImageUrl", model.ImageFileName);
            return View(model);
        }

        // POST: ProductsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product, IFormFile file)
        {
            try
            {
                string oldimageurl = HttpContext.Session.GetString("oldImageUrl");
                if (file != null)
                {
                    using (var fs = new FileStream(env.WebRootPath + "\\Images\\" + file.FileName, FileMode.Create, FileAccess.Write))
                    {
                        file.CopyTo(fs);
                    }
                    product.ImageFileName = "~/Images/" + file.FileName;

                    string[] str = oldimageurl.Split("/");
                    string str1 = (str[str.Length - 1]);
                    string path = env.WebRootPath + "\\images\\" + str1;
                    System.IO.File.Delete(path);
                }
                else
                {
                    product.ImageFileName = oldimageurl;
                }
                int result = dal.EditProduct(product);
                if (result >= 1)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.ErrorMsg = "Something went wrong";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message;
                return View();
            }
        }

        // GET: ProductsController/Delete/5
        public ActionResult Delete(int id)
        {
            var model = dal.GetProductById(id);
            return View(model);
        }

        // POST: ProductsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        {
            try
            {
                int result = dal.DeleteProduct(id);
                if (result >= 1)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.ErrorMsg = "Something went wrong";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message;
                return View();
            }
        
        }
    }
}
