using PensionInsurance.Entities;

namespace PensionInsurance.Web.Views.SystemSetting.Role
{
    public class EditViewModel
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public RoleType RoleType { get; set; }
        public string Description { get; set; }
    }
}