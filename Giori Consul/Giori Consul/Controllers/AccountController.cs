using Giori_Consul.Models;
using Giori_Consul.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace Giori_Consul.Controllers
{
    public class AccountController : BaseController
    {
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Usuario model)
        {
            if (ModelState.IsValid)
            {
                using (var context = new SqlDbContext())
                {
                    Usuario user = context.Users
                                       .Where(u => u.UserId == model.UserId && u.Password == model.Password)
                                       .FirstOrDefault();

                    return RegisterModel(model, user);
                }
            }
            else
            {
                return View(model);
            }
        }

        private ActionResult RegisterModel(Usuario model, Usuario user)
        {
            if (user != null)
            {
                CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel();
                serializeModel.RoleId = user.RoleId;

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                string userData = serializer.Serialize(serializeModel);

                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                         1,
                         user.UserId,
                         DateTime.Now,
                         DateTime.Now.AddMinutes(15),
                         false,
                         userData);

                string encTicket = FormsAuthentication.Encrypt(authTicket);
                HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                Response.Cookies.Add(faCookie);
                Session["UserName"] = user.UserName;
                Session["UserId"] = user.UserId;

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Invalid User Name or Password");
                TempData["Erro"] = "Usuário ou senha inválidos";
                return View(model);
            }
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(Usuario model)
        {
            if (ModelState.IsValid)
            {
                using (var context = new SqlDbContext())
                {
                    Usuario user = context.Users.Add(model);
                    context.SaveChanges();
                    return RegisterModel(model, user);
                }
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        [CustomAuthorize(RoleEnum.Admin)]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session["UserName"] = string.Empty;
            Session["UserId"] = string.Empty;
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie != null)
            {
                FormsAuthentication.SignOut();
                Request.Cookies.Clear();
            }
            return RedirectToAction("Index", "Home");
        }
    }
}