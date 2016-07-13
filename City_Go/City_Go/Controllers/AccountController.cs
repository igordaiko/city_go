using City_Go.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using City_Go.Filters;
using WebMatrix.WebData;

namespace City_Go.Controllers
{
    [Authorize]
    [InitializeSimpleMemberShip]
    public class AccountController : Controller
    {
        [AllowAnonymous]
        public ActionResult Login(string returnurl)
        {
            ViewBag.ReturnUrl = returnurl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnurl)
        {
            if(ModelState.IsValid && WebSecurity.Login(model.Login, model.Password, persistCookie: model.Remember))
            {
                if (Url.IsLocalUrl(returnurl))
                {
                    return Redirect(returnurl);
                }
                else
                {
                    return RedirectToAction("Index", "Admin");
                }
            }

            ModelState.AddModelError("", "Введены некоректные данные");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    WebSecurity.CreateUserAndAccount(model.Login, model.Password);
                    WebSecurity.Login(model.Login, model.Password);
                    return RedirectToAction("Index", "Admin");
                }
                catch (Exception)
                {

                    throw;
                }
               

            }
            return View(model);
        }
    }
}