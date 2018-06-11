using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PensionInsurance.Web.Views.SystemSetting.RoomCost
{
    public class EditViewModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 长期费用
        /// </summary>
        public decimal LongTermCost { get; set; }
        /// <summary>
        /// 短期费用
        /// </summary>
        public decimal ShortTermCost { get; set; }
        /// <summary>
        /// 是否可用
        /// </summary>
        public bool IsEnabled { get; set; }
        public int? RoomTypeId { get; set; }

        public List<SelectListItem> RoomTypes { get; set; }
    }
}