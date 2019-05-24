using Germinmed.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Germinmed;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using System.IO;

namespace Germinmed.Models
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index(int? id)
        {
                                  
            return View();
            
        }

        //public ActionResult CartItems()
        //{


        //    if (id != null)
        //        Add(id, 0);
        //    return PartialView("Add");
        //}


        public void setSesstionCart(int? id, int? Qty)
        {
            GerminmedContext db = new GerminmedContext();
            Products productModel = new Products();
            ProductImage img = new ProductImage();
           
            img = db.ProductImage.Where(x => x.ProductId == id).First();
            productModel = db.Product.Find(id);
            productModel.ImageUrl = img.ImageUrl;
            if (Session["cart"] == null)
            {
                List<Cart> cart = new List<Cart>();
                cart.Add(new Cart { Product = productModel, Quantity = 1 });
                Session["cart"] = cart;
            }
            else
            {
                List<Cart> cart = (List<Cart>)Session["cart"];
                int index = IsExist(id);
                if (index != -1)
                {
                    if ((Qty == -1) && (cart[index].Quantity > 1))
                    {

                        cart[index].Quantity--;
                    }
                    if (Qty == 1)
                    {
                        cart[index].Quantity++;
                    }
                }
                else
                {
                    cart.Add(new Cart { Product = productModel, Quantity =( Qty!=null?Qty.Value:1) });
                }
                Session["cart"] = cart;

            }
        }
        [RefreshDetectFilter]
        public ActionResult Add(int? id, int? Qty)
        {
            //string  pagename= Request.UrlReferrer.ToString();
           
                if (id != null)
                    setSesstionCart(id, Qty);
           
           
            return View();
        }

        public ActionResult Remove(int id)
        {
            if (Request.UrlReferrer != null)
            {
                Session["controller"] = (Request.UrlReferrer.Segments.Skip(1).Take(1).SingleOrDefault() ?? "Home").Trim('/');

                Session["action"] = (Request.UrlReferrer.Segments.Skip(2).Take(1).SingleOrDefault() ?? "Index").Trim('/');
            }


            List<Cart> cart = (List<Cart>)Session["cart"];
            int index = IsExist(id);
            cart.RemoveAt(index);
            Session["cart"] = cart;

           // if(Session["controller"]!=null&& Session["action"]!=null)
           // {
           //     return RedirectToAction(Session["action"].ToString(), Session["controller"].ToString());
          //  }
           // else
          //  {
                return Redirect(Request.UrlReferrer.ToString());
            // }



        }

        private int IsExist(int? id)
        {
            List<Cart> cart = (List<Cart>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
                if (cart[i].Product.Id.Equals(id))
                    return i;
            return -1;
        }

        public ActionResult Checkout()
        {

            if (Session["UserId"] != null)
            {
                UserViewModel vm = new UserViewModel
                {
                    FirstName = Session["firstName"].ToString(),
                    Email = Session["userName"].ToString()
                };

                return View(vm);
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult OrederConfirm(UserViewModel uvm)
        {
            if(Session["cart"]!=null && Session["UserId"]!=null)
            {
             string firstName=   uvm.FirstName;
                string userName = Session["userName"].ToString();
                string phone = uvm.Phone;
                string cartTableForEmail="";
                string ToEmail= uvm.Email;
            List<Cart> cart = (List<Cart>)Session["cart"];
                List<Order> orderList = new List<Order>();
                
                foreach (var item in cart)
                {
                    Order orderModel = new Order();
                    orderModel.ProductId = item.Product.Id;
                    orderModel.UserId = (int)Session["UserId"];
                    orderModel.Qty = item.Quantity;
                    orderList.Add(orderModel);
                    cartTableForEmail = cartTableForEmail+ "<tr><td>" +item.Product.ProductName+"</td><td>"+ item.Quantity + "</td><td>"+item.Product.Price+"</td></tr>";

                }
                using (GerminmedContext db = new GerminmedContext())
                {
                    db.Orders.AddRange(orderList);
                    db.SaveChanges();
                }
                SendEmail(firstName,userName,phone,cartTableForEmail);
                SendEmail(firstName, userName, phone, cartTableForEmail,ToEmail);


            }
            Session["cart"] = null;
            return View();
        }

        public ActionResult CartDropDown()
        {
            return View();
        }


        public void SendEmail(string FirstName, string UserName, string Phone, string CartTable,string ToEmail)
        {
            string fromEmail, toEmail, password, server;
            int port;
            bool isSslEnable;
            MailSettings mailmodel = new MailSettings();
            using (GerminmedContext db = new GerminmedContext())
            {

                mailmodel = db.MailSetting.FirstOrDefault();

            }

            if (mailmodel != null)
            {
                fromEmail = mailmodel.Username;
                toEmail = ToEmail;
                password = mailmodel.Password;
                server = mailmodel.Server;
                port = mailmodel.Port;
                isSslEnable = mailmodel.IsSSLEnabled;
            }
            else
            {
                fromEmail = ConfigurationManager.AppSettings["EmailFromAddress"];
                toEmail = ToEmail;
                password = ConfigurationManager.AppSettings["MailAuthPass"];
                server = ConfigurationManager.AppSettings["MailServer"];
                port = int.Parse(ConfigurationManager.AppSettings["Port"]);
                isSslEnable = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSSL"]);
            }


            try
            {

                using (MailMessage mm = new MailMessage(fromEmail, toEmail))
                {
                    mm.Subject = "GerminMed Product Order Information ";
                    mm.Body = CreateBody(FirstName, UserName, Phone, CartTable);
                    mm.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = server;
                    smtp.EnableSsl = isSslEnable;

                    NetworkCredential NetworkCred = new NetworkCredential(fromEmail, password);
                    smtp.UseDefaultCredentials = isSslEnable;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = port;
                    smtp.Send(mm);
                    ViewBag.Succcess = " Mail Send, Thank you for your Time.";

                }



            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error : " + ex.Message;

            }
        }


        public void SendEmail(string FirstName,string UserName,string Phone,string CartTable)
        {
            string fromEmail, toEmail, password, server;
            int port;
            bool isSslEnable;
            MailSettings mailmodel = new MailSettings();
            using (GerminmedContext db = new GerminmedContext())
            {

                mailmodel = db.MailSetting.FirstOrDefault();

            }

            if (mailmodel != null)
            {
                fromEmail = mailmodel.Username;
                toEmail = mailmodel.Contact;
                password = mailmodel.Password;
                server = mailmodel.Server;
                port = mailmodel.Port;
                isSslEnable = mailmodel.IsSSLEnabled;
            }
            else
            {
                fromEmail = ConfigurationManager.AppSettings["EmailFromAddress"];
                toEmail = ConfigurationManager.AppSettings["MailAuthUser"];
                password = ConfigurationManager.AppSettings["MailAuthPass"];
                server = ConfigurationManager.AppSettings["MailServer"];
                port = int.Parse(ConfigurationManager.AppSettings["Port"]);
                isSslEnable = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSSL"]);
            }


            try
            {

                using (MailMessage mm = new MailMessage(fromEmail, toEmail))
                {
                    mm.Subject = "GerminMed Product Order Information ";
                    mm.Body = CreateBody(FirstName,UserName,Phone,CartTable);
                    mm.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = server;
                    smtp.EnableSsl = isSslEnable;

                    NetworkCredential NetworkCred = new NetworkCredential(fromEmail, password);
                    smtp.UseDefaultCredentials = isSslEnable;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = port;
                    smtp.Send(mm);
                    ViewBag.Succcess = " Mail Send, Thank you for your Time.";

                }

               

            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error : " + ex.Message;
                
            }
        }

        private string CreateBody(string FirstName, string UserName, string Phone, string CartTable)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/AppFiles/Templates/ShoppingCartTemplate.html")))
            {

                body = reader.ReadToEnd();

            }

            body = body.Replace("{FirstName}", FirstName); //replacing Parameters
            body = body.Replace("{UserName}", UserName);
            body = body.Replace("{Phone}", Phone);
            body = body.Replace("{CartTable}", CartTable);
            return body;

        }



    }
}