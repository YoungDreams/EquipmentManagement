using System;
using System.Collections.Generic;
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
using PensionInsurance.Shared;
using PensionInsurance.Web.Common;
using PensionInsurance.Web.Views.Account;
using PensionInsurance.Workflows;

namespace PensionInsurance.Web.Views.Purchase.PurchaseConfirm
{
    public class PurchaseConfirmController : AuthorizedController
    {
        private readonly ICommandService _commandService;
        private readonly IFetcher _fetcher;
        private readonly PurchaseConfirmWorkflow _purchaseConfirmWorkflow;

        public PurchaseConfirmController(IFetcher fetcher, ICommandService commandService, PurchaseConfirmWorkflow purchaseConfirmWorkflow)
        {
            _fetcher = fetcher;
            _commandService = commandService;
            _purchaseConfirmWorkflow = purchaseConfirmWorkflow;
        }

        [HttpGet]
        public ActionResult Detail(int id)
        {
            var confirm = _fetcher.Get<Entities.PurchaseConfirm>(id);
            var orderAcceptances = _fetcher.Query<PurchaseOrderAcceptance>().Where(x => x.PurchaseConfirm == confirm);

            var viewModel = new DetailViewModel()
            {
                PurchaseConfirm = confirm,
                PurchaseOrderAcceptances = orderAcceptances,
                CurrentWorkFlowStep = _purchaseConfirmWorkflow.GetCurrentWorkflowStep(confirm),
                TrackingResult = new TrackingResult.TrackingResultViewModel
                {
                    WorkflowTrackingResults = _purchaseConfirmWorkflow.GetWorkflowTrackingResults(confirm),
                    WorkflowHistoryTrackingResults = _purchaseConfirmWorkflow.GetWorkflowHistoryTrackingResults(confirm)
                }
            };
            decimal totalAmount = 0;

            foreach (var item in orderAcceptances)
            {
                if (item.PurchaseSupplier != null)
                {
                    var items = item.PurchaseOrder.OrderItems.Where(s =>
                        s.PurchaseProductSupplier.PurchaseSupplier == item.PurchaseSupplier).ToList();
                    if (items.Any())
                    {

                        totalAmount += items.Sum(x => x.ActualPrice * x.ActualQuantity);
                    }
                }
                else
                {
                    var items = item.PurchaseOrder.OtherOrderItems.ToList();
                    totalAmount += items.Sum(x => x.ActualPurchaseQuantity * x.PurchasePrice);
                }
            }
            viewModel.TotalAmount = totalAmount;
            if (viewModel.CurrentWorkFlowStep != null)
            {
                viewModel.CurrentWorkflowStepId = viewModel.CurrentWorkFlowStep.Id;
            }
            return View("~/Views/Purchase/PurchaseConfirm/Detail.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult Approval(ApprovalFoodPurchaseOrderCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Detail", new { id = command.ConfirmId });
        }

        [HttpGet]
        public ActionResult Print(int id)
        {
            var confirm = _fetcher.Get<Entities.PurchaseConfirm>(id);
            var orderAcceptances = _fetcher.Query<PurchaseOrderAcceptance>().Where(x => x.PurchaseConfirm == confirm);

            var viewModel = new DetailViewModel()
            {
                PurchaseConfirm = confirm,
                PurchaseOrderAcceptances = orderAcceptances,
                TrackingResult = new TrackingResult.TrackingResultViewModel
                {
                    WorkflowTrackingResults = _purchaseConfirmWorkflow.GetWorkflowTrackingResults(confirm),
                    WorkflowHistoryTrackingResults = _purchaseConfirmWorkflow.GetWorkflowHistoryTrackingResults(confirm)
                }
            };

            decimal totalAmount = 0;

            foreach (var item in orderAcceptances)
            {
                if (item.PurchaseSupplier != null)
                {
                    var items = item.PurchaseOrder.OrderItems.Where(s =>
                        s.PurchaseProductSupplier.PurchaseSupplier == item.PurchaseSupplier).ToList();
                    if (items.Any())
                    {

                        totalAmount += items.Sum(x => x.ActualPrice * x.ActualQuantity);
                    }
                }
                else
                {
                    var items = item.PurchaseOrder.OtherOrderItems.ToList();
                    totalAmount += items.Sum(x => x.ActualPurchaseQuantity * x.PurchasePrice);
                }
            }
            viewModel.TotalAmount = totalAmount;

            if (viewModel.CurrentWorkFlowStep != null)
            {
                viewModel.CurrentWorkflowStepId = viewModel.CurrentWorkFlowStep.Id;
            }

            var rawHtml = this.RenderView("~/Views/Purchase/PurchaseConfirm/DetailPrint.cshtml", viewModel);

            var result =
                _commandService.ExecuteFoResult(new SavePrintPurchaseConfirmFileCommand { Id = id, Content = rawHtml });
            return Redirect(Url.Content(result.FilePath));
        }

    }
}