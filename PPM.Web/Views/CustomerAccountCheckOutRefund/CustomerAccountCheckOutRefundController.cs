using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foundation.Data;
using Foundation.Messaging;
using PensionInsurance.CommandHandlers;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Query;
using PensionInsurance.Web.Common;
using PensionInsurance.Web.Views.Account;
using PensionInsurance.Web.Views.Customer;
using PensionInsurance.Workflows;

namespace PensionInsurance.Web.Views.CustomerAccountCheckOutRefund
{
    public class CustomerAccountCheckOutRefundController : AuthorizedController
    {
        private readonly ICommandService _commandService;
        private readonly IFetcher _fetcher;
        private readonly IProjectQueryService _projectQueryService;
        private readonly CustomerAccountCheckOutRefundWorkflow _contractRefundWorkflow;

        public CustomerAccountCheckOutRefundController(ICommandService commandService, IFetcher fetcher,
            IProjectQueryService projectQueryService, CustomerAccountCheckOutRefundWorkflow contractRefundWorkflow)
        {
            _commandService = commandService;
            _fetcher = fetcher;
            _projectQueryService = projectQueryService;
            _contractRefundWorkflow = contractRefundWorkflow;
        }

        [HttpGet]
        public ActionResult CreateAndSubmit(int id)
        {
            var customerAccount = _fetcher.Get<CustomerAccount>(id);

            CreateViewModel viewModel = new CreateViewModel
            {
                RefundProjects =
                    _projectQueryService.QueryAllValid()
                        .Where(x => x.Id != customerAccount.Project.Id)
                        .Select(x => x.MapToEntity<Project>()),
                CustomerAccount = customerAccount
            };

            return View("~/Views/CustomerAccountCheckOutRefund/Create.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult Submit(SubmitCheckOutRefundCommand command)
        {
            var result = _commandService.ExecuteFoResult(command);
            return RedirectToAction("Detail", new { id = result.CheckOutRefundId });
        }

        [HttpPost]
        public ActionResult Approval(ApprovalCheckOutRefundCommand command)
        {
            _commandService.Execute(command);
            return new RedirectResult(Url.Action("Detail", "CustomerAccountCheckOutRefund", new { Id = command.ContractRefundId }));
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var checkOutRefund = _fetcher.Get<Entities.CustomerAccountCheckOutRefund>(id);

            CreateViewModel viewModel = new CreateViewModel
            {
                CheckOutRefundId = checkOutRefund.Id,
                RefundProjects =
                    _projectQueryService.QueryAllValid()
                        .Where(x => x.Id != checkOutRefund.CustomerAccount.Project.Id)
                        .Select(x => x.MapToEntity<Project>()),
                RefundName = checkOutRefund.RefundName,
                RefundBank = checkOutRefund.RefundBank,
                RefundMoney = checkOutRefund.RefundMoney,
                RefundAccountNo = checkOutRefund.RefundAccountNo,
                RefundName2 = checkOutRefund.RefundName2,
                RefundBank2 = checkOutRefund.RefundBank2,
                RefundMoney2 = checkOutRefund.RefundMoney2,
                RefundAccountNo2 = checkOutRefund.RefundAccountNo2,

                RefundName3 = checkOutRefund.RefundName3,
                RefundBank3 = checkOutRefund.RefundBank3,
                RefundMoney3 = checkOutRefund.RefundMoney3,
                RefundAccountNo3 = checkOutRefund.RefundAccountNo3,

                Project = checkOutRefund.RefundProject,
                RefundType = checkOutRefund.RefundType,
                RefundStatus = checkOutRefund.RefundStatus,
                CustomerAccount = checkOutRefund.CustomerAccount
            };

            viewModel.TrackingResult = new TrackingResult.TrackingResultViewModel
            {
                WorkflowHistoryTrackingResults = _contractRefundWorkflow.GetWorkflowHistoryTrackingResults(checkOutRefund)
            };

            return View("~/Views/CustomerAccountCheckOutRefund/Create.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult Detail(int id)
        {
            var contractRefund = _fetcher.Get<Entities.CustomerAccountCheckOutRefund>(id);

            EditViewModel viewModel = new EditViewModel(Url)
            {
                ContractRefundId = contractRefund.Id,
                RefundInfo = contractRefund
            };
            viewModel.TrackingResult = new TrackingResult.TrackingResultViewModel
            {
                WorkflowTrackingResults = _contractRefundWorkflow.GetWorkflowTrackingResults(contractRefund),
                WorkflowHistoryTrackingResults = _contractRefundWorkflow.GetWorkflowHistoryTrackingResults(contractRefund)
            };
            viewModel.CurrentWorkFlowStep = _contractRefundWorkflow.GetCurrentWorkflowStep(contractRefund);//当前审批步骤
            if (viewModel.CurrentWorkFlowStep != null)
            {
                viewModel.CurrentWorkflowStepId = viewModel.CurrentWorkFlowStep.Id;
            }
            return View("~/Views/CustomerAccountCheckOutRefund/Detail.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult PrintRefund(PrintCheckOutRefundWordToPdfCommand command)
        {
            var result = _commandService.ExecuteFoResult(command);

            return Json(new
            {
                success = true,
                redirect = $"{Url.Content("~/Attachments/Print/")}{result.FileName}"
            }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 打印协议
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PrintAgreement(PrintCheckOutAgreementWordToPdfCommand command)
        {
            var result = _commandService.ExecuteFoResult(command);

            return Json(new
            {
                success = true,
                redirect = $"{Url.Content("~/Attachments/Print/")}{result.FileName}"
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 确认附件上传
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LockedAttachment(LockedCheckOutRefundAttachmentCommand command)
        {
            _commandService.Execute(command);
            return new RedirectResult(Url.Action("Detail", "CustomerAccountCheckOutRefund", new { Id = command.ContractRefundId }));
        }

        [HttpPost]
        public ActionResult ApprovalUpload(ApprovalUploadCheckOutRefundCommand command)
        {
            _commandService.Execute(command);
            return new RedirectResult(Url.Action("Detail", "CustomerAccountCheckOutRefund", new { Id = command.ContractRefundId }));
        }

        [HttpPost]
        public ActionResult Upload(UploadCheckOutRefundCommand command)
        {
            if (Request.Files.Count > 0)
            {
                command.FileBytes = Request.Files[0].ReadBytes();
                command.FileName = Request.Files[0].FileName;
            }
            _commandService.Execute(command);
            return new RedirectResult(Url.Action("Detail", "CustomerAccountCheckOutRefund", new { Id = command.ContractRefundId }));
        }

        [HttpPost]
        public ActionResult UploadAgreement(UploadCheckOutAgreementCommand command)
        {
            if (Request.Files.Count > 0)
            {
                command.FileBytes = Request.Files[0].ReadBytes();
                command.FileName = Request.Files[0].FileName;
            }
            _commandService.Execute(command);
            return new RedirectResult(Url.Action("Detail", "CustomerAccountCheckOutRefund", new { Id = command.ContractRefundId }));
        }

        [HttpPost]
        public ActionResult DefrayCustomerMoneyManually(DefrayCustomerMoneyManuallyCommand command)
        {
            _commandService.Execute(command);
            return new RedirectResult(Url.Action("Detail", "CustomerAccountCheckOutRefund", new { Id = command.ContractRefundId }));
        }
    }
}