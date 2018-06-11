using System.Collections.Generic;
using System.Web.Mvc;
using PPM.Entities;

namespace PPM.MVC.Views.Settings.User
{
    public class EditViewModel
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public string RealName { get; set; }
        public string Phone { get; set; }
        public List<string> RoleIds { get; set; }
        public RoleType RoleType { get; set; }
        public bool IsEnabled { get; set; }
        public IEnumerable<SelectListItem> Roles { get; set; }
    }
}