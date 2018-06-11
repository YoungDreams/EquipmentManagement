using System.Collections.Generic;
using System.Web.Mvc;

namespace PensionInsurance.Web.Views.SystemSetting.Grouping
{
    public class EditViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int? ParentId { get; set; }
        public int Sort { get; set; }
        public int DepartmentId { get; set; }
        public Dictionary<GroupingSelectListItem, List<GroupingSelectListItem>> Groupings { get; set; }
        public List<SelectListItem> Projects { get; set; }
        public List<SelectListItem> Departments { get; set; }
    }
}