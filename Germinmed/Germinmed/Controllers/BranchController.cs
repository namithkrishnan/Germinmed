using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Germinmed.DAL;
using Germinmed.Models;


namespace Germinmed.Controllers
{
    public class BranchController : Controller
    {
        // GET: Branch
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewAll()
        {
            return View(GetAll());

        }
        IEnumerable<Branches> GetAll()
        {
            using (GerminmedContext db = new GerminmedContext())
            {
                return db.Branch.ToList<Branches>();


            }

        }

        public ActionResult AddOrEdit(int id = 0)
        {
            Branches brnch = new Branches();
            if (id != 0)
            {


                using (GerminmedContext db = new GerminmedContext())
                {

                    brnch = db.Branch.Where(x => x.Id == id).FirstOrDefault<Branches>();
                }
            }

            return View(brnch);

        }

        [HttpPost]
        public ActionResult AddOrEdit(Branches brnch)
        {
            try
            {

                using (GerminmedContext db = new GerminmedContext())
                {
                    if (brnch.Id == 0)
                    {

                        if (brnch.ImageUpload != null)
                        {
                            string fileName = Path.GetFileNameWithoutExtension(brnch.ImageUpload.FileName);
                            string extension = Path.GetExtension(brnch.ImageUpload.FileName);
                            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                            brnch.ImageUrl = "~/AppFiles/Images/" + fileName;
                            brnch.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/AppFiles/Images/"), fileName));
                        }
                        db.Branch.Add(brnch);
                        db.SaveChanges();
                    }
                    else
                    {
                        var currentItem = db.Branch.Where(b => b.Id == brnch.Id).FirstOrDefault<Branches>();
                        currentItem.Phone = brnch.Phone;
                        currentItem.Title = brnch.Title;
                        currentItem.Fax = brnch.Fax;
                        currentItem.Email = brnch.Email;
                        currentItem.Location = brnch.Location;
                        currentItem.PoBox = brnch.PoBox;
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
                    Branches brnch  = db.Branch.Where(x => x.Id == Id).FirstOrDefault<Branches>();
                    string fullPath = Request.MapPath(brnch.ImageUrl);
                    if (brnch.ImageUrl != "~/AppFiles/Images/Default.png")
                    {
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                    }
                    db.Branch.Remove(brnch);
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