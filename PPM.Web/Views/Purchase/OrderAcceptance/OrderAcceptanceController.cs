using System;
using System.Linq;
using System.Web.Mvc;
using Foundation.Data;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Query;
using PensionInsurance.Shared;
using PensionInsurance.Web.Common;
using PensionInsurance.Web.Views.Account;
using PensionInsurance.Workflows;

namespace PensionInsurance.Web.Views.Purchase.OrderAcceptance
{
    public class OrderAcceptanceController : AuthorizedController
    {
        private readonly ICommandService _commandService;
        private readonly IFetcher _fetcher;
        private readonly IProjectQueryService _projectQueryService;
        private readonly IPurchaseOrderAcceptanceQuery _purchaseOrderAcceptanceQuery;
        private readonly PurchaseCheckWorkflow _purchaseCheckWorkflow;

        public OrderAcceptanceController(IFetcher fetcher, ICommandService commandService, IProjectQueryService projectQueryService, IPurchaseOrderAcceptanceQuery purchaseOrderAcceptanceQuery, PurchaseCheckWorkflow purchaseCheckWorkflow)
        {
            _fetcher = fetcher;
            _commandService = commandService;
            _projectQueryService = projectQueryService;
            _purchaseOrderAcceptanceQuery = purchaseOrderAcceptanceQuery;
            _purchaseCheckWorkflow = purchaseCheckWorkflow;
        }

        // GET: Order
        public ActionResult Index(int page = 1, int pageSize = Web.Common.PaginationSetttings.PageSize, PurchaseOrderAcceptanceQuery query = null)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.采购管理, Permission.采购订单))
            {
                return RedirectToAction("NoPermission", "Home");
            }
            var viewModel = new IndexViewModel
            {
                Query = query,
                Items = _purchaseOrderAcceptanceQuery.Query(page, pageSize, query),
                ProjectList = _projectQueryService.QueryAllValidByProjectFilter().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };

            return View("~/Views/Purchase/OrderAcceptance/index.cshtml", viewModel);
        }

        public ActionResult Detail(int id)
        {
            var orderAcceptance = _fetcher.Get<PurchaseOrderAcceptance>(id);
            var viewModel = new DetailViewModel(_fetcher)
            {
                PurchaseOrderAcceptance = orderAcceptance,
                PurchaseOrder = orderAcceptance.PurchaseOrder,
            };
            if (viewModel.PurchaseOrder != null)
            {
                if (orderAcceptance.PurchaseSupplier != null)
                {
                    var items = orderAcceptance.PurchaseOrder.OrderItems.Where(s =>
                        s.PurchaseProductSupplier.PurchaseSupplier == orderAcceptance.PurchaseSupplier).ToList();
                    if (items.Any())
                    {
                        viewModel.PurchaseOrderItems = items;
                        viewModel.TotalProductPrice = items.Sum(x => x.ActualPrice * x.ActualQuantity);
                        viewModel.TotalProductQuantity = items.Sum(s => s.ActualQuantity);
                    }
                }
                else
                {
                    viewModel.PurchaseOrderOtherItems = viewModel.PurchaseOrder.OtherOrderItems.ToList();
                    viewModel.TotalProductPrice = viewModel.PurchaseOrderOtherItems.Sum(x => x.ActualPurchaseQuantity * x.ActualPurchasePrice);
                    viewModel.TotalProductQuantity = viewModel.PurchaseOrderOtherItems.Sum(s => s.ActualPurchaseQuantity);
                }
            }

            viewModel.CurrentWorkflowProgress = _purchaseCheckWorkflow.GetCurrentWorkflowProgress(orderAcceptance);

            viewModel.TrackingResult = new TrackingResult.TrackingResultViewModel
            {
                WorkflowTrackingResults = _purchaseCheckWorkflow.GetWorkflowTrackingResults(orderAcceptance),
                WorkflowHistoryTrackingResults = _purchaseCheckWorkflow.GetWorkflowHistoryTrackingResults(orderAcceptance)
            };

            viewModel.CurrentWorkFlowStep = _purchaseCheckWorkflow.GetCurrentWorkflowStep(orderAcceptance);//当前审批步骤

            if (viewModel.CurrentWorkFlowStep != null)
            {
                viewModel.CurrentWorkflowStepId = viewModel.CurrentWorkFlowStep.Id;
            }
            return View("~/Views/Purchase/OrderAcceptance/Detail.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult ViewAttachment(int orderAcceptanceId)
        {
            var order = _fetcher.Get<PurchaseOrderAcceptance>(orderAcceptanceId);
            var viewModel = new ViewAttachmentViewModel
            {
                OrderId = orderAcceptanceId,
                ImageBase64String = Convert.ToBase64String(order.AttachmentBytes)
            };

            return View("~/Views/Purchase/OrderAcceptance/ViewAttachment.cshtml", viewModel);
        }

        //拟单人入库确认
        [HttpPost]
        public ActionResult Inspect(InspectPurchaseOrderAcceptanceCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Detail", new { id = command.OrderAcceptanceId });
        }

        public ActionResult UpdateOrderItemActualQuantity(int quantity, int orderItemId, int orderAcceptanceId)
        {
            UpdateOrderItemActualQuantityCommand command = new UpdateOrderItemActualQuantityCommand
            {
                Quantity = quantity,
                ItemId = orderItemId,
            };
            _commandService.Execute(command);

            var orderAcceptance = _fetcher.Get<PurchaseOrderAcceptance>(orderAcceptanceId);

            var totalQuantity = 0;
            decimal totalAmount = 0;

            var orderitem = _fetcher.Get<PurchaseOrderItem>(orderItemId);

            if (orderAcceptance != null)
            {
                if (orderAcceptance.PurchaseSupplier != null)
                {
                    var items = orderAcceptance.PurchaseOrder.OrderItems.Where(s =>
                        s.PurchaseProductSupplier.PurchaseSupplier == orderAcceptance.PurchaseSupplier).ToList();
                    if (items.Any())
                    {
                        totalAmount = items.Sum(x => x.ActualPrice * x.ActualQuantity);
                        totalQuantity = items.Sum(s => s.ActualQuantity);
                    }
                }
                else
                {
                    var  items = orderAcceptance.PurchaseOrder.OtherOrderItems.ToList();
                    totalAmount = items.Sum(x => x.ActualPurchaseQuantity * x.ActualPurchasePrice);
                    totalQuantity = items.Sum(s => s.ActualPurchaseQuantity);
                }
            }
          
            var itemTotalAmount = orderitem.ActualQuantity * orderitem.ActualPrice;
            return Json(new { success = true, TotalQuantity = totalQuantity, TotalAmount = totalAmount, ItemTotalAmount = itemTotalAmount, msg = "修改成功" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateOrderOtherItemActualQuantity(int quantity, int orderItemId, int orderAcceptanceId)
        {
            UpdateOrderOtherItemActualQuantityCommand command = new UpdateOrderOtherItemActualQuantityCommand
            {
                Quantity = quantity,
                ItemId = orderItemId,
            };
            _commandService.Execute(command);
            var orderAcceptance = _fetcher.Get<PurchaseOrderAcceptance>(orderAcceptanceId);

            var totalQuantity = 0;
            decimal totalAmount = 0;

            var orderitem = _fetcher.Get<PurchaseOrderOtherItem>(orderItemId);

            if (orderAcceptance != null)
            {
                if (orderAcceptance.PurchaseSupplier != null)
                {
                    var items = orderAcceptance.PurchaseOrder.OrderItems.Where(s =>
                        s.PurchaseProductSupplier.PurchaseSupplier == orderAcceptance.PurchaseSupplier).ToList();
                    if (items.Any())
                    {
                        totalAmount = items.Sum(x => x.ActualPrice * x.ActualQuantity);
                        totalQuantity = items.Sum(s => s.ActualQuantity);
                    }
                }
                else
                {
                    var items = orderAcceptance.PurchaseOrder.OtherOrderItems.ToList();
                    totalAmount = items.Sum(x => x.ActualPurchaseQuantity * x.ActualPurchasePrice);
                    totalQuantity = items.Sum(s => s.ActualPurchaseQuantity);
                }
            }
            var itemTotalAmount = orderitem.ActualPurchaseQuantity * orderitem.ActualPurchasePrice;
            return Json(new { success = true, TotalQuantity = totalQuantity, TotalAmount = totalAmount, ItemTotalAmount = itemTotalAmount, msg = "修改成功" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CheckSubmit(CheckSubmitPurchaseOrderAcceptanceCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Detail", new { id = command.OrderAcceptanceId });
        }

        [HttpPost]
        public ActionResult Approval(ApprovalPurchaseOrderAcceptanceCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Detail", new { id = command.OrderAcceptanceId });
        }

        public ActionResult Print(int id)
        {
            var orderAcceptance = _fetcher.Get<PurchaseOrderAcceptance>(id);
            var viewModel = new DetailViewModel(_fetcher)
            {
                PurchaseOrder = orderAcceptance.PurchaseOrder,
                PurchaseOrderAcceptance = orderAcceptance
            };
            if (viewModel.PurchaseOrder != null)
            {
                if (orderAcceptance.PurchaseSupplier != null)
                {
                    var items = orderAcceptance.PurchaseOrder.OrderItems.Where(s =>
                        s.PurchaseProductSupplier.PurchaseSupplier == orderAcceptance.PurchaseSupplier).ToList();
                    if (items.Any())
                    {
                        viewModel.PurchaseOrderItems = items;
                        viewModel.TotalProductPrice = items.Sum(x => x.ActualPrice * x.ActualQuantity);
                        viewModel.TotalProductQuantity = items.Sum(s => s.ActualQuantity);
                    }
                }
                else
                {
                    viewModel.PurchaseOrderOtherItems = viewModel.PurchaseOrder.OtherOrderItems.ToList();
                    viewModel.TotalProductPrice = viewModel.PurchaseOrderOtherItems.Sum(x => x.ActualPurchaseQuantity * x.ActualPurchasePrice);
                    viewModel.TotalProductQuantity = viewModel.PurchaseOrderOtherItems.Sum(s => s.ActualPurchaseQuantity);
                }
            }
            viewModel.TrackingResult = new TrackingResult.TrackingResultViewModel
            {
                WorkflowTrackingResults = _purchaseCheckWorkflow.GetWorkflowTrackingResults(orderAcceptance),
                WorkflowHistoryTrackingResults = _purchaseCheckWorkflow.GetWorkflowHistoryTrackingResults(orderAcceptance)
            };

            var rawHtml = this.RenderView("~/Views/Purchase/OrderAcceptance/Print.cshtml", viewModel);

            var result = _commandService.ExecuteFoResult(new SavePrintPurchaseOrderAcceptanceFileCommand { Id = id, Content = rawHtml });
            return Redirect(Url.Content(result.FilePath));
        }

        [HttpPost]
        public ActionResult Upload(UploadPurchaseOrderAcceptanceCommand command)
        {
            if (Request.Files.Count > 0)
            {
                command.FileBytes = Request.Files[0].ReadBytes();
                command.FileName = Request.Files[0].FileName;
            }
            var result = _commandService.ExecuteFoResult(command);
            return Json(result);
        }

        public ActionResult Edit(int id)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.采购管理, Permission.查看))
            {
                return RedirectToAction("NoPermission", "Home");
            }
            var orderAcceptance = _fetcher.Get<PurchaseOrderAcceptance>(id);
            if (orderAcceptance == null)
            {
                return RedirectToAction("Index");
            }
            var viewModel = new EditViewModel(_fetcher)
            {
                PurchaseOrderAcceptance = orderAcceptance,
                PurchaseOrder = orderAcceptance.PurchaseOrder,
            };

            if (viewModel.PurchaseOrder != null)
            {
                if (orderAcceptance.PurchaseSupplier != null)
                {
                    var items = orderAcceptance.PurchaseOrder.OrderItems.Where(s =>
                        s.PurchaseProductSupplier.PurchaseSupplier == orderAcceptance.PurchaseSupplier).ToList();
                    if (items.Any())
                    {
                        viewModel.PurchaseOrderItems = items;
                        viewModel.TotalProductPrice = items.Sum(x => x.ActualPrice * x.ActualQuantity);
                        viewModel.TotalProductQuantity = items.Sum(s => s.ActualQuantity);
                    }
                }
                else
                {
                    viewModel.PurchaseOrderOtherItems = viewModel.PurchaseOrder.OtherOrderItems.ToList();
                    viewModel.TotalProductPrice = viewModel.PurchaseOrderOtherItems.Sum(x => x.ActualPurchaseQuantity * x.ActualPurchasePrice);
                    viewModel.TotalProductQuantity = viewModel.PurchaseOrderOtherItems.Sum(s => s.ActualPurchaseQuantity);
                }
            }
            if (viewModel.PurchaseOrder != null && viewModel.PurchaseOrder.OrderType != OrderType.食材采购单)
            {
                viewModel.CurrentWorkflowStepUser =
                    _purchaseCheckWorkflow.GetFirstApprovalWorkflowStepUser(orderAcceptance);
            }
            else
            {
                viewModel.CurrentWorkflowStepUser = orderAcceptance.PurchaseOrder.User;
            }
            
            return View("~/Views/Purchase/OrderAcceptance/Edit.cshtml", viewModel);
        }
    }
}