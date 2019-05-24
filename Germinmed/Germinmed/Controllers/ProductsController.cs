




using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Germinmed.DAL;
using Germinmed.Models;



namespace Germinmed.Controllers
{
    public class ProductsController : Controller
    {
        private GerminmedContext db = new GerminmedContext();
       
        // GET: Products
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewAll()
        {
            return View(GetAllProducts());

        }


        public ActionResult All()
        {
            return View(GetAllProducts());

        }


        IEnumerable<Products> GetAllProducts()
        {
            using (GerminmedContext db = new GerminmedContext())
            {
                Category cat = new Category();
                List<Products> productList = db.Product.ToList<Products>();

                foreach (var item in productList)
                {
                    item.CategoryTitle = db.Category.SqlQuery(Category.sqlQuery).Where(x => x.Id == item.CategoryId).FirstOrDefault<Category>().Title;
                }
                return productList;
            }
            
        }
        

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        public ActionResult AddOrEdit(int id = 0)
        {
            Products prod = new Products();
            if (id != 0)
            {

                Session["ProductId"] = id;
                using (GerminmedContext db = new GerminmedContext())
                {
                    prod = db.Product.Where(x => x.Id == id).FirstOrDefault<Products>();
                    prod.BrandList = db.Brand.ToList<Brands>();
                    prod.BrandList.Add(new Brands { Title = "[None]" });
                    prod.CategoryList = db.Category
                            .SqlQuery(Category.sqlQuery)
                            .ToList<Category>();
                    // prod.CategoryList.Add(new Category { Id = 0, Title = "[None]" });


                }
            }
            else
            {
                using (GerminmedContext db = new GerminmedContext())
                {


                    prod.CategoryList = db.Category
                            .SqlQuery(Category.sqlQuery)
                            .ToList<Category>();
                    prod.CategoryList.Add(new Category { Title = "[None]" });

                    prod.BrandList = db.Brand.ToList<Brands>();
                    prod.BrandList.Add(new Brands { Id = 0, Title = "[None]" });
                }
            }

            ProductImage img = new ProductImage();
            //img.ProductId = id;
            return View(prod);

        }



        [HttpPost]
        public ActionResult AddOrEdit(Products prod)
        {
            try
            {
                if (prod.ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(prod.ImageUpload.FileName);
                    string extension = Path.GetExtension(prod.ImageUpload.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    prod.ImagePath = "~/AppFiles/Images/" + fileName;
                    prod.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/AppFiles/Images/"), fileName));
                }
                if (prod.ImageUpload1 != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(prod.ImageUpload1.FileName);
                    string extension = Path.GetExtension(prod.ImageUpload1.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    prod.InnerBannerImage = "/AppFiles/Images/" + fileName;
                    prod.ImageUpload1.SaveAs(Path.Combine(Server.MapPath("~/AppFiles/Images/"), fileName));
                }
                if (prod.CatalogueUplaoder != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(prod.CatalogueUplaoder.FileName);
                    string extension = Path.GetExtension(prod.CatalogueUplaoder.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    prod.Catalogue = "/AppFiles/docs/" + fileName;
                    prod.CatalogueUplaoder.SaveAs(Path.Combine(Server.MapPath("~/AppFiles/docs/"), fileName));
                }
                using (GerminmedContext db = new GerminmedContext())
                {
                    if (prod.Id == 0)
                    {
                        prod.Description= prod.Description == null ? " " : prod.Description;
                        db.Product.Add(prod);
                        db.SaveChanges();
                    }
                    else
                    {
                        prod.Description = prod.Description == null ? " " : prod.Description;
                        db.Entry(prod).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                return Json(data: new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", model: GetAllProducts()), message = "Submitted Successfully" }, behavior: JsonRequestBehavior.AllowGet);

                //RedirectToAction("ViewAll");
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Delete(int Id)
        {
            try
            {
                using (GerminmedContext db = new GerminmedContext())
                {
                    Products prod = db.Product.Where(x => x.Id == Id).FirstOrDefault<Products>();

                    string fullPath = Request.MapPath(prod.ImagePath);
                    if (prod.ImagePath != "~/AppFiles/Images/Default.png")
                    {
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                    }
                    if (prod.Catalogue != null)
                    {
                        string fullPathCatalog = Request.MapPath(prod.Catalogue);
                        if (System.IO.File.Exists(fullPathCatalog))
                        {
                            System.IO.File.Delete(fullPathCatalog);
                        }
                    }

                    db.Product.Remove(prod);
                    db.SaveChanges();
                }
                return Json(data: new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", model: GetAllProducts()), message = "Deleted Successfully" }, behavior: JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult ViewImagesByProduct(int id = 0)
        {
            if (Session["ProductId"] != null)
                id = (int)Session["ProductId"];
            return View(GetAllImagesByProduct(id));

        }


        IEnumerable<ProductImage> GetAllImagesByProduct(int id)
        {
            using (GerminmedContext db = new GerminmedContext())
            {
                return db.ProductImage.Where(x => x.ProductId == id).ToList<ProductImage>();
            }
        }


        [HttpPost]
        public ActionResult AddImage(ProductImage img)
        {
             
            try
            {
                if (Session["ProductId"] != null)
                    img.ProductId = (int)Session["ProductId"];
                if (img.ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(img.ImageUpload.FileName);
                    string extension = Path.GetExtension(img.ImageUpload.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    img.ImageUrl = "~/AppFiles/Images/" + fileName;
                    //img.ProductId = img.txtProductId;
                    img.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/AppFiles/Images/"), fileName));
                 //   ResizeSettings resizeSetting = new ResizeSettings(560,305,FitMode.Max,"png");
                    //{
                    //    Width = 560,
                    //    Height = 305,
                    //    Format = "png"
                    //};
                  //  ImageBuilder.Current.Build(Path.Combine(Server.MapPath("~/AppFiles/Images/"),fileName), Path.Combine(Server.MapPath("~/AppFiles/Images/"),fileName), resizeSetting);

                }
                using (GerminmedContext db = new GerminmedContext())
                {
                    if (img.Id == 0)
                    {
                        db.ProductImage.Add(img);
                        db.SaveChanges();
                    }
                    else
                    {
                        var currentItem = db.ProductImage.Where(b => b.Id == img.Id).FirstOrDefault<ProductImage>();
                        currentItem.DisplayOrder = img.DisplayOrder ;
                       
                        db.Entry(currentItem).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                }
                return Json(data: new { id = img.ProductId, success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewImagesByProduct", model: GetAllImagesByProduct(img.ProductId)), message = "Submitted Successfully" }, behavior: JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
       

        [HttpGet]
        public ActionResult AddImage(int id = 0,int productid=0)
        {
            ProductImage prodimg = new ProductImage();
           // prodimg.ImageUpload.se= prodimg.ImageUrl;
            if (id != 0)
            {

                using (GerminmedContext db = new GerminmedContext())
                {
                    prodimg = db.ProductImage.Where(x => x.Id == id).FirstOrDefault<ProductImage>();
                }
            }
            
            return View(prodimg);
        }

        public ActionResult DeleteImage(int Id, int productid)
        {
            try
            {
                using (GerminmedContext db = new GerminmedContext())
                {


                    ProductImage img = db.ProductImage.Where(x => x.Id == Id).FirstOrDefault<ProductImage>();
                    string fullPath = Request.MapPath(img.ImageUrl);
                    if(img.ImageUrl!= "~/AppFiles/Images/Default.png")
                    { 
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                    }
                    db.ProductImage.Remove(img);
                    db.SaveChanges();

                }
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewImagesByProduct", GetAllImagesByProduct(productid)), message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);

            }
        }




        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
      
    }
}
