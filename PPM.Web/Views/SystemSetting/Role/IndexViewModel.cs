using Foundation.Data;
using PensionInsurance.Query;

namespace PensionInsurance.Web.Views.SystemSetting.Role
{
    public class IndexViewModel
    {
        public RoleQuery Query { get; set; }
        public PagedData<Entities.Role> Items { get; set; }
    }
}