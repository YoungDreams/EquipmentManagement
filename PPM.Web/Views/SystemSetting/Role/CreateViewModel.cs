using PensionInsurance.Entities;

namespace PensionInsurance.Web.Views.SystemSetting.Role
{
    public class CreateViewModel
    {
        public string RoleName { get; set; }
        public RoleType RoleType { get; set; }
        public string Description { get; set; }
    }
}