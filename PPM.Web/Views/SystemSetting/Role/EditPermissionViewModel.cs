using System.Collections.Generic;
using PensionInsurance.Entities;

namespace PensionInsurance.Web.Views.SystemSetting.Role
{
    public class EditPermissionViewModel
    {
        public int RoleId { get; set; }
        public IList<RolePermission> RolePermissions { get; set; }
    }
}