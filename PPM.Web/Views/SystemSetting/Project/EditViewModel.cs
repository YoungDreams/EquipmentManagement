using System.Collections.Generic;
using System.Web.Mvc;
using PensionInsurance.Commands;
using PensionInsurance.Entities;

namespace PensionInsurance.Web.Views.SystemSetting.Project
{
    public class EditViewModel : EditProjectCommand
    {
        public string HeaderText { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public IEnumerable<SelectListItem> Cities { get; set; }
        /// <summary>
        /// 管理区域
        /// </summary>
        public IEnumerable<SelectListItem> ManagementRegions { get; set; }
    }
}