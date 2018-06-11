using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Query;
using PensionInsurance.Web.Views.Account;
using PensionInsurance.Web.Views.Purchase.Checking;

namespace PensionInsurance.Web.Views.Reports.CheckingProjectReport
{
    public class CheckingProjectReportController : AuthorizedController
    {
        private readonly ICommandService _commandService;
        private readonly ICheckingProjectItemQuery _checkingProjectItemQuery;
        private readonly ICheckingProjectItemStockOutDetailQuery _checkingProjectItemStockOutDetailQuery;
        private readonly IProjectQueryService _projectQueryService;
        private readonly IProductCategoryQuery _productCategoryQuery;

        public CheckingProjectReportController(IUserBacklogQueryService backlogQueryService, ICommandService commandService, IProjectQueryService projectQueryService, ICheckingProjectItemQuery checkingProjectItemQuery, IProjectQueryService projectQueryService1, ICheckingProjectItemStockOutDetailQuery checkingProjectItemStockOutDetailQuery, IProductCategoryQuery productCategoryQuery)
        {
            _commandService = commandService;
            _checkingProjectItemQuery = checkingProjectItemQuery;
            _projectQueryService = projectQueryService1;
            _checkingProjectItemStockOutDetailQuery = checkingProjectItemStockOutDetailQuery;
            _productCategoryQuery = productCategoryQuery;
        }

        public ActionResult CheckingAssetStockOutInDetail(CheckingProjectItemQuery query)
        {
            var projects = _projectQueryService.QueryAllValid().ToList();
            var categories = _productCategoryQuery.QueryAllValid().ToList();
            if (query.EndDate != null)
                query.EndDate = new DateTime(query.EndDate.Value.Year, query.EndDate.Value.Month, 1).AddMonths(1)
                    .AddMilliseconds(-1);
            var viewModel = new CheckingAssetStockOutInDetailViewModel
            {
                Query = query,
                Projects = projects.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
            };

            if (query.EndDate.HasValue && query.StartDate.HasValue)
            {
                query.EndDate = new DateTime(query.EndDate.Value.Year, query.EndDate.Value.Month, 1).AddMonths(1).AddMilliseconds(-1);
                viewModel.Items = _checkingProjectItemQuery.QueryAllByProjectsAndMonths(query).Select(
                    x => new CheckingItemViewModel
                    {
                        ProjectId = x.CheckingProject.Project.Id,
                        ProjectName = x.CheckingProject.Project.Name,
                        SinglePrice = x.PurchaseProductStock.ActualPrice,
                        StockInDate = x.PurchaseProductStock.StockInTime,
                        PurchaseProductId = x.PurchaseProduct.Id,
                        PurchaseProductName = x.PurchaseProduct.Name,
                        PurchaseProductCategory = GetFullProductCategory(x.PurchaseProduct.ProductCategory, categories),
                        PurchaseProductCategoryId = x.PurchaseProduct.ProductCategory.Id,
                        PurchaseProductCategoryCode = x.PurchaseProduct.ProductCategory.Code,
                        ProductCode = x.PurchaseProduct.Code,
                        ProductPriceCode = x.PurchaseProductStock.PurchaseProductSupplier.Code,
                        Specification = x.PurchaseProduct.Specification,
                        Stock = x.Stock,
                        CheckingQuantity = x.CheckingQuantity,
                        ProfitLoss = x.ProfitLoss,
                        PurchaseProductStockId = x.PurchaseProductStock.Id,
                        Brand = x.PurchaseProduct.Brand,
                        Unit = x.PurchaseProduct.Unit,
                        PurchaseSupplier = x.PurchaseProductStock.PurchaseProductSupplier.PurchaseSupplier.Name,
                        PurchaseSupplierId = x.PurchaseProductStock.PurchaseProductSupplier.PurchaseSupplier.Id,
                        CurrentCheckCount = x.CurrentCheckCount,
                        CurrentMonthStockInCount = x.CurrentMonthStockInCount,
                        CurrentMonthStockOutCount = x.CurrentMonthStockOutCount,
                        LastMonthCheckCount = x.LastMonthCheckCount,
                        Remarks = x.Remarks
                    }).OrderBy(x => x.StockInDate).ToList();
            }
            return View("~/Views/Reports/CheckingProjectReport/CheckingAssetStockOutInDetail.cshtml", viewModel);
        }

        public ActionResult Export(ExportCheckingAssetStockOutInDetailCommand command)
        {
            var result = _commandService.ExecuteFoResult(command);
            return Json(new
            {
                success = result.IsSucceed,
                redirect = $"{Url.Content("~/Attachments/Report/")}{result.FileName}"
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AssetInUseStockOutDetail(CheckingProjectItemStockOutDetailQuery query)
        {
            var projects = _projectQueryService.QueryAllValid().ToList();
            var categories = _productCategoryQuery.QueryAllValid().ToList();
            var view = "~/Views/Reports/CheckingProjectReport/AssetInUseStockOutDetail.cshtml";
            if (query.EndDate != null)
                query.EndDate = new DateTime(query.EndDate.Value.Year, query.EndDate.Value.Month, 1).AddMonths(1)
                    .AddMilliseconds(-1);
            var viewModel = new CheckingProjectItemStockOutDetailViewModel
            {
                Query = query,
                Items = _checkingProjectItemStockOutDetailQuery.QueryAllByProjectsAndMonths(query).Select(x => new CheckingProjectItemStockOutDetailItem
                {
                    ProjectName = x.CheckingProjectItem.CheckingProject.Project.Name,
                    Brand = x.CheckingProjectItem.PurchaseProduct.Brand,
                    PurchaseProductCategoryName = GetFullProductCategory(x.CheckingProjectItem.PurchaseProduct.ProductCategory, categories),
                    PurchaseProductId = x.CheckingProjectItem.PurchaseProduct.Id,
                    PurchaseProductName = x.CheckingProjectItem.PurchaseProduct.Name,
                    PurchaseProductCategoryCode = x.CheckingProjectItem.PurchaseProduct.ProductCategory.Code,
                    ProductCode = x.CheckingProjectItem.PurchaseProduct.Code,
                    ProductPriceCode = x.CheckingProjectItem.PurchaseProductStock.PurchaseProductSupplier.Code,
                    Unit = x.CheckingProjectItem.PurchaseProduct.Unit,
                    Specification = x.CheckingProjectItem.PurchaseProduct.Specification,
                    PurchaseSupplierName = x.CheckingProjectItem.PurchaseProductStock.PurchaseProductSupplier.PurchaseSupplier.Name,
                    ActualPrice = x.CheckingProjectItem.PurchaseProductStock.ActualPrice,
                    CurrentMonthStockOutCount = x.CheckingProjectItem.CurrentMonthStockOutCount,
                    CollectDate = x.CollectDate.ToShortDateString(),
                    CollectDepartment = x.CollectDepartment,
                    Collector = x.Collector,
                    Location = x.Location,
                    Remarks = x.Remarks
                }).ToList(),
                Projects = projects.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).ToList()
            };
            return View(view, viewModel);
        }

        public ActionResult ExportAssetInUseStockOutDetail(ExportCheckingProjectItemStockOutDetailCommand command)
        {
            var result = _commandService.ExecuteFoResult(command);
            return Json(new
            {
                success = result.IsSucceed,
                redirect = $"{Url.Content("~/Attachments/Report/")}{result.FileName}"
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AssetStatisticsReport(CheckingProjectItemQuery query)
        {
            var view = "~/Views/Reports/CheckingProjectReport/AssetStatisticsReport.cshtml";
            var projects = _projectQueryService.QueryAllValid().ToList();
            var categories = _productCategoryQuery.QueryAllValid().ToList();
            var viewModel = new AssetStatiticsReportViewModel
            {
                Query = query,
                Items = _checkingProjectItemQuery.QueryAllByProjectsAndMonths(query).Select(x => new CheckingItemViewModel
                {
                    PurchaseProductId = x.PurchaseProduct.Id,
                    PurchaseProductName = x.PurchaseProduct.Name,
                    PurchaseProductCategory = GetFullProductCategory(x.PurchaseProduct.ProductCategory, categories),
                    PurchaseProductCategoryId = x.PurchaseProduct.ProductCategory.Id,
                    PurchaseProductCategoryLayer = x.PurchaseProduct.ProductCategory.Layer,
                    PurchaseProductCategoryParentId = x.PurchaseProduct.ProductCategory.ParentId,
                    Specification = x.PurchaseProduct.Specification,
                    Stock = x.Stock,
                    CheckingQuantity = x.CheckingQuantity,
                    ProfitLoss = x.ProfitLoss,
                    PurchaseProductStockId = x.PurchaseProductStock.Id,
                    Brand = x.PurchaseProduct.Brand,
                    Unit = x.PurchaseProduct.Unit,
                    PurchaseSupplier = x.PurchaseProductStock.PurchaseProductSupplier.PurchaseSupplier.Name,
                    PurchaseSupplierId = x.PurchaseProductStock.PurchaseProductSupplier.PurchaseSupplier.Id,
                    CurrentCheckCount = x.CurrentCheckCount,
                    CurrentMonthStockInCount = x.CurrentMonthStockInCount,
                    CurrentMonthStockOutCount = x.CurrentMonthStockOutCount,
                    LastMonthCheckCount = x.LastMonthCheckCount,
                    Remarks = x.Remarks,
                    SinglePrice = x.PurchaseProductStock.ActualPrice
                }).ToAssetStatisticsReportViewModels(categories),
                Projects = projects.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).ToList()
            };
            return View(view, viewModel);
        }

        public ActionResult ExportAssetStatisticsReport(ExportAssetStatisticsReportCommand command)
        {
            var result = _commandService.ExecuteFoResult(command);
            return Json(new
            {
                success = result.IsSucceed,
                redirect = $"{Url.Content("~/Attachments/Report/")}{result.FileName}"
            }, JsonRequestBehavior.AllowGet);
        }

        private string GetFullProductCategory(PurchaseProductCategory category, List<PurchaseProductCategory> categories)
        {
            var nameList = new StringBuilder();
            if (category.ParentId != 0)
            {
                var parentId = category.ParentId;
                var parent = categories.First(x => x.Id == parentId);
                nameList.Append(GetFullProductCategory(parent, categories) + "-");
            }
            nameList.Append(category.Name);
            return nameList.ToString();
        }
    }
}