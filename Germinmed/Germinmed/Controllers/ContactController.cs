using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Germinmed.DAL;
using Germinmed.Models;

namespace Germinmed.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult Index()
        {

            using (GerminmedContext db = new GerminmedContext())
            {
                InnerBanner bnr = new InnerBanner();
                ViewBag.InnerBanner = db.InnerBanners.Where(x => x.PageName == "ContactUs").FirstOrDefault();
            }

            return View("Index");
        }
        [HttpPost]
        public ActionResult Index(string name, string email, string phone, string message)
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

                mm.Subject = "GerminMed Contact Customer Information ";
                mm.Body = CreateBody(name,email,phone,message);
                mm.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = server;
                smtp.EnableSsl = isSslEnable;

                NetworkCredential NetworkCred = new NetworkCredential(fromEmail,password);
                smtp.UseDefaultCredentials = isSslEnable;
                smtp.Credentials = NetworkCred;
                smtp.Port = port;
                smtp.Send(mm);
                    ViewBag.Succcess = "Contact Mail Send, Thank you for your Time.";

                }
               
                return View();

            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error : " + ex.Message;
                return View();
            }
        }

        private string CreateBody(string name, string email, string phone, string message)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/AppFiles/Templates/ContactTemplate.html")))
            {

                body = reader.ReadToEnd();

            }

            body = body.Replace("{name}", name); //replacing Parameters

            body = body.Replace("{email}", email);

            body = body.Replace("{phone}", phone);
            body = body.Replace("{message}", message);
           

            return body;

        }

    }
}