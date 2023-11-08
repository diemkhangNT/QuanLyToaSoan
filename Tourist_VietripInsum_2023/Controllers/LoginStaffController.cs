using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Tourist_VietripInsum_2023.Models;

namespace Tourist_VietripInsum_2023.Controllers
{
    public class LoginStaffController : Controller
    {
        QuanLyToaSoanEntities database = new QuanLyToaSoanEntities();
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var nb = "NB"; var dt = "DT"; var cb = "CB";var ad = "AD";

            var data = database.NhanViens.Where(s => s.Username == username && s.UserPassword == password).FirstOrDefault();
            var taikhoan = database.NhanViens.SingleOrDefault(s => s.Username == username && s.UserPassword == password);
            if (taikhoan == null)
            {
                TempData["error"] = "err";
                return View("Login");
            }
            else if (taikhoan != null)
            {
                database.Configuration.ValidateOnSaveEnabled = false;
                Session["user"] = taikhoan;
                var role = data.MaCV.ToString();
                if (role == nb)
                {
                    TempData["AlertMessage"] = "Login sucess";
                    return RedirectToAction("TrangChuNhaBao", "NhaBao");
                }
                else if (role == dt)
                {
                    TempData["AlertMessage"] = "Login sucess";
                    return RedirectToAction("", "");
                }
                else if (role == cb)
                {
                    TempData["AlertMessage"] = "Login sucess";
                    return RedirectToAction("Index", "ChuBien");
                }
                else if (role == ad)
                {
                    TempData["AlertMessage"] = "Login sucess";
                    return RedirectToAction("HomePage", "Admin");
                }

            }
            else
            {
                TempData["AlertMessage"] = "Login error";
                return RedirectToAction("Login", "LoginStaff");
            }
            return View();
        }
        public ActionResult ForgotPassword()
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();//remove session
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}