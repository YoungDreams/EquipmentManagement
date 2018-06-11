using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PensionInsurance.Web.Views.SystemSetting.ManagementRegion
{
    public class CreateViewModel
    {
        /// <summary>
        /// 管理区域名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 城市名称
        /// </summary>
        public int AreaId { get; set; }
        /// <summary>
        /// 区域负责人
        /// </summary>
        public int? ManagementUserId { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        public List<SelectListItem> Users { get; set; }
        public List<SelectListItem> Cities { get; set; }
    }
}