using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Foundation.Core;
using Foundation.Data;
using Foundation.Messaging;
using Newtonsoft.Json;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Entities.Exceptions;
using PensionInsurance.Query;
using PensionInsurance.Query.Implemention;
using PensionInsurance.SendEmail;
using PensionInsurance.Shared;
using PensionInsurance.Web.Common;
using PensionInsurance.Web.Views.Account;
using PensionInsurance.Workflows;

namespace PensionInsurance.Web.Views.Purchase.Order
{
    public class OrderController : AuthorizedController
    {
        public const string CookiePurchaseCart = "CookiePurchaseCart";

        private readonly ICommandService _commandService;
        private readonly IFetcher _fetcher;
        private readonly IPurchaseOrderQuery _purchaseOrderQuery;
        private readonly IProjectQueryService _projectQueryService;
        private readonly IPurchaseProductStockQuery _purchaseProductStockQuery;
        private readonly IProductCategoryQuery _productCategoryQuery;
        private readonly IBudgetQueryService _budgetQueryService;
        private readonly PurchaseWorkflow _purchaseWorkflow;
        private readonly PurchaseOrderSendEmailService _emailService;
        private readonly IUserQueryService _userQueryService;
        private readonly IDepartmentQueryService _departmentQueryService;

        public OrderController(IFetcher fetcher, ICommandService commandService, IProductCategoryQuery productCategoryQuery, IProductQuery productQuery, IPurchaseOrderQuery purchaseOrderQuery, IPurchaseProductSupplierQuery purchaseProductSupplierQuery, IProjectQueryService projectQueryService, IPurchaseProductStockQuery purchaseProductStockQuery, IBudgetQueryService budgetQueryService, PurchaseWorkflow purchaseWorkflow, PurchaseOrderSendEmailService emailService, IUserQueryService userQueryService, IDepartmentQueryService departmentQueryService)
        {
            _fetcher = fetcher;
            _commandService = commandService;
            _productCategoryQuery = productCategoryQuery;
            _purchaseOrderQuery = purchaseOrderQuery;
            _projectQueryService = projectQueryService;
            _purchaseProductStockQuery = purchaseProductStockQuery;
            _budgetQueryService = budgetQueryService;
            _purchaseWorkflow = purchaseWorkflow;
            _emailService = emailService;
            _userQueryService = userQueryService;
            _departmentQueryService = departmentQueryService;
        }

        // GET: Order
        public ActionResult Index(int page = 1, int pageSize = Web.Common.PaginationSetttings.PageSize, PurchaseOrderQuery query = null)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.采购管理, Permission.采购订单))
            {
                return RedirectToAction("NoPermission", "Home");
            }
            var viewModel = new IndexViewModel()
            {
                Query = query,
                Items = _purchaseOrderQuery.Query(page, pageSize, query),
                ProjectList = _projectQueryService.QueryAllValidByProjectFilter().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                DepartmentList = _departmentQueryService.QueryAllValidByDepartmentFilter().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                UserList = _userQueryService.QueryAllValid().Select(x => new SelectListItem
                {
                    Text = x.RealName,
                    Value = x.Id.ToString()
                })
            };

            return View("~/Views/Purchase/order/index.cshtml", viewModel);
        }

        public ActionResult Detail(int id)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.采购管理, Permission.查看))
            {
                return RedirectToAction("NoPermission", "Home");
            }
            var order = _fetcher.Get<PurchaseOrder>(id);
            var viewModel = new DetailViewModel(_fetcher)
            {
                PurchaseOrder = order,
            };
            if (order != null)
            {
                viewModel.TotalProductPrice = order.OrderItems.Sum(s => s.ActualPrice * s.ActualQuantity)+ order.OtherOrderItems.Sum(s => s.ActualPurchasePrice * s.ActualPurchaseQuantity);
                viewModel.TotalProductQuantity = order.OrderItems.Sum(s => s.ActualQuantity)+ order.OtherOrderItems.Sum(s => s.ActualPurchaseQuantity);
            }

            viewModel.TrackingResult = new TrackingResult.TrackingResultViewModel
            {
                WorkflowTrackingResults = _purchaseWorkflow.GetWorkflowTrackingResults(order),
                WorkflowHistoryTrackingResults = _purchaseWorkflow.GetWorkflowHistoryTrackingResults(order)
            };
            viewModel.CurrentWorkFlowStep = _purchaseWorkflow.GetCurrentWorkflowStep(order);//当前审批步骤

            if (viewModel.CurrentWorkFlowStep != null)
            {
                viewModel.CurrentWorkflowStepId = viewModel.CurrentWorkFlowStep.Id;
            }
            return View("~/Views/Purchase/order/Detail.cshtml", viewModel);
        }

        public ActionResult DetailPrint(int id)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.采购管理, Permission.查看))
            {
                return RedirectToAction("NoPermission", "Home");
            }
            var order = _fetcher.Get<PurchaseOrder>(id);
            var viewModel = new DetailViewModel(_fetcher)
            {
                PurchaseOrder = order,
            };
            if (order != null)
            {
                //if (!string.IsNullOrWhiteSpace(order.PrintOrderFilePath))
                //{
                //    return Redirect(Url.Content(order.PrintOrderFilePath));
                //}
                viewModel.TotalProductPrice = order.OrderItems.Sum(s => s.ActualPrice * s.ActualQuantity) +
                                              order.OtherOrderItems.Sum(s =>
                                                  s.ActualPurchasePrice * s.ActualPurchaseQuantity);
                viewModel.TotalProductQuantity = order.OrderItems.Sum(s => s.ActualQuantity) +
                                                 order.OtherOrderItems.Sum(s => s.ActualPurchaseQuantity);
                viewModel.PurchaseSuppliers = viewModel.PurchaseOrder.OrderItems
                    .Select(m => m.PurchaseProductSupplier.PurchaseSupplier.Name).Distinct().ToList();
            }
            viewModel.TrackingResult = new TrackingResult.TrackingResultViewModel
            {
                WorkflowTrackingResults = _purchaseWorkflow.GetWorkflowTrackingResults(order),
                WorkflowHistoryTrackingResults = _purchaseWorkflow.GetWorkflowHistoryTrackingResults(order)
            };
            viewModel.CurrentWorkFlowStep = _purchaseWorkflow.GetCurrentWorkflowStep(order); //当前审批步骤

            if (viewModel.CurrentWorkFlowStep != null)
            {
                viewModel.CurrentWorkflowStepId = viewModel.CurrentWorkFlowStep.Id;
            }
            var rawHtml = this.RenderView("~/Views/Purchase/order/DetailPrint.cshtml", viewModel);

            //var bytes = Encoding.UTF8.GetBytes(rawHtml);
            var result =
                _commandService.ExecuteFoResult(new SavePrintPurchaseOrderFileCommand { Id = id, Content = rawHtml });
            return Redirect(Url.Content(result.FilePath));
        }

        public ActionResult AgainEdit(int id)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.采购管理, Permission.查看))
            {
                return RedirectToAction("NoPermission", "Home");
            }
            var order = _fetcher.Get<Entities.PurchaseOrder>(id);
            if (order == null)
            {
                return RedirectToAction("Index");
            }
            var viewModel = new AgainEditViewModel(Url,_fetcher)
            {
                Order = order,
                Cart = order.PurchaseShoppingCart
            };
            viewModel.TotalPrice = order.PurchaseShoppingCart.CartItems.Sum(s => s.PurchaseQuantity * s.PurchaseProductSupplier.Price);
            viewModel.TotalPrice += order.PurchaseShoppingCart.OtherCartItems.Sum(s => s.PurchaseQuantity * s.PurchasePrice);
            viewModel.TotalQuantity = order.PurchaseShoppingCart.CartItems.Sum(s => s.PurchaseQuantity);
            viewModel.TotalQuantity += order.PurchaseShoppingCart.OtherCartItems.Sum(s => s.PurchaseQuantity);
            viewModel.TrackingResult = new TrackingResult.TrackingResultViewModel
            {
                WorkflowTrackingResults = _purchaseWorkflow.GetWorkflowTrackingResults(order),
                WorkflowHistoryTrackingResults = _purchaseWorkflow.GetWorkflowHistoryTrackingResults(order)
            };
            return View("~/Views/Purchase/order/AgainEdit.cshtml", viewModel);
        }

        public ActionResult AgainEditSubmit(AgainEditSubmitPurchaseShoppingCartCommand command)
        {
            var result = _commandService.ExecuteFoResult(command);
            
            return RedirectToAction("Detail", new { id = result.OrderId });
        }

        [HttpPost]
        public ActionResult Approval(ApprovalPurchaseOrderCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Detail", new { id = command.OrderId });
        }
        
        /// <summary>
        /// 提交确认食材采购单
        /// </summary>
        public ActionResult SubmitFoodPurchaseOrder(SubmitFoodPurchaseOrderCommand command)
        {
            var result = _commandService.ExecuteFoResult(command);
            return RedirectToAction("Detail", "PurchaseConfirm", new { id = result.ConfirmId });
        }

        [HttpGet]
        public ActionResult ConfirmOrder(OrderType orderType, int projectId, DateTime purchaseStartDate, DateTime purchaseEndDate)
        {
            var viewModel = new ConfirmOrderViewModel
            {
                OrderType = orderType,
                Project = _fetcher.Get<Entities.Project>(projectId),
                PurchaseStartDate = purchaseStartDate,
                PurchaseEndDate = purchaseEndDate
            };
            var purchaseOrderAcceptancess = _fetcher.Query<PurchaseOrderAcceptance>().Where(x =>
                x.PurchaseOrder.Project.Id == projectId && x.PurchaseOrder.OrderType == orderType &&
                x.Status == AcceptanceStatus.已验收 &&
                x.PurchaseOrder.OrderDate.Date >= purchaseStartDate.Date
                && x.PurchaseOrder.OrderDate.Date <= purchaseEndDate.Date).OrderByDescending(o => o.CreatedOn).ToList();

            if (purchaseOrderAcceptancess.Any())
            {
                viewModel.PurchaseOrderAcceptances = purchaseOrderAcceptancess;
                //viewModel.TotalAmount = purchaseOrderAcceptancess.Sum(s => s.ActualTotalAmount);

                decimal totalAmount = 0;

                foreach (var item in purchaseOrderAcceptancess)
                {
                    if (item.PurchaseSupplier != null)
                    {
                        var items = item.PurchaseOrder.OrderItems.Where(s =>
                            s.PurchaseProductSupplier.PurchaseSupplier == item.PurchaseSupplier).ToList();
                        if (items.Any())
                        {

                            totalAmount+= items.Sum(x => x.ActualPrice * x.ActualQuantity);
                        }
                    }
                    else
                    {
                        var items= item.PurchaseOrder.OtherOrderItems.ToList();
                        totalAmount += items.Sum(x => x.ActualPurchaseQuantity * x.PurchasePrice);
                    }
                }
                viewModel.TotalAmount = totalAmount;
            }
            return View("~/Views/Purchase/order/ConfirmOrder.cshtml", viewModel);
        }

        public Workflow GetWorkflow(PurchaseOrder order)
        {
            WorkflowCategory workflowCategory = WorkflowCategory.计划采购验收;

            if (order.OrderType == OrderType.运营紧急采购)
            {
                workflowCategory = WorkflowCategory.紧急采购验收;
            }
            if (order.OrderType == OrderType.食材紧急采购)
            {
                workflowCategory = WorkflowCategory.食材紧急采购验收;
            }
            if (order.OrderType == OrderType.食材采购单)
            {
                workflowCategory = WorkflowCategory.食材采购验收;
            }
            if (order.OrderType == OrderType.餐饮部计划采购)
            {
                workflowCategory = WorkflowCategory.餐饮部计划采购验收;
            }

            var workflow =_fetcher.Query<Entities.Workflow>()
                    .FirstOrDefault(x => x.Project == order.Project && x.WorkflowCategory == workflowCategory && x.Status == WorkflowStatus.启用);

            if (workflow == null)
            {
                throw new DomainValidationException("操作失败，请先联系管理员添加审批流程");
            }
            return workflow;
        }

        [HttpGet]
        public ActionResult FoodOrderDetail(int orderAcceptanceId)
        {
            var orderAcceptance = _fetcher.Get<PurchaseOrderAcceptance>(orderAcceptanceId);

            var list = new List<PurchaseOrderItemDetail>();

            var num = 1;

            var items = orderAcceptance.PurchaseOrder.OrderItems.Where(s =>s.PurchaseProductSupplier.PurchaseSupplier == orderAcceptance.PurchaseSupplier).ToList();
           
            foreach (var item in items)
            {
                PurchaseOrderItemDetail model = new PurchaseOrderItemDetail
                {
                    No = num,
                    Name = item.PurchaseProduct.Name,
                    Unit = item.PurchaseProduct.Unit,
                    Price = item.ActualPrice,
                    Count = item.ActualQuantity,
                    TotalAmount = item.ActualQuantity * item.ActualPrice,
                };
                if (!string.IsNullOrWhiteSpace(item.PurchaseProduct.Specification))
                {
                    model.Specification = item.PurchaseProduct.Specification;
                }
                else
                {
                    model.Specification = string.Empty;
                }
                list.Add(model);
                num++;
            }
            return Json(new
            {
                Result = true,
                TotalPrice = items.Sum(s => s.ActualPrice * s.ActualQuantity),
                Data = list
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditOrderOtherItem(int id)
        {
            var orderOtherItem = _fetcher.Get<Entities.PurchaseOrderOtherItem>(id);
            var editOrderOtherItem = new EditOrderOtherItemViewModel()
            {
                Id = orderOtherItem.Id,
                ProductUrl = orderOtherItem.ProductUrl,
                PurchasePrice = orderOtherItem.PurchasePrice,
                ProductName = orderOtherItem.ProductName
            };
            return Json(editOrderOtherItem, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditOrderOtherItem(EditOrderOtherItemCommand command)
        {
            _commandService.Execute(command);

            return RedirectToAction("Index");
        }

        public class PurchaseOrderItemDetail
        {
            public int No { get; set; }
            public string Name { get; set; }
            public string Specification { get; set; }
            public string Unit { get; set; }
            public decimal Price { get; set; }
            public int Count { get; set; }
            public decimal TotalAmount { get; set; }
        }
    }
}