using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PensionInsurance.Web.Views.SystemSetting.RoomType
{
    public class EditViewModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
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