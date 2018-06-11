using System.Collections.Generic;
using System.Web.Mvc;

namespace PensionInsurance.Web.Views.SystemSetting.User
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
        public List<int> RoleIds { get; set; }
        public List<int> ProjectIds { get; set; }
        public bool IsEnabled { get; set; }
        public int? DepartmentId { get; set; }
        public IEnumerable<SelectListItem> Departments { get; set; }
        public IEnumerable<SelectListItem> Roles { get; set; }
        public IEnumerable<SelectListItem> Projects { get; set; }
    }
}