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
    public class ClientController : Controller
    {
        // GET: Client
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UpdateRow(Clients model, int Id, int fromPosition, int toPosition)
        {
            using (GerminmedContext db = new GerminmedContext())
            {
                var GenderList = db.Client.ToList();
                db.Client.First(c => c.DisplayOrder == Id).DisplayOrder = toPosition;
                db.SaveChanges();
            }
            return Json(data: new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", model: GetAll()), message = "Submitted Successfully" }, behavior: JsonRequestBehavior.AllowGet);
        }
        public ActionResult ViewAll()
        {
            return View(GetAll());

        }
        IEnumerable<Clients> GetAll()
        {
            using (GerminmedContext db = new GerminmedContext())
            {
                return db.Client.ToList<Clients>();


            }

        }

        public ActionResult AddOrEdit(int id = 0)
        {
            Clients clns = new Clients();
            if (id != 0)
            {


                using (GerminmedContext db = new GerminmedContext())
                {

                    clns = db.Client.Where(x => x.Id == id).FirstOrDefault<Clients>();
                }
            }

            return View(clns);

        }

        [HttpPost]
        public ActionResult AddOrEdit(Clients clns)
        {
            try
            {

                using (GerminmedContext db = new GerminmedContext())
                {
                    if (clns.Id == 0)
                    {

                        if (clns.ImageUpload != null)
                        {
                            string fileName = Path.GetFileNameWithoutExtension(clns.ImageUpload.FileName);
                            string extension = Path.GetExtension(clns.ImageUpload.FileName);
                            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                            clns.ImageUrl = "~/AppFiles/Images/" + fileName;
                            clns.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/AppFiles/Images/"), fileName));
                        }
                        db.Client.Add(clns);
                        db.SaveChanges();
                    }
                    else
                    {
                        var currentItem = db.Client.Where(b => b.Id == clns.Id).FirstOrDefault<Clients>();
                       
                        currentItem.Url = clns.Url;
                        currentItem.DisplayOrder = clns.DisplayOrder;
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
                    Clients clns = db.Client.Where(x => x.Id == Id).FirstOrDefault<Clients>();
                    string fullPath = Request.MapPath(clns.ImageUrl);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                    db.Client.Remove(clns);
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