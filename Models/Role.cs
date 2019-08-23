using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceDotNet.Models
{
    public class Role
    {
        public const string Admin = "Admin";
        public const string Customer = "Customer";
        public string[] GetAllRoles() { return new string[] { Admin, Customer }; }

    }
}
