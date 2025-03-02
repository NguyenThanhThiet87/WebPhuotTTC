using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebPhuotTTC.Models
{
    public class UpdateUser
    {
        public class UserUpdateModel
        {
            public string HoTen { get; set; }
            public string SDT { get; set; }
            public string DiaChi { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }
        }
    }
}