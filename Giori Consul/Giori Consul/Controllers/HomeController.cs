using Giori_Consul.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Giori_Consul.Controllers
{
    [CustomAuthenticationFilter]
    public class HomeController : BaseController
    {
        [CustomAuthorize(RoleEnum.Admin, RoleEnum.Secretaria, RoleEnum.Medico)]
        public ActionResult Index()
        {
            return View();
        }


        [CustomAuthorize(RoleEnum.Admin, RoleEnum.Medico)]
        public ActionResult About()
        {
            return View();
        }

        [CustomAuthorize(RoleEnum.Admin)]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


    }
}