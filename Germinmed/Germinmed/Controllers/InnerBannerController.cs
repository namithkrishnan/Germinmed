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
    public class InnerBannerController : Controller
    {
        // GET: InnerBanner
        public ActionResult Index()
        {
            return View();
        }



        public ActionResult UpdateRow(InnerBanner model, int Id, int fromPosition, int toPosition)
        {
            using (GerminmedContext db = new GerminmedContext())
            {
                var GenderList = db.Banners.ToList();
                db.InnerBanners.First(c => c.DisplayOrder == Id).DisplayOrder = toPosition;
                db.SaveChanges();
            }
            return Json(data: new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", model: GetAll()), message = "Submitted Successfully" }, behavior: JsonRequestBehavior.AllowGet);
        }




        public ActionResult ViewAll()
        {
            return View(GetAll());

        }
        IEnumerable<InnerBanner> GetAll()
        {
            using (GerminmedContext db = new GerminmedContext())
            {
                return db.InnerBanners.ToList<InnerBanner>();


            }

        }

        public ActionResult AddOrEdit(int id = 0)
        {
            InnerBanner banr = new InnerBanner();
            if (id != 0)
            {


                using (GerminmedContext db = new GerminmedContext())
                {

                    banr = db.InnerBanners.Where(x => x.Id == id).FirstOrDefault<InnerBanner>();
                }
            }

            return View(banr);

        }

        [HttpPost]
        public ActionResult AddOrEdit(InnerBanner banr)
        {
            try
            {

                using (GerminmedContext db = new GerminmedContext())
                {
                    if (banr.Id == 0)
                    {

                        if (banr.ImageUpload != null)
                        {
                            string fileName = Path.GetFileNameWithoutExtension(banr.ImageUpload.FileName);
                            string extension = Path.GetExtension(banr.ImageUpload.FileName);
                            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                            banr.ImageUrl = "~/AppFiles/Images/" + fileName;
                            banr.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/AppFiles/Images/"), fileName));
                        }
                        db.InnerBanners.Add(banr);
                        db.SaveChanges();
                    }
                    else
                    {
                        var currentItem = db.InnerBanners.Where(b => b.Id == banr.Id).FirstOrDefault<InnerBanner>();
                        currentItem.Title = banr.Title;
                        currentItem.PageName = banr.PageName;
                        currentItem.Description = banr.Description;
                        currentItem.DisplayOrder = banr.DisplayOrder;

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
                    InnerBanner banr = db.InnerBanners.Where(x => x.Id == Id).FirstOrDefault<InnerBanner>();
                    string fullPath = Request.MapPath(banr.ImageUrl);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                    db.InnerBanners.Remove(banr);
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