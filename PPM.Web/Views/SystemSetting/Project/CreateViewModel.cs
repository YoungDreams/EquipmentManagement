using System.Collections.Generic;
using System.Web.Mvc;
using PensionInsurance.Entities;

namespace PensionInsurance.Web.Views.SystemSetting.Project
{
    public class CreateViewModel
    {
        /// <summary>
        /// 项目编号
        /// </summary>
        public string ProjectNo { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 项目地点
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 项目所属城市
        /// </summary>
        public int? City { get; set; }
        /// <summary>
        /// 管理区域
        /// </summary>
        public int? ManagementRegion { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 公司地址
        /// </summary>
        public string CompanyAddress { get; set; }
        /// <summary>
        /// 公司法人
        /// </summary>
        public string CompanyCorporation { get; set; }
        /// <summary>
        /// 公司电话
        /// </summary>
        public string CompanyTel { get; set; }
        /// <summary>
        /// 公司账户名称
        /// </summary>
        public string CompanyAccountName { get; set; }
        /// <summary>
        /// 公司账户
        /// </summary>
        public string CompanyAccount { get; set; }
        /// <summary>
        /// 公司账户开户行
        /// </summary>
        public string CompanyAccountBank { get; set; }
        /// <summary>
        /// 提供养老服务地点
        /// </summary>
        public string PensionAddress { get; set; }
        public string PostCode { get; set; }
        public string Email { get; set; }
        /// <summary>
        /// 项目状态
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 项目类型
        /// </summary>
        public ProjectType? ProjectType { get; set; }
        public int BuildingCount { get; set; }
        public int UnitCount { get; set; }
        public int FloorCount { get; set; }
        /// <summary>
        /// 刷卡手续费率
        /// </summary>
        public decimal PosCardRate { get; set; }
        /// <summary>
        /// 最大手续费
        /// </summary>
        public decimal MaxPosCardCost { get; set; }
        /// <summary>
        /// 特别减免限额
        /// </summary>
        public decimal SpecialExemptionLimit { get; set; }
        /// <summary>
        /// 项目全称
        /// </summary>
        public string ProjectFullName { get; set; }
        /// <summary>
        /// 积分额度
        /// </summary>
        public int IntegralLimit { get; set; }

        /// <summary>
        /// 一次性安置费
        /// </summary>
        public  decimal RelocationCost { get; set; }
        /// <summary>
        /// 短期餐费
        /// </summary>
        public  decimal ShortTermMealsCost { get; set; }
        /// <summary>
        /// 短期基础付费
        /// </summary>
        public  decimal ShortTermServiceCost { get; set; }
        /// <summary>
        /// 长期餐费
        /// </summary>
        public  decimal LongTermMealsCost { get; set; }
        /// <summary>
        /// 基础服务费（长期） 
        /// </summary>
        public  decimal LongTermServiceCost { get; set; }
        /// <summary>
        /// 请假退费
        /// </summary>
        public  decimal RefundCost { get; set; }
        /// <summary>
        /// 年减免额度总额
        /// </summary>
        public decimal SpecialExemptionLimitTotal { get; set; }
        public decimal PersonalSpecialExemptionLimitTotal { get; set; }
        public decimal LiquidatedDamagesRatio { get; set; }
        public int RecommendPoint { get; set; }
        /// <summary>
        /// 被推荐人奖励积分
        /// </summary>
        public int BonusPoint { get; set; }
        public decimal FirstDeposit { get; set; }
        public decimal SecondDeposit { get; set; }

        public string NCCompanyCode { get; set; }
        /// <summary>
        /// 借方科目编码
        /// </summary>
        public string NCDebitAccountCode { get; set; }
        /// <summary>
        /// 贷方房费科目编码
        /// </summary>
        public string NCCreditRoomAccountCode { get; set; }
        /// <summary>
        /// 贷方餐费科目编码
        /// </summary>
        public string NCCreditMealsAccountCode { get; set; }
        /// <summary>
        /// 贷方护理费科目编码
        /// </summary>
        public string NCCreditServiceAccountCode { get; set; }
        /// <summary>
        /// 贷方增值服务费科目编码
        /// </summary>
        public string NCCreditIncrementAccountCode { get; set; }
        /// <summary>
        /// 贷方基础服务费科目编码
        /// </summary>
        public string NCCreditBasicallyAccountCode { get; set; }
        /// <summary>
        /// 制单人
        /// </summary>
        public string NCEnter { get; set; }
        /// <summary>
        /// 借方一次性安置费结转科目编码
        /// </summary>
        public virtual string NCDebitRelocationAccountCode { get; set; }
        /// <summary>
        /// 贷方一次性安置费结转科目编码
        /// </summary>
        public virtual string NCCreditRelocationAccountCode { get; set; }
        /// <summary>
        /// 代办提醒日期
        /// </summary>
        public int ReminderDateOfDay { get; set; }

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