using System.Collections.Generic;
using System.Web.Mvc;
using PensionInsurance.Entities;

namespace PensionInsurance.Web.Views.SystemSetting.Grouping
{
    public class CreateViewModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int? ParentId { get; set; }
        public int Sort { get; set; }
        public int Layer { get; set; }
        public int DepartmentId { get; set; }
        public Dictionary<GroupingSelectListItem, List<GroupingSelectListItem>> Organizations { get; set; }
        public List<SelectListItem> Projects { get; set; }
        public List<SelectListItem> Departments { get; set; }
    }

    public class GroupingSelectListItem : SelectListItem
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int? ParentId { get; set; }
    }
}