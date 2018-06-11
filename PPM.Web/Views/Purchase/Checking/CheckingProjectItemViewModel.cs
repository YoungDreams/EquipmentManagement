using System;
using System.Web.Mvc;
using Foundation.Data;
using PensionInsurance.Query;
using PensionInsurance.Web.Common;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using PensionInsurance.Web.Views.Reports.CheckingProjectReport;
using PensionInsurance.Web.Views.TrackingResult;

namespace PensionInsurance.Web.Views.Purchase.Checking
{
    public class CheckingProjectItemViewModel
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int? CheckingProjectId { get; set; }
        public int PersonId { get; set; }
        public string PersonName { get; set; }
        public DateTime OpenDate { get; set; }
        public PurchaseProductStockQuery Query { get; set; }
        public List<CheckingItemViewModel> Items { get; set; }
        public CheckingProject CheckingProject { get; set; }
        public List<SelectListItem> Categories { get; set; }
    }

    public class CheckingItemViewModel
    {
        public int Id { get; set; }
        public int PurchaseProductStockId { get; set; }
        public int PurchaseProductId { get; set; }
        public int PurchaseProductCategoryId { get; set; }
        public int? PurchaseProductCategoryParentId { get; set; }
        public int PurchaseProductCategoryLayer { get; set; }
        public string PurchaseProductCategory { get; set; }
        public string PurchaseProductCategoryCode { get; set; }
        public string ProductCode { get; set; }
        public string ProductPriceCode { get; set; }
        public string PurchaseProductName { get; set; }
        public string PurchaseSupplier { get; set; }
        public int PurchaseSupplierId { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string Specification { get; set; }
        public string Unit { get; set; }
        public string Brand { get; set; }
        public string AssetClass { get; set; }
        public int Stock { get; set; }
        public int? CheckingQuantity { get; set; }
        public decimal? CheckingQuantityAmount => CheckingQuantity * SinglePrice;
        public int? ProfitLoss {get;set;}
        public decimal SinglePrice { get; set; }
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
        /// <summary>
        /// 备注及用途
        /// </summary>
        public string Remarks { get; set; }
        public DateTime StockInDate { get; set; }
    }

    public static class CheckingItemModelConverter
    {
        public static List<CheckingItemViewModel> ToViewModels(this IEnumerable<CheckingItemViewModel> stocks, ICheckingProjectItemQuery checkingProjectItemQuery, IPurchaseOrderQuery purchaseOrderQuery, int projectId)
        {
            var productIds = stocks.Select(x => x.PurchaseProductId).Distinct();
            var mergedModels = new List<CheckingItemViewModel>();
            var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var endDate = startDate.AddMonths(1).AddMilliseconds(-1);

            var lastMonthStartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month-1, 1);
            var lastMonthEndDate = lastMonthStartDate.AddMonths(1).AddMilliseconds(-1);
            var allCheckingItems = checkingProjectItemQuery.QueryAll(new CheckingProjectItemQuery{ProjectId = stocks.FirstOrDefault().ProjectId});
            foreach (var productId in productIds)
            {
                var temps = stocks.Where(x => x.PurchaseProductId == productId);
                var temp = temps.First(x => x.PurchaseProductId == productId);
                var currentMonthStockInCount = temps.Where(x => x.StockInDate >= startDate && x.StockInDate <= endDate).Sum(x => x.Stock);
                var lastMonthCheckCount = allCheckingItems.Where(x => x.PurchaseProduct.Id == productId && x.CheckingProject.CheckingDate >= lastMonthStartDate && x.CheckingProject.CheckingDate <= lastMonthEndDate).Sum(x => x.CurrentCheckCount);
                var newModel = new CheckingItemViewModel
                {
                    PurchaseProductId = temp.PurchaseProductId,
                    PurchaseProductName = temp.PurchaseProductName,
                    PurchaseProductCategory = temp.PurchaseProductCategory,
                    PurchaseProductCategoryCode = temp.PurchaseProductCategoryCode,
                    ProductCode = temp.ProductCode,
                    PurchaseProductCategoryId = temp.PurchaseProductCategoryId,
                    Specification = temp.Specification,
                    Stock = temp.Stock,
                    PurchaseProductStockId = temp.PurchaseProductStockId,
                    Brand = temp.Brand,
                    Unit = temp.Unit,
                    PurchaseSupplier = temp.PurchaseSupplier,
                    PurchaseSupplierId = temp.PurchaseSupplierId,
                    CurrentMonthStockInCount = currentMonthStockInCount,
                    LastMonthCheckCount = lastMonthCheckCount
                };
                mergedModels.Add(newModel);
                    
                
            }
            return mergedModels.ToList();
        }

        public static List<AssetStatisticsReportViewModel> ToAssetStatisticsReportViewModels(this IEnumerable<CheckingItemViewModel> stocks, List<PurchaseProductCategory> categories)
        {
            if (stocks == null|| !stocks.Any())
            {
                return new List<AssetStatisticsReportViewModel>();
            }
            var assertStatisticsReport = new List<AssetStatisticsReportViewModel>();
            var firstLayerCategories = categories.Where(x => x.ParentId == 0 && x.Layer == 1);
            foreach (var firstLayerCategory in firstLayerCategories)
            {
                var itemsBy1stLayerCategory = new List<CheckingItemViewModel>();
                var categoryList = new List<PurchaseProductCategory>();
                categoryList.Add(firstLayerCategory);
                foreach (var checkingItemViewModel in stocks)
                {
                    var category = categories.FirstOrDefault(x => x.Id == checkingItemViewModel.PurchaseProductCategoryId);
                    var allCategories = GetAllCategories(category, categories);
                    var topCategory = allCategories.FirstOrDefault();
                    if (topCategory == firstLayerCategory)
                    {
                        //categoryList.Add(category);
                        categoryList.AddRange(allCategories);
                        itemsBy1stLayerCategory.Add(checkingItemViewModel);
                    }
                }
                var resultBy1stCategory = new AssetStatisticsReportViewModel
                {
                    PurchaseProductCategory = firstLayerCategory.Name,
                    PurchaseProductCategoryId = firstLayerCategory.Id,
                    PurchaseProductCategoryLayer = firstLayerCategory.Layer,
                    PurchaseProductCategoryParentId = firstLayerCategory.ParentId,
                    ProductCategoryTreeView = new ProductCategoryTreeView(null, true).GetProductCategoryTreeView(categoryList.Distinct().ToList()),
                    LastMonthCheckCountAmount = itemsBy1stLayerCategory.Sum(x => x.LastMonthCheckCount * x.SinglePrice),
                    CheckingQuantityAmount = itemsBy1stLayerCategory.Sum(x =>
                    {
                        if (x.CheckingQuantity != null) return x.CheckingQuantity.Value * x.SinglePrice;
                        return 0;
                    }),
                    CurrentCheckCountAmount = itemsBy1stLayerCategory.Sum(x => x.CurrentCheckCount * x.SinglePrice),
                    CurrentMonthStockInCountAmount = itemsBy1stLayerCategory.Sum(x => x.CurrentMonthStockInCount * x.SinglePrice),
                    CurrentMonthStockOutCountAmount = itemsBy1stLayerCategory.Sum(x => x.CurrentMonthStockOutCount * x.SinglePrice)
                };
                assertStatisticsReport.Add(resultBy1stCategory);

            }
            return assertStatisticsReport;
        }

        private static List<PurchaseProductCategory> GetAllCategories(PurchaseProductCategory category, List<PurchaseProductCategory> categories)
        {
            var categoryList = new List<PurchaseProductCategory>();
            if (category != null && category.ParentId != 0)
            {
                var parentId = category.ParentId;
                var parent = categories.First(x => x.Id == parentId);
                categoryList.AddRange(GetAllCategories(parent, categories));
            }
            categoryList.Add(category);
            return categoryList;
        }
    }

    public class CheckingProjectItemApprovalViewModel
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int? CheckingProjectId { get; set; }
        public int PersonId { get; set; }
        public string PersonName { get; set; }
        public DateTime OpenDate { get; set; }
        public PurchaseProductStockQuery Query { get; set; }
        public List<CheckingItemViewModel> Items { get; set; }
        public CheckingProject CheckingProject { get; set; }
        public List<SelectListItem> Categories { get; set; }
        public TrackingResultViewModel TrackingResult { get; set; }
        public WorkflowStep CurrentWorkFlowStep { get; set; }
        public int CurrentWorkflowStepId { get; set; }
    }

    public class CheckingProjectItemDetailViewModel
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int? CheckingProjectId { get; set; }
        public int PersonId { get; set; }
        public string PersonName { get; set; }
        public DateTime OpenDate { get; set; }
        public PurchaseProductStockQuery Query { get; set; }
        public List<CheckingItemViewModel> Items { get; set; }
        public CheckingProject CheckingProject { get; set; }
        public List<SelectListItem> Categories { get; set; }
    }

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
        public List<SelectListItem> Categories { get; set; }
        public TrackingResultViewModel TrackingResult { get; set; }
        public WorkflowStep CurrentWorkFlowStep { get; set; }
        public int CurrentWorkflowStepId { get; set; }
    }
}