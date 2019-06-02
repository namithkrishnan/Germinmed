using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Germinmed.DAL;
using Germinmed.Models;


namespace Germinmed.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin

        public ActionResult Index()
        {


            return View();
        }

        public ActionResult UserProfile(int id = 0)
        {
            return View();
        }


        public ActionResult UserDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (GerminmedContext db = new GerminmedContext())
            {


                Users usr = db.User.Find(id);

                if (usr == null)
                {
                    return HttpNotFound();
                }
                return View(usr);


            }
        }

        public ActionResult Home()
        {
            DashBoardViewModel vm = new DashBoardViewModel();
            using (GerminmedContext db = new GerminmedContext())
            {
                vm.OrderCount = db.Orders.Count();
                vm.UserCount = db.User.Where(x => x.UserTypeId == 1).Count();

            }
            return View(vm);
        }

        public UserViewModel ChangePasswordModel(int id = 0)
        {

            UserViewModel vm = new UserViewModel();
            if (id != 0)
            {

                using (GerminmedContext db = new GerminmedContext())
                {
                    var userDetials = db.User.Where(x => x.Id == id).FirstOrDefault<Users>();
                    vm.Id = userDetials.Id;
                    vm.UserName = userDetials.UserName;

                }
            }
            return (vm);

        }


        public ActionResult ChangePassword(int id = 0)
        {

            UserViewModel vm = new UserViewModel();
            if (id != 0)
            {

                using (GerminmedContext db = new GerminmedContext())
                {
                    var userDetials = db.User.Where(x => x.Id == id).FirstOrDefault<Users>();
                    vm.Id = userDetials.Id;
                    vm.UserName = userDetials.UserName;

                }
            }
            return View(vm);

        }
        [HttpPost]
        public ActionResult ChangePassword(UserViewModel usr)
        {

            try
            {

                using (GerminmedContext db = new GerminmedContext())
                {
                    var currentItem = db.User.Where(b => b.Id == usr.Id).FirstOrDefault<Users>();
                    if (currentItem.Password == usr.OldPassword)
                    {
                        currentItem.Password = usr.Password;
                        //  currentItem.OldPassword = usr.OldPassword;
                        currentItem.ConfirmPassword = usr.ConfirmPassword;
                        db.Entry(currentItem).State = EntityState.Modified;
                        db.SaveChanges();

                        return Json(data: new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ChangePassword", model: usr), message = "Password changed Successfully" }, behavior: JsonRequestBehavior.AllowGet);

                    }
                    else
                    {

                        return Json(new { success = false, message = "Wrong Old password" }, JsonRequestBehavior.AllowGet);
                    }

                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }





        [HttpPost]
        public ActionResult Autherize(Users userModel)
        {

            using (GerminmedContext db = new GerminmedContext())
            {
                var userDetails = db.User.Where(x => x.UserName == userModel.UserName && x.Password == userModel.Password && (x.UserTypeId == 2 || x.UserTypeId == 3)).FirstOrDefault();
                if (userDetails == null)
                {
                    userModel.LoginErrorMessage = "Wrong username or password.";
                    return View("Index", userModel);
                }
                else
                {
                    Session["userID"] = userDetails.Id;
                    Session["userTypeId"] = userDetails.UserTypeId;
                    Session["userName"] = userDetails.UserName;
                    Session["firstName"] = userDetails.FirstName;
                    Session["CreatedDate"] = userDetails.CreatedDate.Value.ToString("MMM yyyy");

                    return RedirectToAction("Home", "Admin");

                }
            }
        }

        public ActionResult LogOut()
        {
            //int userId = (int)Session["userID"];
            if (Session != null)
            {
                Session.Abandon();
            }
            return RedirectToAction("Index", "Admin");
        }



        public MailSettings MailSettingsModel(int id = 0)
        {

            MailSettings vm = new MailSettings();
            if (id != 0)
            {

                using (GerminmedContext db = new GerminmedContext())
                {
                    vm = db.MailSetting.Where(x => x.Id == id).FirstOrDefault<MailSettings>();


                }
            }
            return (vm);

        }


        public ActionResult MailSettings(int id = 0)
        {

            MailSettings vm = new MailSettings();
            if (id != 0)
            {

                using (GerminmedContext db = new GerminmedContext())
                {
                    vm = db.MailSetting.Where(x => x.Id == id).FirstOrDefault<MailSettings>();

                }
            }
            return View(vm);

        }
        [HttpPost]
        public ActionResult MailSettings(MailSettings mail)
        {

            try
            {

                using (GerminmedContext db = new GerminmedContext())
                {
                    if (mail.Id == 0)
                    {
                        db.MailSetting.Add(mail);
                        db.SaveChanges();
                    }
                    else
                    {
                        db.Entry(mail).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    return Json(data: new { success = true, html = GlobalClass.RenderRazorViewToString(this, "MailSettings", model: mail), message = "Mail Settings submitted Successfully" }, behavior: JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Settings()
        {


            return View();
        }


    }
}