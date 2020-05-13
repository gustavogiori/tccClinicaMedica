using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Giori_Consul.Security
{
    public interface ICustomPrincipal : IPrincipal
    {
        int RoleId { get; set; }
    }
}
