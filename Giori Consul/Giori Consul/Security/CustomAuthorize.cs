using Giori_Consul.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Giori_Consul.Security
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly RoleEnum[] allowedroles;
        public CustomAuthorizeAttribute(params RoleEnum[] roles)
        {
            this.allowedroles = roles;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            var userId = Convert.ToString(httpContext.Session["UserId"]);
            if (!string.IsNullOrEmpty(userId))
                using (var context = new SqlDbContext())
                {
                    var userRole = (from u in context.Users
                                    where u.UserId == userId
                                    select new
                                    {
                                        u.RoleId
                                    }).FirstOrDefault();
                    if (userRole != null)
                    {
                        foreach (var role in allowedroles)
                        {
                            if ((int)role == userRole.RoleId) return true;
                        }
                    }
                }


            return authorize;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
               new RouteValueDictionary
               {
                    { "controller", "Home" },
                    { "action", "UnAuthorized" }
               });
        }
    }
}