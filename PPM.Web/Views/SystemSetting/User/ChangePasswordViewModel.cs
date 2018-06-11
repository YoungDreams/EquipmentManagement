using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PensionInsurance.Web.Views.SystemSetting.User
{
    public class ChangePasswordViewModel
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string OldPassword { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}