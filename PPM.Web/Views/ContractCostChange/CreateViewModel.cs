using System;
using PensionInsurance.Entities;

namespace PensionInsurance.Web.Views.ContractCostChange
{
    public class CreateViewModel
    {
        /// <summary>
        /// 合同ID
        /// </summary>
        public int ContractId { get; set; }
        /// <summary>
        /// 费用变更补充协议编号
        /// </summary>
        public string ContractCostChangeNo { get; set; }
        /// <summary>
        /// 客户姓名
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 费用变更日期
        /// </summary>
        public DateTime ChangeDate { get; set; }
        /// <summary>
        /// 费用变更结束日期
        /// </summary>
        public DateTime ChangeEndDate { get; set; }
        /// <summary>
        /// 费用变更额度
        /// </summary>
        public decimal ChangeLimit { get; set; }

        public  ChangeType ChangeType { get; set; }
        public  decimal ChangeRoomCost { get; set; }
        public  decimal ChangeMealsCost { get; set; }
        public  decimal ChangeRatio { get; set; }
        public  bool IsIncluded { get; set; }
        /// <summary>
        /// 协议状态
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 附件文件路径
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 当前协议变更额度
        /// </summary>
        public decimal CurrentDiscount { get; set; }

        public decimal CustomerCurrentYearDiscount { get; set; }
        public decimal ProjectYearDiscount { get; set; }
    }
}