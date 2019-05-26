using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Germinmed.DAL;
using Germinmed.Models;

namespace Germinmed.Controllers
{
    public class ProductController : Controller
    {

        // GET: Product
        [OutputCache(Duration = 3600, VaryByParam = "none")]
        public ActionResult Index(int? Id)
        {


            using (GerminmedContext db = new GerminmedContext())
            {
                InnerBanner bnr = new InnerBanner();
                ViewBag.InnerBanner = db.InnerBanners.Where(x => x.PageName == "Products").FirstOrDefault();
            }


            ProductViewModel vm = new ProductViewModel();
            vm.Category = GetAllCat();
            // vm.SubCategory=
            vm.Brands = GetAllBrands();

            //   GetAllCatByRootCategory(Id);
            ViewBag.CategoryId = Id;

            return View(vm);
        }

      

        public ActionResult Offer(int? Id)
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

        public PartialViewResult GetRootCategories()
        {
            ProductViewModel vm = new ProductViewModel();
            vm.Category = GetAllCat();
            vm.Brands = GetAllBrands();

            return PartialView(vm);
        }

        public ActionResult GetAllProductsByBrand(int? id)
        {
            if (id != null && id != 0)
            {
                return PartialView(GetAllProductsByBrands(id));
            }
            else
                return PartialView(GetAllProducts());



        }

        public ActionResult GetAllProductsByCatAction(int? id)
        {
            if (id != null && id != 0)
            {
                return PartialView(GetAllProductsByCat(id));
            }
            else
                return PartialView(GetAllProducts());


        }




        public ActionResult GetAllProductsByCatActionOffer(int? id)
        {
            if (id != null && id != 0)
            {
                return PartialView(GetAllProductsByCatOffer(id));
            }
            else
                return PartialView(GetAllProductsOffer());


        }

        public ActionResult Details(int? id)
        {
            List<ProductViewModel> ProductVMlist = new List<ProductViewModel>();
            // ProductViewModel vm = new ProductViewModel();
            using (GerminmedContext db = new GerminmedContext())
            {


                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var product = (from prod1 in db.Product
                               join img in db.ProductImage on prod1.Id equals img.ProductId
                               join brnd in db.Brand on prod1.BrandId equals brnd.Id
                               join cat in db.Category on prod1.CategoryId equals cat.Id
                               where prod1.Id == id
                               orderby img.DisplayOrder ascending
                               select new
                               {
                                   prod1.Id,
                                   prod1.CategoryId,
                                   prod1.ProductName,
                                   prod1.Description,
                                   prod1.IsFeatured,
                                   prod1.ShowInHomePage,
                                   prod1.WebUrl,
                                   prod1.VideoUrl,
                                   prod1.InnerBannerImage,
                                   prod1.ItemCode,
                                   prod1.Catalogue,
                                   prod1.TechnicalSpecifications,
                                   prod1.IsOffer,
                                   prod1.OfferPercentage,
                                   img.ImageUrl,
                                   BrandName = brnd.Title,
                                   CategoryName = cat.Title
                               }).ToList();
                if (product == null)
                {
                    return HttpNotFound();
                }

                foreach (var item in product)
                {

                    ProductViewModel objProductvm = new ProductViewModel();// ViewModel
                    objProductvm.Id = item.Id;
                    objProductvm.CategoryId = item.CategoryId;
                    objProductvm.ProductName = item.ProductName;
                    objProductvm.Description = item.Description;
                    objProductvm.IsFeatured = item.IsFeatured;
                    objProductvm.ShowInHomePage = item.ShowInHomePage;
                    objProductvm.ImageUrl = item.ImageUrl;
                    objProductvm.IsOffer = item.IsOffer;
                    objProductvm.OfferPercentage = item.OfferPercentage;
                    objProductvm.InnerBannerImage = item.InnerBannerImage;
                    objProductvm.BrandTitle = item.BrandName;
                    objProductvm.Catalogue = item.Catalogue;
                    objProductvm.TechnicalSpecifications = item.TechnicalSpecifications;
                    objProductvm.CategoryName = item.CategoryName;
                    objProductvm.ItemCode = item.ItemCode;
                    objProductvm.WebUrl = item.WebUrl;
                    objProductvm.VideoUrl = item.VideoUrl;
                    ProductVMlist.Add(objProductvm);

                }



                return View(ProductVMlist);
            }

        }

     


        IEnumerable<ProductViewModel> GetAllProductsByCat(int? id)
        {

            List<ProductViewModel> ProductVMlist = new List<ProductViewModel>();
            IEnumerable<Category> catList = new List<Category>();
            List<Products> prodList = new List<Products>();
            List<Brands> BrandList = new List<Brands>();
            List<Products> prodListCopy = new List<Products>();
            catList = GetAllCatByRootCat(id);
            try
            {


                using (GerminmedContext db = new GerminmedContext())
                {
                    prodList = db.Product.ToList<Products>();
                    BrandList = db.Brand.ToList<Brands>();
                    foreach (var item in catList)
                    {

                        //prodListCopy.AddRange(prodList.FindAll(x => x.CategoryId == item.Id));
                        prodListCopy.AddRange(prodList.FindAll(x => x.CategoryId == id));
                    }
                    prodListCopy.AddRange(prodList.FindAll(x => x.CategoryId == id));





                    var productlist = (

                             from prod1 in prodListCopy
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
            catch (Exception ex)
            {

                throw;
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

        IEnumerable<ProductViewModel> GetAllProductsByBrands(int? id)
        {
            List<ProductViewModel> ProductVMlist = new List<ProductViewModel>();
            using (GerminmedContext db = new GerminmedContext())
            {
                var productlist = (from prod1 in db.Product
                                   join img in db.ProductImage on prod1.Id equals img.ProductId
                                   join brnd in db.Brand on prod1.BrandId equals brnd.Id
                                   where prod1.BrandId == id
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

        IEnumerable<ProductViewModel> GetAllProducts()
        {
            List<ProductViewModel> ProductVMlist = new List<ProductViewModel>();
            using (GerminmedContext db = new GerminmedContext())
            {
                var productlist = (from prod1 in db.Product
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

        public ActionResult GetAllCatByRootCategory(int? id)
        {
            ProductViewModel vm = new ProductViewModel();
            IEnumerable<Category> cat;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cat = GetAllCatByRootCat(id);
            if (cat == null)
            {
                return HttpNotFound();
            }
            vm.Category = cat;
            return PartialView(vm);
        }

        public ActionResult GetAllCatByRootCategoryCopy(int? id)
        {
            ProductViewModel vm = new ProductViewModel();
            IEnumerable<Category> cat;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cat = GetAllCatByRootCat(id);
            if (cat == null)
            {
                return HttpNotFound();
            }
            vm.Category = cat;
            return PartialView(vm);
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


                    //foreach (var item in catList)
                    //{
                    //    catListcopy.AddRange(db.Category.SqlQuery(Category.sqlQuery1).Where(x => x.ParentId == item.Id).ToList<Category>());
                    //}

                    catList.AddRange(catListcopy);
                    return catList;
                }
                else
                    return null;




            }
        }

        public ActionResult ProductSub(int Id)
        {
            IEnumerable<Category> cat = GetAllCatByParent(Id);
            ViewBag.CategoryId = Id;
            return View(cat);
            //if (cat.Count() != 0)
            //    return View(cat);
            //else
            //{
            //    return RedirectToAction("Products", "Product",new {Id=Id });
            //}

        }


        public ActionResult CategorySub(int Id)
        {
          IEnumerable<Category> cat = GetAllCatByParent(Id);
            ViewBag.CategoryId = Id;
            // if (cat.Count() != 0)
            return PartialView(cat);

          //  else
           // {
                // return RedirectToAction("Products", "Product", new { Id = Id });
              //  return PartialView("ProductAll",  GetAllProductsByCat(Id));
           // }
             
        }
        public ActionResult ProductAll(int? Id)
        {

            if (Id != null && Id != 0)
            {
                return PartialView(GetAllProductsByCat(Id));
            }
            else
                return PartialView(GetAllProducts());

        }

        public ActionResult Products(int? Id)
        {
            if (Id != null && Id != 0)
            {
                return PartialView(GetAllProductsByCat(Id));
            }
            else
                return PartialView(GetAllProducts());
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

        public PartialViewResult GetCategoryNameByID(int id )
        {
            Category cat;
            using (GerminmedContext db=new GerminmedContext())
            {
                 cat = db.Category.SqlQuery(Category.sqlQuery1).Where(x => x.Id == id).FirstOrDefault<Category>();
                
            }

            return PartialView(cat);
        }

        public PartialViewResult GetCategoryNameBreadcrumb(int id)
        {
            Category cat;
            using (GerminmedContext db = new GerminmedContext())
            {
                cat = db.Category.SqlQuery(Category.sqlQuery1).Where(x => x.Id == id).FirstOrDefault<Category>();

            }

            return PartialView(cat);
        }


        IEnumerable<Category> GetAllCat()
        {


            using (GerminmedContext db = new GerminmedContext())
            {


                return db.Category.ToList<Category>();


            }
        }



    

       




    }

    
}