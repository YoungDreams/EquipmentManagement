using System;

namespace PensionInsurance.Web.Views.ContractServicePackChange
{
    public class CreateViewModel
    {
        /// <summary>
        /// 项目ID
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// 合同ID
        /// </summary>
        public int ContractId { get; set; }
        public int CustomerAccountId { get; set; }
        /// <summary>
        /// 服务包变更补充协议编号
        /// </summary>
        public string ContractServicePackChangeNo { get; set; }
        /// <summary>
        /// 客户姓名
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 服务包变更日期
        /// </summary>
        public DateTime ChangeDate { get; set; }
        /// <summary>
        /// 服务包变更结束日期
        /// </summary>
        public DateTime ChangeEndDate { get; set; }
        /// <summary>
        /// 生活能力评估级别
        /// </summary>
        public string ConcernType { get; set; }
        /// <summary>
        /// 生活照料服务包等级
        /// </summary>
        public string ServiceLevel { get; set; }
        /// <summary>
        /// 生活照料服务包服务项明细
        /// </summary>
        public string ServicePakgeDetail { get; set; }
        /// <summary>
        /// 可选服务包明细
        /// </summary>
        public string OptionServicePakgeDetail { get; set; }
        /// <summary>
        /// 乙方应缴纳生活照料服务包月费（短期）
        /// </summary>
        public decimal ShortServiceMonthlyAmount { get; set; }
        /// <summary>
        /// 乙方应缴失智照护费（短期）
        /// </summary>
        public decimal ShortConcernAmount { get; set; }
        /// <summary>
        /// 乙方可选服务包月费（短期）
        /// </summary>
        public decimal ShortMonthlyAmount { get; set; }
        /// <summary>
        /// 乙方应缴纳生活照料服务包月费（长期）
        /// </summary>
        public decimal LongServiceMonthlyAmount { get; set; }
        /// <summary>
        /// 乙方应缴失智照护费（长期） 
        /// </summary>
        public decimal LongConcernAmount { get; set; }
        /// <summary>
        /// 乙方可选服务包月费（短期）
        /// </summary>
        public decimal LongMonthlyAmount { get; set; }
        /// <summary>
        /// 协议状态
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 附件文件路径
        /// </summary>
        public string FilePath { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 长期附加费用
        /// </summary>
        public decimal LongAttachAmount { get; set; }
        /// <summary>
        /// 短期附加费用
        /// </summary>
        public decimal ShortAttachAmount { get; set; }

        public Entities.Contract Contarct { get; set; }
    }
}