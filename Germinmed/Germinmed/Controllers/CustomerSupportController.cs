using Germinmed.DAL;
using Germinmed.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Germinmed.Controllers
{
    public class CustomerSupportController : Controller
    {
        // GET: CustomerSupport
        public ActionResult Index()
        {
            using (GerminmedContext db = new GerminmedContext())
            {
                InnerBanner bnr = new InnerBanner();
                ViewBag.InnerBanner = db.InnerBanners.Where(x => x.PageName == "CustomerSupport").FirstOrDefault();
            }

                return View("Index");
        }


        [HttpPost]
        public ActionResult Index(string Name,string Email,string Contact,string Message, HttpPostedFileBase ImageUpload)
        {
            string fromEmail, toEmail, password, server;
            int port;
            bool isSslEnable;
            MailSettings mailmodel = new MailSettings();
            using (GerminmedContext db=new GerminmedContext())
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

                using (MailMessage mm = new MailMessage(fromEmail,toEmail))
                {
                    mm.Subject = "GerminMed Technical Support Information ";
                    mm.Body = CreateBody(Name, Email, Contact, Message);
                    mm.IsBodyHtml = true;
                    if (ImageUpload != null)
                    {

                        mm.Attachments.Add(new Attachment(ImageUpload.InputStream, ImageUpload.FileName));
                    }
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = server;
                    smtp.EnableSsl = isSslEnable;

                    NetworkCredential NetworkCred = new NetworkCredential(fromEmail,password);
                    smtp.UseDefaultCredentials = isSslEnable;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = port;
                    smtp.Send(mm);
                    ViewBag.Succcess = "Support Mail Send, Thank you for your Time.";

                }

                return View();

            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error : " + ex.Message;
                return View();
            }
        }

        private string CreateBody(
        string Name,
        string Email,
        string Contact,
        string Message
       
      )
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/AppFiles/Templates/CustomerSupportTemplate.html")))
            {

                body = reader.ReadToEnd();

            }

            body = body.Replace("{Name}", Name); //replacing Parameters
            body = body.Replace("{Email}", Email);
            body = body.Replace("{Contact}", Contact);
            body = body.Replace("{Message}", Message);
            
          


            return body;

        }

    }
}