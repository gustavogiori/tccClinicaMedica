using Giori_Consul.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Giori_Consul.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private SqlDbContext db = new SqlDbContext();

        // GET: /Manage/Index
        public async Task<ActionResult> Index()
        {
            ViewBag.StatusMessage = "";

            //var userId = User.Identity.GetUserId();

            return View();
        }


        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var usuarioLogado = Session["UserId"].ToString();

            var user = db.Users.Where(x => x.UserId == usuarioLogado).FirstOrDefault();
            user.Password = model.NewPassword;
            if (user.Password != model.OldPassword)
            {
                TempData["Erro"] = "Senha atual inválida.";
                return View(model);
            }
            db.Entry(user).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            TempData["Sucess"] = "Senha alterada com sucesso!";

            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }
        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            Error
        }
    }
}