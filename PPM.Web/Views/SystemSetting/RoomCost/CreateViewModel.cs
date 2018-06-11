using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using PensionInsurance.Entities;

namespace PensionInsurance.Web.Views.SystemSetting.RoomCost
{
    public class CreateViewModel
    {
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

        public List<SelectListItem> RoomTypes { get; set; }
    }
}