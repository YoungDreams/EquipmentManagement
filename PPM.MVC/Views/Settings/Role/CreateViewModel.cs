using PPM.Entities;

namespace PPM.MVC.Views.Settings.Role
{
    public class CreateViewModel
    {
        public string RoleName { get; set; }
        public RoleType RoleType { get; set; }
        public string Description { get; set; }
    }
}