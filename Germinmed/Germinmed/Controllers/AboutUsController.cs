using Germinmed.DAL;
using Germinmed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Germinmed.Controllers
{
    public class AboutUsController : Controller
    {
        // GET: AboutUs
        public ActionResult Index()
        {
            using (GerminmedContext db = new GerminmedContext())
            {
                InnerBanner bnr = new InnerBanner();
                bnr = db.InnerBanners.Where(x => x.PageName == "AboutUs").FirstOrDefault();
                return View(bnr);
            }
        }
    }
}