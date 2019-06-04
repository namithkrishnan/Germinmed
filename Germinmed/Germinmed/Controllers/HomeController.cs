using Germinmed.DAL;
using Germinmed.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Germinmed.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {

        public ActionResult Index1()
        {
            using (GerminmedContext db = new GerminmedContext())
            {
                return View(db.Banners.ToList<Banner>());
            }
        }


        // GET: Home
        [OutputCache(Duration = 3600, VaryByParam = "none")]
        public ActionResult Index()
        {
            using (GerminmedContext db = new GerminmedContext())
            {
                return View(db.Banners.ToList<Banner>());
            }
            // return View();
        }


        public ActionResult Search(string search)
        {
            return View(GetSearchResult(search));
        }

        List<int> allSubCats = new List<int>();
        void GetChilds(int id)
        {
            using (GerminmedContext db = new GerminmedContext())
            {
                List<int> cats = db.Category.Where(x => x.ParentId == id).Select(y => y.Id).ToList();
                allSubCats.AddRange(cats);
                foreach (int item in cats)
                {
                    if (id != item)
                        GetChilds(item);
                }
            }
        }

        List<string> FetchSearchInputs(string searchText)
        {
            List<string> searchInputs = new List<string>();
            //Adding the input text without space eg)if user enters product name like S500H or S500 H 
            searchInputs.Add(searchText.Replace(" ", "").ToUpper());
            if (searchText.Length >= 3)
            {
                searchInputs.Add(searchText.Substring(0, 3).ToUpper());
            }

            //Spliting the input by space
            string[] splitString = searchText.Split(' ');
            foreach (string item in splitString)
            {
                searchInputs.Add(item.ToUpper());
            }

            return searchInputs;
        }

        public SearchViewModel GetSearchResult(string searchText)
        {
            SearchViewModel vm = new SearchViewModel();
            List<Events> evet = new List<Events>();
            List<Products> prod = new List<Products>();

            if (searchText != null && searchText != "")
            {
                //may be the user searches not an exacting words in the input. So we need to consider the maximum possibilities
                List<string> searchInputs = FetchSearchInputs(searchText);

                using (GerminmedContext db = new GerminmedContext())
                {
                    //Adding Products to list
                    //prod.AddRange(db.Product.Where(p => p.ProductName != null && p.ProductName.ToUpper().Contains(searchText)).ToList());
                    //prod.AddRange(db.Product.Where(p => p.ProductName != null && searchInputs.Any(p.ProductName.ToUpper().Contains)).ToList());
                    prod.AddRange(db.Product.Where(p => p.ProductName != null && searchInputs.Any(item => p.ProductName.ToUpper().Contains(item))).ToList());

                    //Checking the categories and adding products under it.
                    //List<Category> objCategories = db.Category.Where(p => p.Title != null && p.Title.ToUpper().Contains(searchText)).ToList();
                    List<Category> objCategories = db.Category.Where(p => p.Title != null && searchInputs.Any(item => p.Title.ToUpper().Contains(item))).ToList();
                    foreach (Category cat in objCategories)
                    {
                        prod.AddRange(db.Product.Where(p => p.CategoryId == cat.Id).ToList());
                        allSubCats = new List<int>();
                        GetChilds(cat.Id);
                        foreach (int catId in allSubCats)
                        {
                            prod.AddRange(db.Product.Where(p => p.CategoryId == catId).ToList());
                        }
                    }
                    prod = prod.GroupBy(p => p.Id).Select(g => g.First()).ToList();

                    searchText = searchText.ToUpper();
                    //Adding brands
                    List<Brands> brands = db.Brand.Where(p => p.Title != null && p.Title.ToUpper().Contains(searchText)).ToList();
                    List<int> brandIds = brands.Select(p => p.Id).ToList();
                    prod.AddRange(db.Product.Where(p => p.BrandId != null && brandIds.Contains(p.BrandId.Value)).ToList());

                    //Adding events 
                    evet.AddRange(db.Event.Where(p => p.Description != null && p.Description.ToUpper().Contains(searchText)).ToList());

                }
            }
            vm.ProductsList = prod;
            vm.EventList = evet;
            return (vm);
        }
        //public SearchViewModel GetSearchResult(string searchText)
        //{
        //    SearchViewModel vm = new SearchViewModel();
        //    GerminmedContext db = new GerminmedContext();
        //    List<Events> evet = null;
        //    List<Products> prod = null;


        //    if (searchText != null && searchText != "")
        //    {
        //        using (var connection = db.Database.Connection)
        //        {

        //            connection.Open();
        //            var command = connection.CreateCommand();
        //            var command1 = connection.CreateCommand();
        //            command.CommandText = "EXEC SP_SearchTables @Tablenames = 'Products',@SearchStr  = '%" + searchText + "%' ";
        //            command1.CommandText = "EXEC SP_SearchTables @Tablenames = 'Events',@SearchStr  = '%" + searchText + "%' ";

        //            using (var reader = command.ExecuteReader())
        //            {
        //                if (reader.HasRows)
        //                    prod = ((IObjectContextAdapter)db)
        //                               .ObjectContext
        //                               .Translate<Products>(reader)
        //                               .ToList();
        //            }
        //            using (var reader1 = command1.ExecuteReader())
        //            {
        //                if (reader1.HasRows)
        //                    evet =
        //                       ((IObjectContextAdapter)db)
        //                           .ObjectContext
        //                           .Translate<Events>(reader1)
        //                           .ToList();
        //            }
        //            connection.Close();

        //        }
        //    }
        //    vm.ProductsList = prod;
        //    vm.EventList = evet;
        //    return (vm);
        //}


        public ActionResult GetFeaturedProducts()
        {

            List<HomeViewModel> ProductVMlist = new List<HomeViewModel>();
            using (GerminmedContext db = new GerminmedContext())
            {

                // to hold list of Customer and order details

                var productlist = (from prod1 in db.Product

                                   where prod1.IsFeatured == true

                                   select new
                                   {
                                       prod1.Id,
                                       prod1.ProductName,
                                       prod1.Description,
                                       prod1.IsFeatured,
                                       prod1.ShowInHomePage,
                                       prod1.ImagePath
                                   }).ToList();
                // productlist = productlist.
                foreach (var item in productlist)
                {

                    HomeViewModel objHome = new HomeViewModel();// ViewModel
                    objHome.Id = item.Id;
                    objHome.ProductName = item.ProductName;


                    objHome.Description = item.Description;


                    // objHome.Description = item.Description;

                    objHome.IsFeatured = item.IsFeatured;
                    objHome.ShowInHomePage = item.ShowInHomePage;
                    objHome.ImageUrl = item.ImagePath;
                    ProductVMlist.Add(objHome);
                }
            }
            return View(ProductVMlist);
        }

        public ActionResult GetMenuCategories()
        {

            return View(GetAllCat());
        }

        public ActionResult GetCategoryFooter()
        {

            return View(GetAllCat());

        }
        public ActionResult GetRootCategories()
        {

            return View(GetAllCat());
        }




        IEnumerable<Category> GetAllCat()
        {
            Category catg = new Category();

            using (GerminmedContext db = new GerminmedContext())
            {


                catg.CategoryList = db.Category.ToList<Category>();

                return catg.CategoryList;
            }
        }


        public ActionResult GetAllBrand()
        {
            return View(GeAllBrandsList());
        }



        IEnumerable<Brands> GeAllBrandsList()
        {


            using (GerminmedContext db = new GerminmedContext())
            {
                return db.Brand.ToList<Brands>();
            }
        }



    }
}