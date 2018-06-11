using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using PensionInsurance.Entities;

namespace PensionInsurance.Web.Views.SystemSetting.RoomType
{
    public class CreateViewModel
    {
        /// <summary>
        /// 房型名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// 房屋类型
        /// </summary>
        public string HouseType { get; set; }
        /// <summary>
        /// 床位数
        /// </summary>
        public int BedCount { get; set; }

        public List<SelectListItem> Projects { get; set; }
        public List<SelectListItem> HouseTypes { get; set; }
    }
}