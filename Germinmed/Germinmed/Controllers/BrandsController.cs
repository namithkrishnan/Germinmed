using Germinmed.DAL;
using Germinmed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Germinmed.Controllers
{
    public class BrandsController : Controller
    {
        // GET: Brands
        public ActionResult Index()
        {
            return View(GetAll());
        }

        IEnumerable<Brands> GetAll()
        {

            using (GerminmedContext db = new GerminmedContext())
            {
                InnerBanner bnr = new InnerBanner();
                ViewBag.InnerBanner = db.InnerBanners.Where(x => x.PageName == "Brand").FirstOrDefault();

                return db.Brand.Where(x=>x.ShowInBrandPage==true).ToList<Brands>();
            }
                
        }
    }
}