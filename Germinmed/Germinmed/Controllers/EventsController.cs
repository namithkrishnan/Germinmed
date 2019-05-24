using Germinmed.DAL;
using Germinmed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Germinmed.Controllers
{
    public class EventsController : Controller
    {
        // GET: Events
        public ActionResult Index()
        {
            return View(GetAll());
        }


        public ActionResult EventDetails(int? id)
        {
            using (GerminmedContext db = new GerminmedContext())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Events evnt = db.Event.Find(id);
                if (evnt == null)
                {
                    return HttpNotFound();
                }
                return View(evnt);
            }
            
        }

        IEnumerable<Events> GetAll()
        {
            using (GerminmedContext db =new GerminmedContext())
            {

                InnerBanner bnr = new InnerBanner();
                ViewBag.InnerBanner = db.InnerBanners.Where(x => x.PageName == "Events").FirstOrDefault();
                return db.Event.ToList<Events>();
            }
        }
    }
}