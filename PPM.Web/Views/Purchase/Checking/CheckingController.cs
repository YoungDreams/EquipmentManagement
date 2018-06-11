using System.Linq;
using System.Web.Mvc;
using Foundation.Data;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Query;
using PensionInsurance.Web.Common;
using PensionInsurance.Web.Views.Account;
using Foundation.Core;
using PensionInsurance.Entities;
using PensionInsurance.Shared;
using Foundation.Data.Implemention;
using System;
using System.Collections.Generic;
using System.Text;
using PensionInsurance.Workflows;

namespace PensionInsurance.Web.Views.Purchase.Checking
{
    public class CheckingController : AuthorizedController
    {
        private readonly ICommandService _commandService;
        private readonly IPurchaseProductStockQuery _purchaseProductStockQuery;
        private readonly ICheckingProjectItemQuery _checkingProjectItemQuery;
        private readonly ICheckingProjectQuery _checkingProjectQuery;
        private readonly IProjectQueryService _projectQueryService;
        private readonly IProductCategoryQuery _productCategoryQuery;
        private readonly IPurchaseOrderQuery _purchaseOrderQuery;
        private readonly CheckingProjectWorkflow _checkingProjectWorkflow;
        private readonly IRepository _repository;

        public CheckingController(ICommandService commandService, IProductCategoryQuery productCategoryQuery, IPurchaseProductStockQuery purchaseProductStockQuery, ICheckingProjectItemQuery checkingProjectItemQuery, ICheckingProjectQuery checkingProjectQuery, IProjectQueryService projectQueryService, IPurchaseOrderQuery purchaseOrderQuery, CheckingProjectWorkflow checkingProjectWorkflow, IRepository repository)
        {
            _commandService = commandService;
            _purchaseProductStockQuery = purchaseProductStockQuery;
            _checkingProjectItemQuery = checkingProjectItemQuery;
            _checkingProjectQuery = checkingProjectQuery;
            _projectQueryService = projectQueryService;
            _purchaseOrderQuery = purchaseOrderQuery;
            _checkingProjectWorkflow = checkingProjectWorkflow;
            _repository = repository;
            _productCategoryQuery = productCategoryQuery;
        }

        public ActionResult CheckingProjectItem(PurchaseProductStockQuery query)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.盘点, Permission.查看))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var checkingProject = _checkingProjectQuery.Get(query.CheckingProjectId);
            var openDate = checkingProject.CreatedOn.Date;
            var personName = checkingProject.CheckingPerson.RealName;
            var view = "~/Views/Purchase/Checking/CheckingProjectItem.cshtml";
            var categories = _productCategoryQuery.QueryAllValid().ToList();
            if (checkingProject.CheckingStatus == CheckingStatus.已完成)
            {
                var project = _projectQueryService.Get(query.ProjectId);
                var checkingProjectItemQuery = new CheckingProjectItemQuery
                {
                    ProjectId = query.ProjectId,
                    ProjectName = project.Name,
                    CheckingProjectId = query.CheckingProjectId,
                };
                query.ProjectName = project.Name;
                var viewModel = new CheckingProjectItemViewModel
                {
                    Query = query,
                    CheckingProject = checkingProject,
                    Items = _checkingProjectItemQuery.QueryAll(checkingProjectItemQuery).Select(x => new CheckingItemViewModel
                    {
                        PurchaseProductId = x.PurchaseProduct.Id,
                        PurchaseProductName = x.PurchaseProduct.Name,
                        PurchaseProductCategory = GetFullProductCategory(x.PurchaseProduct.ProductCategory, categories),
                        PurchaseProductCategoryId = x.PurchaseProduct.ProductCategory.Id,
                        PurchaseProductCategoryCode = x.PurchaseProduct.ProductCategory.Code,
                        ProductCode = x.PurchaseProduct.Code,
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
                    }).ToList(),
                    OpenDate = openDate,
                    PersonName = personName
                };
                return View(view, viewModel);
            }
            else
            {
                var project = _projectQueryService.Get(query.ProjectId);
                query.ProjectName = project.Name;
                var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                var endDate = startDate.AddMonths(1).AddMilliseconds(-1);
                var viewModel = new CheckingProjectItemViewModel
                {
                    Query = query,
                    CheckingProject = checkingProject,
                    Items = _purchaseProductStockQuery.QueryAll(query).Where(x => x.StockInTime >= startDate && x.StockInTime <= endDate).Select(x => new CheckingItemViewModel
                    {
                        PurchaseProductId = x.PurchaseProductSupplier.PurchaseProduct.Id,
                        PurchaseProductName = x.PurchaseProductSupplier.PurchaseProduct.Name,
                        PurchaseProductCategory = GetFullProductCategory(x.PurchaseProductSupplier.PurchaseProduct.ProductCategory, categories),
                        PurchaseProductCategoryId = x.PurchaseProductSupplier.PurchaseProduct.ProductCategory.Id,
                        PurchaseProductCategoryCode = x.PurchaseProductSupplier.PurchaseProduct.ProductCategory.Code,
                        ProductCode = x.PurchaseProductSupplier.PurchaseProduct.Code,
                        Specification = x.PurchaseProductSupplier.PurchaseProduct.Specification,
                        Stock = x.Stock,
                        PurchaseProductStockId = x.Id,
                        Brand = x.PurchaseProductSupplier.PurchaseProduct.Brand,
                        Unit = x.PurchaseProductSupplier.PurchaseProduct.Unit,
                        PurchaseSupplier = x.PurchaseProductSupplier.PurchaseSupplier.Name,
                        PurchaseSupplierId = x.PurchaseProductSupplier.PurchaseSupplier.Id,
                        StockInDate = x.StockInTime,
                        SinglePrice = x.ActualPrice
                    }).ToViewModels(_checkingProjectItemQuery, _purchaseOrderQuery, project.Id),
                    OpenDate = openDate,
                    PersonName = personName
                };
                return View(view, viewModel);
            }
        }

        public ActionResult CheckingProject(CheckingProjectQuery query, int page = 1, int pageSize = PaginationSetttings.PageSize)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.盘点, Permission.查看))
            {
                return RedirectToAction("NoPermission", "Home");
            }
            var project = _projectQueryService.Get(query.ProjectId);
            query.ProjectName = project.Name;
            var viewModel = new CheckingProjectViewModel(Url)
            {
                Query = query,
                Items = _checkingProjectQuery.Query(query, page, pageSize)
            };
            return View("~/Views/Purchase/Checking/CheckingProject.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult Save(SaveCheckingProjectCommand command)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.盘点, Permission.删除))
            {
                return RedirectToAction("NoPermission", "Home");
            }
            _commandService.Execute(command);
            return RedirectToAction("CheckingProject", new {command.ProjectId, command.CheckingProjectId });
        }

        [HttpPost]
        public ActionResult Submit(SubmitCheckingProjectCommand command)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.盘点, Permission.删除))
            {
                return RedirectToAction("NoPermission", "Home");
            }
            _commandService.Execute(command);
            return RedirectToAction("CheckingApprovalDetail", new { id = command.CheckingProjectId });
        }

        [HttpPost]
        public ActionResult Cancel(DeleteCheckingProjectCommand command)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.盘点, Permission.删除))
            {
                return RedirectToAction("NoPermission", "Home");
            }
            _commandService.Execute(command);
            return RedirectToAction("CheckingApprovalDetail", new CheckingProjectQuery { ProjectId = command.ProjectId });
        }

        public ActionResult Index()
        {
            var currentUserProjectCount = WebAppContext.Current.User.Projects.Count;
            var projects = _projectQueryService.QueryAllValid();
            var currentUserProejctId = WebAppContext.Current.User.Projects?.FirstOrDefault()?.Id;
            if (currentUserProejctId.HasValue)
            {
                projects = projects.Where(x => x.Id == currentUserProejctId);
            }
            var viewModel = new SelectViewModel
            {
                ShowProjectList = currentUserProjectCount != 1,
                ProjectId = currentUserProejctId ?? 0,
                ProjectList = projects.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).ToList(),
                CategoryList = _productCategoryQuery.QueryParent().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
            };
            return PartialView("~/Views/purchase/Checking/_Checking.SelectProject.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult CheckingApprovalDetail(int id)
        {
            var checkingProjectId = id;
            var checkingProject = _checkingProjectQuery.Get(checkingProjectId);
            var openDate = checkingProject.CreatedOn.Date;
            var personName = checkingProject.CheckingPerson.RealName;
            var view = "~/Views/Purchase/Checking/CheckingApprovalDetail.cshtml";
            var categories = _productCategoryQuery.QueryAllValid().ToList();
            var project = checkingProject.Project;
            var checkingProjectSummaries = _repository.Query<CheckingProjectSummary>()
                .Where(x => x.CheckingProject.Id == checkingProjectId);
            var checkingProjectItemQuery = new CheckingProjectItemQuery
            {
                ProjectId = project.Id,
                ProjectName = project.Name,
                CheckingProjectId = checkingProject.Id,
            };
            var viewModel = new CheckingProjectItemApprovalViewModel
            {
                CheckingProject = checkingProject,
                Items = checkingProjectSummaries.Select(
                    x => new CheckingItemViewModel
                    {
                        PurchaseProductId = x.PurchaseProduct.Id,
                        PurchaseProductName = x.PurchaseProduct.Name,
                        PurchaseProductCategory = GetFullProductCategory(x.PurchaseProduct.ProductCategory, categories),
                        PurchaseProductCategoryId = x.PurchaseProduct.ProductCategory.Id,
                        PurchaseProductCategoryCode = x.PurchaseProduct.ProductCategory.Code,
                        ProductCode = x.PurchaseProduct.Code,
                        Specification = x.PurchaseProduct.Specification,
                        CheckingQuantity = x.CheckingQuantity,
                        Brand = x.PurchaseProduct.Brand,
                        Unit = x.PurchaseProduct.Unit,
                        CurrentCheckCount = x.CurrentCheckCount,
                        CurrentMonthStockInCount = x.CurrentMonthStockInCount,
                        CurrentMonthStockOutCount = x.CurrentMonthStockOutCount,
                        LastMonthCheckCount = x.LastMonthCheckCount,
                        Remarks = x.Remarks
                    }).ToList(),
                OpenDate = openDate,
                PersonName = personName,
                TrackingResult = new TrackingResult.TrackingResultViewModel
                {
                    WorkflowTrackingResults = _checkingProjectWorkflow.GetWorkflowTrackingResults(checkingProject),
                    WorkflowHistoryTrackingResults =
                        _checkingProjectWorkflow.GetWorkflowHistoryTrackingResults(checkingProject)
                },
                CurrentWorkFlowStep = _checkingProjectWorkflow.GetCurrentWorkflowStep(checkingProject)
            };
            //当前审批步骤

            if (viewModel.CurrentWorkFlowStep != null)
            {
                viewModel.CurrentWorkflowStepId = viewModel.CurrentWorkFlowStep.Id;
            }
            return View(view, viewModel);
        }

        [HttpPost]
        public ActionResult Approval(ApprovalCheckingProjectCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("CheckingApprovalDetail", new { id = command.CheckingProjectId });
        }

        [HttpGet]
        public ActionResult AssetInUseDetail(int id)
        {
            var checkingProjectId = id;
            var checkingProject = _checkingProjectQuery.Get(checkingProjectId);
            var openDate = checkingProject.CreatedOn.Date;
            var personName = checkingProject.CheckingPerson.RealName;
            var view = "~/Views/Purchase/Checking/AssetInUseDetail.cshtml";
            var categories = _productCategoryQuery.QueryAllValid().ToList();
            var project = checkingProject.Project;
            var checkingProjectItemQuery = new CheckingProjectItemQuery
            {
                ProjectId = project.Id,
                ProjectName = project.Name,
                CheckingProjectId = checkingProject.Id,
            };
            var viewModel = new CheckingProjectItemDetailViewModel
            {
                CheckingProject = checkingProject,
                Items = _checkingProjectItemQuery.QueryAll(checkingProjectItemQuery).Where(x => x.PurchaseProduct.ProductCategory.IsAssetOrLongConsumption).Select(
                    x => new CheckingItemViewModel
                    {
                        Id = x.Id,
                        PurchaseProductId = x.PurchaseProduct.Id,
                        PurchaseProductName = x.PurchaseProduct.Name,
                        PurchaseProductCategory = GetFullProductCategory(x.PurchaseProduct.ProductCategory, categories),
                        PurchaseProductCategoryId = x.PurchaseProduct.ProductCategory.Id,
                        PurchaseProductCategoryCode = x.PurchaseProduct.ProductCategory.Code,
                        ProductCode = x.PurchaseProduct.Code,
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
                    }).ToList(),
                OpenDate = openDate,
                PersonName = personName,
            };
            return View(view, viewModel);
        }

        [HttpPost]
        public ActionResult SubmitAssetInUseItems(AssetInUseItemsCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Index", "Home");
        }

        private string GetFullProductCategory(PurchaseProductCategory category, List<PurchaseProductCategory> categories)
        {
            var nameList = new StringBuilder();
            if (category.ParentId !=0)
            {
                var parentId = category.ParentId;
                var parent = categories.First(x => x.Id == parentId);
                nameList.Append(GetFullProductCategory(parent, categories)+"-");
            }
            nameList.Append(category.Name);
            return nameList.ToString();
        }
    }
}