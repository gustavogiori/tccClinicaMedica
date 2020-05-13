using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Giori_Consul.Services
{
    public class RetornoServiceUser
    {
        public bool Sucess { get; set; }
        public string MsgError { get; set; }

        public int IdUser { get; set; }
    }
}