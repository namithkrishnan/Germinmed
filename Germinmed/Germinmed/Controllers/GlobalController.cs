using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Germinmed.DAL;
using Germinmed.Models;

namespace Germinmed.Controllers
{
    public class GlobalController : Controller
    {
        // GET: Global
        public ActionResult Index()
        {
            return View(GetAll());
        }


        IEnumerable<Branches> GetAll()
        {

            using (GerminmedContext db = new GerminmedContext())
            {
                InnerBanner bnr = new InnerBanner();
                ViewBag.InnerBanner = db.InnerBanners.Where(x => x.PageName == "GlobalPresence").FirstOrDefault();

                return db.Branch.ToList<Branches>();
            }

        }
    }
}