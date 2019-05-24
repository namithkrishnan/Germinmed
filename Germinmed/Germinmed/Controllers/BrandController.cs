using Germinmed.DAL;
using Germinmed.Models;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Germinmed.Controllers
{
    public class BrandController : Controller
    {
        // GET: Brand
        public ActionResult Index()
        {
            return View();
            
        }


        public ActionResult ViewAll()
        {
            return View(GetAll());

        }
        IEnumerable<Brands> GetAll()
        {
            using (GerminmedContext db = new GerminmedContext())
            {
                return db.Brand.ToList<Brands>();


            }

        }
       
        public ActionResult AddOrEdit(int id = 0)
        {
            Brands brnd = new Brands();
            if (id != 0)
            {

               
                using (GerminmedContext db = new GerminmedContext())
                {
                    
                    brnd = db.Brand.Where(x => x.Id == id).FirstOrDefault<Brands>();
                }
            }
           
            return View(brnd);

        }

        [HttpPost]
        public ActionResult AddOrEdit(Brands brnd)
        {
            try
            {
               
                using (GerminmedContext db = new GerminmedContext())
                {
                    if (brnd.Id == 0)
                    {

                        if (brnd.ImageUpload != null)
                        {
                            string fileName = Path.GetFileNameWithoutExtension(brnd.ImageUpload.FileName);
                            string extension = Path.GetExtension(brnd.ImageUpload.FileName);
                            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                            brnd.ImageUrl = "~/AppFiles/Images/" + fileName;
                            brnd.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/AppFiles/Images/"), fileName));
                        }
                        db.Brand.Add(brnd);
                        db.SaveChanges();
                    }
                    else
                    {
                        var currentItem = db.Brand.Where(b => b.Id == brnd.Id).FirstOrDefault<Brands>();
                        currentItem.Url = brnd.Url;
                        currentItem.Title = brnd.Title;
                        currentItem.ShowInBrandPage = brnd.ShowInBrandPage;
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
                    Brands brnd = db.Brand.Where(x => x.Id == Id).FirstOrDefault<Brands>();
                    string fullPath = Request.MapPath(brnd.ImageUrl);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                    db.Brand.Remove(brnd);
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