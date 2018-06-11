using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PensionInsurance.Entities;
using PensionInsurance.Entities.DetailViews;

namespace PensionInsurance.Web.Views.Finance.Bill
{
    public class DetailViewModel
    {
        public CustomerBill Bill { get; set; }
        public IEnumerable<CustomerBill> Items { get; set; }
        public IEnumerable<ServiceRecordDetail> ServiceRecordsItems { get; set; }
        public decimal PayBackCostTotal { get; set; }
        public decimal PayBackRoomCostTotal { get; set; }
        /// <summary>
        /// 提前退住补缴餐费
        /// </summary>
        public decimal PayBackMealsCostTotal { get; set; }
        /// <summary>
        /// 提前退住补缴基础服务费
        /// </summary>
        public decimal PayBackServiceCostTotal { get; set; }
        /// <summary>
        /// 提前退住补缴打包服务费
        /// </summary>
        public decimal PayBackPackageServiceCostTotal { get; set; }
        /// <summary>
        /// 提前退住补缴请假费用
        /// </summary>
        public decimal PayBackRefundCostTotal { get; set; }
        /// <summary>
        /// 提前退住补缴变更费用
        /// </summary>
        public decimal PayBackChangeCostTotal { get; set; }

        //public decimal CurrentRoomCost { get; set; }
        //public decimal CurrentMealsCost { get; set; }
        //public decimal CurrentServiceCost { get; set; }
        //public decimal CurrentPackageServiceCost { get; set; }

        public decimal PayBackRoomChangeCostTotal { get; set; }
        public decimal PayBackMealChangeCostTotal { get; set; }
        public decimal PayBackLevaPackageServiceCostTotal { get; set; }


    }
}