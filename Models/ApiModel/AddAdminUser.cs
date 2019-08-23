using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceDotNet.Models.API
{
    public class AddAdminUserRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class AddCustomerUserRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class DeleteUserRequest
    {
        public string Username { get; set; }
    }
}
