using Germinmed.DAL;
using Germinmed.Models;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Germinmed.Controllers
{
    public class PromotionController : Controller
    {
        // GET: Promotion
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewAll()
        {
            return View(GetAll());

        }
        IEnumerable<Promotions > GetAll()
        {
            using (GerminmedContext db = new GerminmedContext())
            {
                return db.Promotion.ToList<Promotions>();


            }

        }

        public ActionResult AddOrEdit(int id = 0)
        {
            Promotions promo = new Promotions();
            if (id != 0)
            {


                using (GerminmedContext db = new GerminmedContext())
                {

                    promo = db.Promotion.Where(x => x.Id == id).FirstOrDefault<Promotions>();
                }
            }

            return View(promo);

        }

        [HttpPost]
        public ActionResult AddOrEdit(Promotions promo)
        {
            try
            {

                using (GerminmedContext db = new GerminmedContext())
                {
                    if (promo.Id == 0)
                    {

                       
                        db.Promotion.Add(promo);
                        db.SaveChanges();
                    }
                    else
                    {
                       
                        db.Entry(promo).State = EntityState.Modified;
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
                    Promotions promo = db.Promotion.Where(x => x.Id == Id).FirstOrDefault<Promotions>();
                    
                    db.Promotion.Remove(promo);
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