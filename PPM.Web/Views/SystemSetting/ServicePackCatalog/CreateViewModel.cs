using PensionInsurance.Entities;

namespace PensionInsurance.Web.Views.SystemSetting.ServicePackCatalog
{
    public class CreateViewModel
    {
        /// <summary>
        /// 服务项目编码
        /// </summary>
        public string ServicePackCatalogNo { get; set; }
        /// <summary>
        /// 服务项目名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 单次价格
        /// </summary>
        public decimal UnitPrice { get; set; }
        /// <summary>
        /// 价格说明
        /// </summary>
        public string UnitPriceRemark { get; set; }
        /// <summary>
        /// 包月价格
        /// </summary>
        public decimal MonthPrice { get; set; }
        /// <summary>
        /// 服务项目归属类别
        /// </summary>
        public ServicePackCatalogType Type { get; set; }
        /// <summary>
        /// 频次
        /// </summary>
        public string MonthlyAmount { get; set; }
        /// <summary>
        /// 耗材
        /// </summary>
        public string Consumptive { get; set; }
        /// <summary>
        /// 耗材价格
        /// </summary>
        public decimal ConsumptivePrice { get; set; }
        /// <summary>
        /// 服务包等级(必选)
        /// </summary>
        public string ServicePackage { get; set; }
        /// <summary>
        /// 服务包等级(可选)
        /// </summary>
        public string ServicePackageOptional { get; set; }
        /// <summary>
        /// 服务说明
        /// </summary>
        public string Remark { get; set; }
    }
}