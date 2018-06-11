using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PensionInsurance.Entities;
using PensionInsurance.Query;
using PensionInsurance.Web.Views.Purchase;
using PensionInsurance.Web.Views.Purchase.Checking;
using PensionInsurance.Web.Views.TrackingResult;

namespace PensionInsurance.Web.Views.Reports.CheckingProjectReport
{
    public class CheckingAssetStockOutInDetailViewModel
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int? CheckingProjectId { get; set; }
        public int PersonId { get; set; }
        public string PersonName { get; set; }
        public DateTime OpenDate { get; set; }
        public CheckingProjectItemQuery Query { get; set; }
        public List<CheckingItemViewModel> Items { get; set; }
        public CheckingProject CheckingProject { get; set; }
        public IEnumerable<SelectListItem> Projects { get; set; }
        public TrackingResultViewModel TrackingResult { get; set; }
        public WorkflowStep CurrentWorkFlowStep { get; set; }
        public int CurrentWorkflowStepId { get; set; }
    }

    public class CheckingProjectItemStockOutDetailViewModel
    {
        public CheckingProjectItemStockOutDetailQuery Query { get; set; }
        public List<CheckingProjectItemStockOutDetailItem> Items { get; set; }
        public List<SelectListItem> Projects { get; set; }
    }

    public class AssetStatiticsReportViewModel
    {
        public CheckingProjectItemQuery Query { get; set; }
        public List<AssetStatisticsReportViewModel> Items { get; set; }
        public List<SelectListItem> Projects { get; set; }
    }

    public class CheckingProjectItemStockOutDetailItem
    {
        public string ProjectName { get; set; }
        public int PurchaseProductId { get; set; }
        public string PurchaseProductName { get; set; }
        public string PurchaseProductCategoryName { get; set; }
        public string PurchaseProductCategoryCode { get; set; }
        public string ProductCode { get; set; }
        public string ProductPriceCode { get; set; }
        public string PurchaseSupplierName { get; set; }
        public string Brand { get; set; }
        public string Specification { get; set; }
        public string Unit { get; set; }
        public decimal ActualPrice { get; set; }
        public int CurrentMonthStockOutCount { get; set; }
        public string CurrentMonthStockOutCountAmountString => (ActualPrice * 1).ToString("F2");
        public string CollectDate { get; set; }
        public string CollectDepartment { get; set; }
        public string Collector { get; set; }
        public string Location { get; set; }
        public string Remarks { get; set; }
    }

    public class AssetStatisticsReportViewModel
    {
        public int PurchaseProductCategoryId { get; set; }
        public int? PurchaseProductCategoryParentId { get; set; }
        public int PurchaseProductCategoryLayer { get; set; }
        public string PurchaseProductCategory { get; set; }
        public ProductCategoryTreeView ProductCategoryTreeView { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int? CheckingQuantity { get; set; }
        public decimal? CheckingQuantityAmount { get; set; }
        /// <summary>
        /// 期初数，上月期末库存，即上月该商品的盘点数
        /// </summary>
        public int LastMonthCheckCount { get; set; }

        public decimal LastMonthCheckCountAmount { get; set; }

        /// <summary>
        /// 本月入库数，该项目 本月 所有 已完成 采购订单的 该商品 入库数
        /// </summary>
        public int CurrentMonthStockInCount { get; set; }
        public decimal CurrentMonthStockInCountAmount { get; set; }
        /// <summary>
        /// 本月出库数 = 期初+本月入库-盘点数
        /// </summary>
        public int CurrentMonthStockOutCount { get; set; }
        public decimal CurrentMonthStockOutCountAmount { get; set; }
        /// <summary>
        /// 期末库存，为本月盘点数
        /// </summary>
        public int CurrentCheckCount { get; set; }
        public decimal CurrentCheckCountAmount { get; set; }
    }
}