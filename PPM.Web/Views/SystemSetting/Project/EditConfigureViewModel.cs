using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PensionInsurance.Entities;
using PensionInsurance.Entities.DetailViews;

namespace PensionInsurance.Web.Views.SystemSetting.Project
{
    public class EditConfigureViewModel
    {
        public int ProjectId { get; set; }
        public ConfigureCategory Category { get; set; }
        public IEnumerable<SelectListItem> Users { get; set; }
        public IEnumerable<SelectListItem> UserSelectListItems { get; set; }
    }
}