using Germinmed.DAL;
using Germinmed.Models;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Germinmed.Controllers
{
    public class CategoryController : Controller
    {
        
        // GET: Category
        public ActionResult Index()
        {
            Category catg1 = new Category();
            using (GerminmedContext db = new GerminmedContext())
            {

                catg1.CategoryList = db.Category.ToList<Category>();
                ViewBag.Data= catg1.CategoryList;
            }
                return View();
        }




        public ActionResult ViewAll()
        {
            return View(GetAll());

        }


    
        

        IEnumerable<Category> GetAll()
        {
            Category catg = new Category();

            using (GerminmedContext db = new GerminmedContext())
            {


                catg.CategoryList = db.Category
                             .SqlQuery(Category.sqlQuery)
                             .ToList<Category>();
               // catg.CategoryList.Add(new Category { Id = 0, Title = "[None]" });

                return catg.CategoryList;
            }

        }

        public ActionResult AddOrEdit(int id = 0)
        {
        
            Category catg = new Category();
            if (id != 0)
            {

                using (GerminmedContext db = new GerminmedContext())
                {
                   
                    catg = db.Category.Where(x => x.Id == id).FirstOrDefault<Category>();

                    catg.CategoryList = db.Category
                            .SqlQuery(Category.sqlQuery)
                            .ToList<Category>();
                    catg.CategoryList.Add(new Category { Id = 0, Title = "[None]"});
                   
                }

            }
            else
            {
                using (GerminmedContext db = new GerminmedContext())
                {
                 
                   catg.CategoryList = db.Category
                           .SqlQuery(Category.sqlQuery)
                          .ToList<Category>();
                   catg.CategoryList.Add(new Category { Id = 0, Title = "[None]" });
                   


                }
            }
            

            return View(catg);

        }

        [HttpPost]
        public ActionResult AddOrEdit(Category catg,FormCollection fobj)
        {
            try
            {

                using (GerminmedContext db = new GerminmedContext())
                {
                    if (catg.Id == 0)
                    {
                        if (catg.ImageUpload != null)
                        {
                            string fileName = Path.GetFileNameWithoutExtension(catg.ImageUpload.FileName);
                            string extension = Path.GetExtension(catg.ImageUpload.FileName);
                            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                            catg.ImageUrl = "~/AppFiles/Images/" + fileName;
                            catg.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/AppFiles/Images/"), fileName));
                        }
                        else
                        {
                            catg.ImageUrl =null;
                        }

                        if (catg.InnerBannerImageUpload != null)
                        {
                            string fileName = Path.GetFileNameWithoutExtension(catg.InnerBannerImageUpload.FileName);
                            string extension = Path.GetExtension(catg.InnerBannerImageUpload.FileName);
                            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                            catg.InnerBannerImageUrl = "~/AppFiles/Images/" + fileName;
                            catg.InnerBannerImageUpload.SaveAs(Path.Combine(Server.MapPath("~/AppFiles/Images/"), fileName));
                        }
                        else
                        {
                            catg.InnerBannerImageUrl = null;
                        }

                        db.Category.Add(catg);
                        db.SaveChanges();
                    }
                    else
                    {

                        //if (catg.ImageUpload != null)
                        //{
                        //    string fileName = Path.GetFileNameWithoutExtension(catg.ImageUpload.FileName);
                        //    string extension = Path.GetExtension(catg.ImageUpload.FileName);
                        //    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        //    catg.ImageUrl = "~/AppFiles/Images/" + fileName;
                        //    catg.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/AppFiles/Images/"), fileName));
                        //}
                        //else
                        //{
                        //    catg.ImageUrl = null;
                        //}
                        var currentItem = db.Category.Where(b => b.Id == catg.Id).FirstOrDefault<Category>();
                        currentItem.Title = catg.Title;
                        currentItem.ParentId = catg.ParentId;
                        currentItem.Description = catg.Description;
                      //  currentItem.ImageUrl = catg.ImageUrl;

                        db.Entry(currentItem).State = EntityState.Modified;

                        db.SaveChanges();
                    }
                

                }
                return Json(data: new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", model: GetAll()), message = "Submitted Successfully" }, behavior: JsonRequestBehavior.AllowGet);

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
                    Category catg = db.Category.Where(x => x.Id == Id).FirstOrDefault<Category>();
                    string fullPath = Request.MapPath(catg.ImageUrl);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                    db.Category.Remove(catg);
                    db.SaveChanges();
                }
                return Json(data: new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", model: GetAll()), message = "Deleted Successfully" }, behavior: JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}