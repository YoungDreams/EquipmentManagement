using PensionInsurance.Entities;
using PensionInsurance.Entities.CustomerHealthItem;
using PensionInsurance.Entities.DetailViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PensionInsurance.Web.Views.HealthManagement
{
    public class HealthMonitoringViewModel
    {
        public HealthMonitoringViewModel()
        {
            HMReportModel = new HealthManageReport();
        }
        //健康状态（用于标识 去评估2或去查看1）
        public int HealthReportStatus { get; set; }
        /// <summary>
        /// 指标项名称
        /// </summary>
        public string DetailTypeName { get; set; }
        /// <summary>
        /// 标识是入住客户还是退住客户（-1 退住，4入住）
        /// </summary>
        public string CheckStatus { get; set; }
        /// <summary>
        /// 客户基本信息实体
        /// </summary>
        public CustomerBasicInfo CustomerInfo { get; set; }
        /// <summary>
        /// 血压指标数据列表
        /// </summary>
        public DetailForBloodPresureHistories CustomerHealthForBloodPresureInfo { get; set; }
        /// <summary>
        /// 单个指标项数据列表（用户画图）
        /// </summary>
        public HealthDetails HealthDetailInfo { get; set; }
        /// <summary>
        /// 健康报告列表
        /// </summary>
        public List<HealthManageReport> HMReportList { get; set; }
        /// <summary>
        /// 编辑状态，记录是从健康指标项首页或其他页面的编辑按钮进入还是tab进入健康报告页面
        /// </summary>
        public int EditStatus { get; set; }
        /// <summary>
        /// 健康报告实体
        /// </summary>
        public HealthManageReport HMReportModel { get; set; }
        /// <summary>
        /// 健康指标项数据列表
        /// </summary>
        public List<HealthManageInfoViewModel> HMVList { get; set; }
    }
    /// <summary>
    /// 指标项实体
    /// </summary>
    public class HealthDetailViewModel
    {
        /// <summary>
        /// 指标类型
        /// </summary>
        public string DetailType { get; set; }
        /// <summary>
        /// 指标单位
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 指标值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 检测时间
        /// </summary>
        public string CheckDate { get; set; }
    }
    /// <summary>
    /// 单个指标项数据集（画曲线图用）
    /// </summary>
    public class HealthDetails
    {
        /// <summary>
        /// 指标项数据源
        /// </summary>
        public List<HealthDetailViewModel> Healths { get; set; }
        /// <summary>
        /// 最小值
        /// </summary>
        public decimal MinValue { get; set; }
        /// <summary>
        /// 最大值
        /// </summary>
        public decimal MaxValue { get; set; }
    }
    /// <summary>
    /// 血压指标项实体
    /// </summary>
    public class DetailHistoryForBloodPresureViewModel
    {
        /// <summary>
        /// 指标类型
        /// </summary>
        public string DetailType { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 收缩压
        /// </summary>
        public string ValueHigh { get; set; }
        /// <summary>
        /// 舒张压
        /// </summary>
        public string ValueLow { get; set; }
        /// <summary>
        /// 检测时间
        /// </summary>
        public string CheckDate { get; set; }
    }
    /// <summary>
    /// 血压指标项实体（画曲线图用）
    /// </summary>
    public class DetailForBloodPresureHistories
    {
        /// <summary>
        /// 血压指标数据集合
        /// </summary>
        public List<DetailHistoryForBloodPresureViewModel> Histories { get; set; }
        public decimal MinValue { get; set; }
        public decimal MaxValue { get; set; }
        /// <summary>
        /// 类型集合（比如：高血压、低血压）
        /// </summary>
        public List<string> Categorys { get; set; }
    }

    public class HealthManageReportViewModel
    {
        public string Id { get; set; }
        public int customerId { get; set; }
        /// <summary>
        /// 健康报告
        /// </summary>
        public string HReport { get; set; }
        /// <summary>
        /// 健康指导意见
        /// </summary>
        public string HGuidance { get; set; }
    }
}