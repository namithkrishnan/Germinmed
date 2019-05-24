using Germinmed.DAL;
using Germinmed.Models;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Germinmed.Controllers
{
    public class EventController : Controller
    {
        // GET: Event
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult ViewAll()
        {
            return View(GetAll());

        }
        IEnumerable<Events> GetAll()
        {
            using (DAL.GerminmedContext db = new GerminmedContext())
            {
                return db.Event.ToList<Events>();


            }

        }

        public ActionResult AddOrEdit(int id = 0)
        {
            Events evnt = new Events();
            if (id != 0)
            {


                using (GerminmedContext db = new GerminmedContext())
                {

                    evnt = db.Event.Where(x => x.Id == id).FirstOrDefault<Events>();
                }
            }

            return View(evnt);

        }

        [HttpPost]
        public ActionResult AddOrEdit(Events evnt)
        {
            try
            {

                using (GerminmedContext db = new GerminmedContext())
                {
                    if (evnt.Id == 0)
                    {

                        if (evnt.ImageUpload != null)
                        {
                            string fileName = Path.GetFileNameWithoutExtension(evnt.ImageUpload.FileName);
                            string extension = Path.GetExtension(evnt.ImageUpload.FileName);
                            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                            evnt.ImageUrl = "~/AppFiles/Images/" + fileName;
                            evnt.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/AppFiles/Images/"), fileName));
                        }
                        db.Event.Add(evnt);
                        db.SaveChanges();
                    }
                    else
                    {
                        var currentItem = db.Event.Where(b => b.Id == evnt.Id).FirstOrDefault<Events>();
                        currentItem.Date = evnt.Date;
                        currentItem.Title = evnt.Title;
                        currentItem.Description = evnt.Description;
                        db.Entry(currentItem).State = EntityState.Modified;

                        db.SaveChanges();
                    }


                }
                return Json(data: new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", model: GetAll()), message = "Submitted Successfully" }, behavior: JsonRequestBehavior.AllowGet);

                //RedirectToAction("ViewAll");
            }
            catch (DataException ex)
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
                    Events evnt = db.Event.Where(x => x.Id == Id).FirstOrDefault<Events>();
                    string fullPath = Request.MapPath(evnt.ImageUrl);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                    db.Event.Remove(evnt);
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