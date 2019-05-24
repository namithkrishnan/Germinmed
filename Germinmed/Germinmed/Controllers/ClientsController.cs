using Germinmed.Models;
using Germinmed.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Germinmed.Controllers
{
    public class ClientsController : Controller
    {
        // GET: Clients
        public ActionResult Index()
        {
            return View(GetAll());
        }

        IEnumerable<Clients> GetAll()
        {
            using (GerminmedContext  db=new GerminmedContext())
            {

                InnerBanner bnr = new InnerBanner();
                ViewBag.InnerBanner = db.InnerBanners.Where(x => x.PageName == "OurReferences").FirstOrDefault();

                return db.Client.ToList<Clients>();
            }
        }
    }
}