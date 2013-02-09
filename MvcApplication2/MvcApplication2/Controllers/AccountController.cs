using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MvcApplication2.Models;

namespace MvcApplication2.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }


        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(UserLogon model, string returnUrl)
        {
            
            try
            {
                MembershipUser user = Membership.CreateUser(model.UserName, model.Password);
                if (!Roles.GetAllRoles().Contains("Admin"))
                {
                    Roles.CreateRole("Admin");
                }
                Roles.AddUserToRole(model.UserName,"Admin");
            }
            catch (Exception e )
            {
                ModelState.AddModelError("", "Can't add user!!.");
                return View("Add", model);
            }
            return RedirectToAction("Login");

        }
        public ActionResult Login()
        {
            return View();
        }
        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(UserLogon model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, true);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                       
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Articles", new { area = "Admin" });
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }
            return RedirectToAction("Login", model);

            // If we got this far, something failed, redisplay form
        }
        public ActionResult Error()
        {
            return View();
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");

        }
    }
}
