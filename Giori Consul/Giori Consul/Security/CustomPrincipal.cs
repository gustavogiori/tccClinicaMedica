using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Giori_Consul.Security
{
    public class CustomPrincipal : ICustomPrincipal
    {
        public IIdentity Identity { get; private set; }
        public bool IsInRole(string role)
        {
            if (Convert.ToInt32(role) == RoleId)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public CustomPrincipal(string email)
        {

            this.Identity = new GenericIdentity(email);
        }

        public int RoleId { get; set; }
    }
}