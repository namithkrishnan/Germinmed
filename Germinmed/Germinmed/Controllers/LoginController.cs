using Germinmed.DAL;
using Germinmed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Configuration;
using System.Net;
using System.Web.Helpers;

namespace Germinmed.Controllers
{
    public class LoginController : Controller
    {
       
        // GET: Login
        public ActionResult Index()
        {
            if (Request.UrlReferrer != null)
            {
                Session["controller"] = (Request.UrlReferrer.Segments.Skip(1).Take(1).SingleOrDefault() ?? "Home").Trim('/');

               Session["action"] = (Request.UrlReferrer.Segments.Skip(2).Take(1).SingleOrDefault() ?? "Index").Trim('/');
            }
            
            return View();
          
        }

        [HttpPost]
        public ActionResult Autherize(Users userModel)
        {
           
            var controller="";
            var action="";
            if (Session["controller"]!=null&& Session["action"]!=null)
            {
                 controller = Session["controller"].ToString();
                 action = Session["action"].ToString();
            }
             


            using (GerminmedContext db = new GerminmedContext())
            {
                var userDetails = db.User.Where(x => x.UserName == userModel.UserName && x.Password == userModel.Password && x.IsActive == true).FirstOrDefault();
                if (userDetails == null)
                {
                    userModel.LoginErrorMessage = "Wrong username or password.";
                    return View("Index", userModel);
                }
                else
                {
                    Session["userID"] = userDetails.Id;
                    Session["userName"] = userDetails.UserName;
                    Session["firstName"] = userDetails.FirstName;
                    
                        //if (controller != "")
                        //{ 
                        //    if(controller!="Login" && (controller!="User" && action !="Register"))
                        //    return RedirectToAction(action, controller);
                        //    else
                        //        return RedirectToAction("Index", "Home");
                        //}
                       // else
                            return RedirectToAction("Index", "Home");
                   
                    
                        



                }
            }
        }

               

        public ActionResult LogOut()
        {
            int userId = (int)Session["userID"];
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }





      

        public bool UserExists(string UserName)
        {
            try
            {

                using (GerminmedContext  db=new GerminmedContext())
                {
                    if(db.User.Where(x => x.UserName == UserName).FirstOrDefault()!=null)
                    {
                        return true;

                    }
                    else
                    {
                        return false;
                    }

                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }


        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            bool Status = false;
            using (GerminmedContext db = new GerminmedContext())
            {
                db.Configuration.ValidateOnSaveEnabled = false; // This line I have added here to avoid 
                                                                // Confirm password does not match issue on save changes
                var v = db.User.Where(a => a.ActivationCode == new Guid(id).ToString()).FirstOrDefault();
                if (v != null)
                {
                    v.IsEmailVerified = true;
                    db.SaveChanges();
                    Status = true;
                }
                else
                {
                    ViewBag.Message = "Invalid Request";
                }
            }
            ViewBag.Status = Status;
            return View();
        }


        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode, string emailFor = "ResetPassword")
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
                    " successfully created. Please click on the below link to verify your account" +
                    " <br/><br/><a href='" + link + "'>" + link + "</a> ";

            }
            else if (emailFor == "ResetPassword")
            {
                subject = "Reset Password";
                body = "Hi,<br/><br/>We got request for reset your account password. Please click on the below link to reset your password" +
                    "<br/><br/><a href=" + link + ">Reset Password link</a>";
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
        }


        public ActionResult ForgotPassword()
        {
            return View();
        }







        [HttpPost]
        public ActionResult ForgotPassword(string EmailID)
        {
            //Verify Email ID
            //Generate Reset password link 
            //Send Email 
            string message = "";
            bool status = false;

            using (GerminmedContext db = new GerminmedContext())
            {
                var account = db.User.Where(a => a.Email == EmailID).FirstOrDefault();
                if (account != null)
                {
                    //Send email for reset password
                    string resetCode = Guid.NewGuid().ToString();
                    SendVerificationLinkEmail(account.Email, resetCode, "ResetPassword");
                    account.ResetPasswordCode = resetCode;
                    //This line I have added here to avoid confirm password not match issue , as we had added a confirm password property 
                    //in our model class in part 1
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                    message = "Reset password link has been sent to your email id.";
                }
                else
                {
                    message = "Account not found";
                }
            }
            ViewBag.Message = message;
            return View();
        }


        public ActionResult ResetPassword(string id)
        {
            //Verify the reset password link
            //Find account associated with this link
            //redirect to reset password page
            if (string.IsNullOrWhiteSpace(id))
            {
                return HttpNotFound();
            }

            using (GerminmedContext db = new GerminmedContext())
            {
                var user = db.User.Where(a => a.ResetPasswordCode == id).FirstOrDefault();
                if (user != null)
                {
                    ResetPasswordModel model = new ResetPasswordModel();
                    model.ResetCode = id;
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            var message = "";
            if (ModelState.IsValid)
            {
                using (GerminmedContext db = new GerminmedContext())
                {
                    var user = db.User.Where(a => a.ResetPasswordCode == model.ResetCode).FirstOrDefault();
                    if (user != null)
                    {
                        user.Password = model.NewPassword;
                        user.ResetPasswordCode = "";
                        db.Configuration.ValidateOnSaveEnabled = false;
                        db.SaveChanges();
                        message = "New password updated successfully";
                    }
                }
            }
            else
            {
                message = "Something invalid";
            }
            ViewBag.Message = message;
            return View(model);
        }




    }
}