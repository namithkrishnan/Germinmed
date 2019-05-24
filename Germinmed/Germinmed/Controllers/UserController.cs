using Germinmed.DAL;
using Germinmed.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Germinmed.Controllers
{
    public class UserController : Controller
    {


        public ActionResult Index()
        {
            return View();
        }
        public ActionResult RegisteredUsers()
        {
            return View();
        }


        public ActionResult ViewAll()
        {
            return View(GetAll());

        }
        IEnumerable<Users> GetAll()
        {
            using (GerminmedContext db = new GerminmedContext())
            {
                return db.User.Where(x=>x.UserTypeId==2 || x.UserTypeId ==3)   .ToList<Users>();


            }

        }


        public ActionResult ViewAllRegisteredUser()
        {
            return View(GetAllRegisteredUser());

        }
        IEnumerable<Users> GetAllRegisteredUser()
        {
            using (GerminmedContext db = new GerminmedContext())
            {
                return db.User.Where(x => x.UserTypeId == 1).ToList<Users>();


            }

        }

        public ActionResult AddOrEdit(int id = 0)
        {
            Users usr = new Users();
            if (id != 0)
            {


                using (GerminmedContext db = new GerminmedContext())
                {

                    usr = db.User.Where(x => x.Id == id).FirstOrDefault<Users>();
                }
            }

            return View(usr);

        }

        [HttpPost]
        public ActionResult AddOrEdit(Users usr)
        {
            try
            {

                using (GerminmedContext db = new GerminmedContext())
                {
                    if (usr.Id == 0)
                    {

                        if (db.User.Any(x => x.UserName == usr.UserName))
                        {
                           
                            return Json(new { success = false, message = "Username already exist." }, JsonRequestBehavior.AllowGet);
                        }
                        db.User.Add(usr);
                        db.SaveChanges();
                    }
                    else
                    {
                        var currentItem = db.User.Where(b => b.Id == usr.Id).FirstOrDefault<Users>();
                        currentItem.UserName = usr.UserName;
                        currentItem.FirstName = usr.FirstName;
                        currentItem.Email = usr.Email;
                        currentItem.IsAdmin = usr.IsAdmin;
                        currentItem.ConfirmPassword = currentItem.Password;
                        if (db.User.Any(x => x.UserName == currentItem.UserName && x.Id!=currentItem.Id))
                        {
                           
                            return Json(new { success = false, message = "Username already exist." }, JsonRequestBehavior.AllowGet);
                        }

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

        public ActionResult Delete(int Id)
        {
            try
            {
                using (GerminmedContext db = new GerminmedContext())
                {
                    Users usr = db.User.Where(x => x.Id == Id).FirstOrDefault<Users>();
                    
                    db.User.Remove(usr);
                    db.SaveChanges();
                }
                return Json(data: new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", model: GetAll()), message = "Deleted Successfully" }, behavior: JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult DeleteRegUser(int Id)
        {
            try
            {
                using (GerminmedContext db = new GerminmedContext())
                {
                    Users usr = db.User.Where(x => x.Id == Id).FirstOrDefault<Users>();

                    db.User.Remove(usr);
                    db.SaveChanges();
                }
                return Json(data: new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAllRegisteredUser", model: GetAllRegisteredUser()), message = "Deleted Successfully" }, behavior: JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult Approve(int Id)
        {
            try
            {
                using (GerminmedContext db = new GerminmedContext())
                {
                    db.Configuration.ValidateOnSaveEnabled = false;
                    Users usr = db.User.Where(x => x.Id == Id).FirstOrDefault<Users>();
               

                                                                             
                    if(usr.IsActive==false)
                    {
                        usr.IsActive = true;
                    }
                                                         
                    // 
                    //entities.Entry(user).Property(u => u.LastActivity).IsModified = true;
                    //entities.SaveChanges();

                    db.User.Attach(usr);
                    db.Entry(usr).Property(x => x.IsActive).IsModified = true;
                    db.SaveChanges();
                    SendVerificationLinkEmail(usr.Email, "", "Approve");
                }
               
                return Json(data: new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAllRegisteredUser", model: GetAllRegisteredUser()), message = "Updated Successfully" }, behavior: JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        
        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode, string emailFor = "VerifyAccount")
        {
            var verifyUrl = "/Login/" + emailFor + "/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = "";
            var toEmail = "";
            var fromEmailPassword = "";
            string server = "";
            int port = 587;
            bool isSslEnable = true;


            using (GerminmedContext db = new GerminmedContext())
            {
                MailSettings mailmodel = new MailSettings();
                mailmodel = db.MailSetting.FirstOrDefault();



                if (mailmodel != null)
                {

                    fromEmail = mailmodel.Username;
                    toEmail = emailID;
                    fromEmailPassword = mailmodel.Password; // Replace with actual password

                    server = mailmodel.Server;
                    port = mailmodel.Port;
                    isSslEnable = mailmodel.IsSSLEnabled;
                }
                else
                {
                    fromEmail = ConfigurationManager.AppSettings["EmailFromAddress"];
                    toEmail = emailID;
                    fromEmailPassword = ConfigurationManager.AppSettings["MailAuthPass"]; // Replace with actual password

                    server = ConfigurationManager.AppSettings["MailServer"];
                    port = int.Parse(ConfigurationManager.AppSettings["Port"]);
                    isSslEnable = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSSL"]);
                }

            }



            string subject = "";
            string body = "";
            if (emailFor == "VerifyAccount")
            {

                subject = "Your account is successfully created!";
                body = "<br/><br/>We are excited to tell you that your Germinmed account is" +
                    " successfully created. Please click on the below link to verify your Email." +
                    " You can login only after approval from our support team." +
                    " <br/><br/><a href='" + link + "'>" + link + "</a> ";

            }
            else if (emailFor == "ResetPassword")
            {
                subject = "Reset Password";
                body = "Hi,<br/><br/>We got request for reset your account password. Please click on the below link to reset your password" +
                    "<br/><br/><a href=" + link + ">Reset Password link</a>";
            }
            else if (emailFor == "Approve")
            {
                subject = "Account Approved";
                body = "Hi,<br/><br/>Your account is activated now you can Login,please click on the below link to Login" +
                    "<br/><br/><a href='" + Request.Url.Host + Request.ApplicationPath + "/Login'>Login link</a>";
            }


            var smtp = new SmtpClient
            {
                Host = server,
                Port = port,
                EnableSsl = isSslEnable,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
            SendEmailToAdminNewReg();
        }


        [NonAction]
        public void SendEmailToAdminNewReg()
        {
            //var verifyUrl = "/Login/" + emailFor + "/" + activationCode;
           // var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = "";
            var toEmail = "";
            var fromEmailPassword = "";
            string server = "";
            int port = 587;
            bool isSslEnable = true;


            using (GerminmedContext db = new GerminmedContext())
            {
                MailSettings mailmodel = new MailSettings();
                mailmodel = db.MailSetting.FirstOrDefault();



                if (mailmodel != null)
                {

                    fromEmail = mailmodel.Username;
                    toEmail = mailmodel.Contact;
                    fromEmailPassword = mailmodel.Password; // Replace with actual password

                    server = mailmodel.Server;
                    port = mailmodel.Port;
                    isSslEnable = mailmodel.IsSSLEnabled;
                }
                else
                {
                    fromEmail = ConfigurationManager.AppSettings["MailAuthUser"];
                    toEmail = ConfigurationManager.AppSettings["EmailFromAddress"]; 
                    fromEmailPassword = ConfigurationManager.AppSettings["MailAuthPass"]; // Replace with actual password

                    server = ConfigurationManager.AppSettings["MailServer"];
                    port = int.Parse(ConfigurationManager.AppSettings["Port"]);
                    isSslEnable = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSSL"]);
                }

            }



            string subject = "";
            string body = "";
         
                subject = "GerminMed ::New User Registration";
                body = "<br/><br/>New User registered to germinmed.com " +
                " Please go to germinmed admin panel and approve the account request.";
               
                    

         


            var smtp = new SmtpClient
            {
                Host = server,
                Port = port,
                EnableSsl = isSslEnable,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }



        #region User Registration Client Side GET
        [HttpGet]
        public ActionResult Register(int id = 0)
        {
            Users userModel = new Users();
            return View(userModel);
        }
        #endregion

        #region User Registration Client side POST
        [HttpPost]
        public ActionResult Register(Users userModel)
        {
            using (GerminmedContext db = new GerminmedContext())
            {
                userModel.IsAdmin = false;
               
                userModel.ActivationCode = Guid.NewGuid().ToString();
                userModel.IsEmailVerified = false;
                userModel.IsActive = false;
                if (db.User.Any(x => x.UserName == userModel.UserName))
                {
                    ViewBag.DuplicateMessage = "Username already exist.";
                    return View("Register", userModel);
                }


                db.User.Add(userModel);
                db.SaveChanges();

                SendVerificationLinkEmail(userModel.Email, userModel.ActivationCode.ToString(), "VerifyAccount");
                ViewBag.SuccessMessage = "Germin MED Welcome you to Our online shopping. Email verification link " +
                    " has been sent to your email :" + userModel.Email;
            }
            ModelState.Clear();
           // ViewBag.SuccessMessage = "Registration Successful.";
            return View("Success");
        }
        #endregion



        public ActionResult Success()
        {
            return View();
        }
    }
}