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
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewAll()
        {
            return View(GetAllOrder());

        }
        IEnumerable<Order> GetAllOrder()
        {
            using (GerminmedContext db = new GerminmedContext())
            {
                List<Products> prod = db.Product.ToList<Products>();
                List<Users> usr = db.User.ToList<Users>();
                List<Order> ordertList = db.Orders.ToList<Order>();

                foreach (var item in ordertList)
                {
                    item.ProducName = db.Product.Where(x => x.Id == item.ProductId).FirstOrDefault<Products>().ProductName;
                    item.UserName = db.User.Where(x => x.Id == item.UserId).FirstOrDefault<Users>().UserName;
                }
                return ordertList;
            }

        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (GerminmedContext db=new GerminmedContext())
            {

           
           Order order = db.Orders.Find(id);
                var orders = (from ord in db.Orders
                               join prod in db.Product on ord.ProductId equals prod.Id
                               join usr in db.User on ord.UserId equals usr.Id
                               where ord.Id == id
                              select new
                               {
                                  ord.Id,
                                  ProducName = prod.ProductName,
                                  UserName=usr.UserName,
                                  ord.ProductId,
                                  ord.UserId,
                                  ord.Qty,
                                 }).FirstOrDefault();
                if (orders == null)
                {
                    return HttpNotFound();
                }
                order.Id = orders.Id;
                order.ProductId = orders.ProductId;
                order.UserId = orders.UserId;
                order.ProducName =orders.ProducName;
                order.UserName = orders.UserName;
                order.Qty = orders.Qty;

                return View(order);


            }
        }

        public ActionResult Delete(int Id)
        {
            try
            {
                using (GerminmedContext db = new GerminmedContext())
                {
                    Order ord = db.Orders.Where(x => x.Id == Id).FirstOrDefault<Order>();
                    db.Orders.Remove(ord);
                    db.SaveChanges();
                }
                return Json(data: new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", model: GetAllOrder()), message = "Deleted Successfully" }, behavior: JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


    }
}
