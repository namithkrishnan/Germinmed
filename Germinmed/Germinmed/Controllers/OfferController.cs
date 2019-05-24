using Germinmed.DAL;
using Germinmed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Germinmed.Controllers
{
    public class OfferController : Controller
    {
        // GET: Offer
        // GET: Product
        public ActionResult Index(int? Id)
        {


            using (GerminmedContext db = new GerminmedContext())
            {
                InnerBanner bnr = new InnerBanner();
                ViewBag.InnerBanner = db.InnerBanners.Where(x => x.PageName == "Offer").FirstOrDefault();
            }


            ProductViewModel vm = new ProductViewModel();
            vm.Category = GetAllCat();
            // vm.SubCategory=
            vm.Brands = GetAllBrands();

            //   GetAllCatByRootCategory(Id);


            return View(vm);
        }


        public ActionResult CategorySubOffer(int Id)
        {
            IEnumerable<Category> cat = GetAllCatByParent(Id);

           // if (cat.Count() != 0)
                return View(cat);
            //else
            //{
            //    return RedirectToAction("ProductsOffer", "Offer", new { Id = Id });
            //}

        }
      

        public ActionResult ProductSubOffer(int Id)
        {
            IEnumerable<Category> cat = GetAllCatByParent(Id);

            if (cat.Count() != 0)
                return View(cat);
            else
            {
                return RedirectToAction("ProductsOffer", "Offer", new { Id = Id });
            }

        }

        public ActionResult ProductAllOffer(int? Id)
        {

            if (Id != null && Id != 0)
            {
                return PartialView(GetAllProductsByCatOffer(Id));
            }
            else
                return PartialView(GetAllProductsOffer());

        }

        public ActionResult ProductsOffer(int? Id)
        {
            if (Id != null && Id != 0)
            {
                return PartialView(GetAllProductsByCatOffer(Id));
            }
            else
                return PartialView(GetAllProductsOffer());
        }

        IEnumerable<Category> GetAllCatByParent(int? id)
        {
            List<Category> catList = new List<Category>();
            using (GerminmedContext db = new GerminmedContext())
            {
                if (id != null)
                {
                    catList = db.Category.SqlQuery(Category.sqlQuery1).Where(x => x.ParentId == id).ToList<Category>();
                }

            }
            return catList;

        }

        IEnumerable<Category> GetAllCat()
        {


            using (GerminmedContext db = new GerminmedContext())
            {


                return db.Category.ToList<Category>();


            }
        }

        IEnumerable<Brands> GetAllBrands()
        {
            Brands brnd = new Brands();

            using (GerminmedContext db = new GerminmedContext())
            {

                brnd.BrandList = db.Brand.ToList<Brands>();
                brnd.BrandList.Add(new Brands { Id = 0, Title = "Select Brand" });
                return brnd.BrandList;
            }
        }

        IEnumerable<Category> GetAllCatByRootCat(int? id)
        {
            List<Category> catList = new List<Category>();
            List<Category> catListcopy = new List<Category>();
            using (GerminmedContext db = new GerminmedContext())
            {


                if (id != null)
                {
                    catList = db.Category.SqlQuery(Category.sqlQuery1).Where(x => x.ParentId == id).ToList<Category>();


                    foreach (var item in catList)
                    {


                        catListcopy.AddRange(db.Category.SqlQuery(Category.sqlQuery1).Where(x => x.ParentId == item.Id).ToList<Category>());

                    }

                    catList.AddRange(catListcopy);
                    return catList;
                }
                else
                    return null;




            }
        }

        IEnumerable<ProductViewModel> GetAllProductsByCatOffer(int? id)
        {

            List<ProductViewModel> ProductVMlist = new List<ProductViewModel>();
            IEnumerable<Category> catList = new List<Category>();
            List<Products> prodList = new List<Products>();
            List<Products> prodListCopy = new List<Products>();
            catList = GetAllCatByRootCat(id);

            using (GerminmedContext db = new GerminmedContext())
            {
                prodList = db.Product.Where(x => x.IsOffer == true).ToList<Products>();
                foreach (var item in catList)
                {

                    prodListCopy.AddRange(prodList.FindAll(x => x.CategoryId == item.Id));
                }
                prodListCopy.AddRange(prodList.FindAll(x => x.CategoryId == id));
                var productlist = (from prod1 in prodListCopy
                                   join img in db.ProductImage on prod1.Id equals img.ProductId
                                   join brnd in db.Brand on prod1.BrandId equals brnd.Id
                                   orderby img.DisplayOrder ascending
                                   select new
                                   {
                                       prod1.Id,
                                       prod1.ProductName,
                                       prod1.Description,
                                       prod1.IsFeatured,
                                       prod1.IsOffer,
                                       prod1.OfferPercentage,
                                       prod1.ShowInHomePage,
                                       img.ImageUrl,
                                       brnd.Title
                                   }).ToList();
                // productlist = productlist.
                foreach (var item in productlist)
                {

                    ProductViewModel objHome = new ProductViewModel();// ViewModel
                    objHome.Id = item.Id;
                    objHome.ProductName = item.ProductName;
                    objHome.Description = item.Description;
                    objHome.IsFeatured = item.IsFeatured;
                    objHome.IsOffer = item.IsOffer;
                    objHome.OfferPercentage = item.OfferPercentage;
                    objHome.ShowInHomePage = item.ShowInHomePage;
                    objHome.ImageUrl = item.ImageUrl;
                    objHome.BrandTitle = item.Title;
                    ProductVMlist.Add(objHome);
                }
                return ProductVMlist;
            }

        }
      
        IEnumerable<ProductViewModel> GetAllProductsOffer()
        {
            List<ProductViewModel> ProductVMlist = new List<ProductViewModel>();
            using (GerminmedContext db = new GerminmedContext())
            {
                var productlist = (from prod1 in db.Product
                                   join img in db.ProductImage on prod1.Id equals img.ProductId
                                   join brnd in db.Brand on prod1.BrandId equals brnd.Id
                                   where prod1.IsOffer == true
                                   orderby img.DisplayOrder ascending
                                   select new
                                   {
                                       prod1.Id,
                                       prod1.ProductName,
                                       prod1.Description,
                                       prod1.IsFeatured,
                                       prod1.IsOffer,
                                       prod1.OfferPercentage,
                                       prod1.ShowInHomePage,
                                       img.ImageUrl,
                                       brnd.Title
                                   }).ToList();

                foreach (var item in productlist)
                {
                    ProductViewModel objHome = new ProductViewModel();// ViewModel
                    objHome.Id = item.Id;
                    objHome.ProductName = item.ProductName;
                    objHome.Description = item.Description;
                    objHome.IsFeatured = item.IsFeatured;
                    objHome.IsOffer = item.IsOffer;
                    objHome.OfferPercentage = item.OfferPercentage;
                    objHome.ShowInHomePage = item.ShowInHomePage;
                    objHome.ImageUrl = item.ImageUrl;
                    objHome.BrandTitle = item.Title;
                    ProductVMlist.Add(objHome);
                }
                return ProductVMlist;
            }

        }
    }
}