using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Germinmed.Models;
using Germinmed.DAL;
using System.IO;
using System.Data.Entity;

using System.Data;
using System.Net.Mail;
using System.Configuration;
using System.Net;

namespace Germinmed.Controllers
{
    public class NewsLetterController : Controller
    {
        // GET: NewsLetter
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewAll()
        {
            return View(GetAll());

        }
        IEnumerable<NewsLetter> GetAll()
        {
            using (GerminmedContext db = new GerminmedContext())
            {
                return db.NewsLetters.ToList<NewsLetter>();

            }

        }

        public ActionResult AddOrEdit(int id = 0)
        {

            NewsLetter news = new NewsLetter();

            if (id != 0)
            {


                using (GerminmedContext db = new GerminmedContext())
                {

                    news = db.NewsLetters.Where(x => x.Id == id).FirstOrDefault<NewsLetter>();
                    news.UserList = db.User.Where(x => x.UserTypeId == 1).ToList();
                }
            }
            else
            {
                using (GerminmedContext db = new GerminmedContext())
                {
                    news.UserList = db.User.Where(x => x.UserTypeId == 1).ToList();
                }
            }
            return View(news);

        }

        [HttpPost]
        public ActionResult AddOrEdit(NewsLetter news)
        {
            try
            {
                HttpPostedFileBase attach = null;
                string fileName="";
                using (GerminmedContext db = new GerminmedContext())
                {
                    news.UserList = db.User.Where(x => x.UserTypeId == 1).ToList();
                    if (news.Id == 0)
                    {

                        if (news.ImageUpload != null)
                        {
                            attach = news.ImageUpload;
                             fileName = Path.GetFileNameWithoutExtension(news.ImageUpload.FileName);
                            string extension = Path.GetExtension(news.ImageUpload.FileName);
                            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                            news.ImageUrl = "~/AppFiles/Images/" + fileName;
                            news.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/AppFiles/Images/"), fileName));

                        }

                        sendEmail(news.Subject,news.Message,news.Recipients, attach, fileName);

                        db.NewsLetters.Add(news);
                        db.SaveChanges();
                    }
                    else
                    {
                        var currentItem = db.NewsLetters.Where(b => b.Id == news.Id).FirstOrDefault<NewsLetter>();
                        currentItem.Message = news.Message;
                        currentItem.Message = news.Message;
                        currentItem.Recipients = news.Recipients;
                        sendEmail(news.Subject, news.Message, news.Recipients,attach,fileName);

                        db.Entry(currentItem).State = EntityState.Modified;
                        db.SaveChanges();
                    }


                }
                return Json(data: new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", model: GetAll()), message = "Submitted Successfully" }, behavior: JsonRequestBehavior.AllowGet);

                //RedirectToAction("ViewAll");
            }
            catch (DataException ex)
            {

                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }


        public void sendEmail(string subject,string messgaeBody,string recipients,HttpPostedFileBase attach,string filename)
        {

            string fromEmail, password, server;
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
              
                password = mailmodel.Password;
                server = mailmodel.Server;
                port = mailmodel.Port;
                isSslEnable = mailmodel.IsSSLEnabled;
            }
            else
            {
                fromEmail = ConfigurationManager.AppSettings["EmailFromAddress"];
               
                password = ConfigurationManager.AppSettings["MailAuthPass"];
                server = ConfigurationManager.AppSettings["MailServer"];
                port = int.Parse(ConfigurationManager.AppSettings["Port"]);
                isSslEnable = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSSL"]);
            }


            try
            {

                using (MailMessage mm = new MailMessage(fromEmail, recipients))
            {

                
                mm.Subject = (subject != null && subject != "") ? subject : "GerminMed News Letter ";
                mm.Body = messgaeBody;                  //  CreateBody(name, email, phone, message);
                mm.IsBodyHtml = true;

                    if (attach != null)
                    {
                      
                        mm.Attachments.Add(new Attachment(attach.InputStream, filename));
                    }
                    SmtpClient smtp = new SmtpClient();
                smtp.Host = server;
                smtp.EnableSsl = isSslEnable;

                NetworkCredential NetworkCred = new NetworkCredential(fromEmail, password);
                   
                smtp.UseDefaultCredentials = isSslEnable;
                smtp.Credentials = NetworkCred;
                smtp.Port = port;
                smtp.Send(mm);
                ViewBag.Succcess = "Mail Send, Thank you for your Time.";

            }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error : " + ex.Message;
              
            }

        }



        public ActionResult Delete(int Id)
        {
            try
            {
                using (GerminmedContext db = new GerminmedContext())
                {
                    NewsLetter news = db.NewsLetters.Where(x => x.Id == Id).FirstOrDefault<NewsLetter>();
                    string fullPath = Request.MapPath(news.ImageUrl);
                    if (news.ImageUrl != "~/AppFiles/Images/Default.png")
                    {
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                    }
                    db.NewsLetters.Remove(news);
                    db.SaveChanges();
                }
                return Json(data: new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", model: GetAll()), message = "Deleted Successfully" }, behavior: JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


    }
}