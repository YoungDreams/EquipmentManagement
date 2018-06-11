using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PensionInsurance.Web.Views.SystemSetting.Department
{
    public class CreateViewModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int ProjectId { get; set; }
        public int? ManagementUserId { get; set; }
        public int Sort { get; set; }
        public List<SelectListItem> Projects { get; set; }
    }
}