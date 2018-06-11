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

namespace PensionInsurance.Web.Views.Finance.CustomerAccountTransfer
{
    public class CustomerAccountTransferController : AuthorizedController
    {
        private readonly ICustomerAccountQueryService _customerAccountQueryService;
        private readonly ICommandService _commandService;
        private readonly IFetcher _fetcher;
        private readonly CustomerAccountTransferWorkflow _workflow;

        public CustomerAccountTransferController(ICustomerAccountQueryService customerAccountQueryService, ICommandService commandService, IFetcher fetcher, CustomerAccountTransferWorkflow workflow)
        {
            _customerAccountQueryService = customerAccountQueryService;
            _commandService = commandService;
            _fetcher = fetcher;
            _workflow = workflow;
        }

        [HttpGet]
        public ActionResult CreateAndSubmit(int customerAccountId)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.客户转账, Permission.转账))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var customerAccount = _customerAccountQueryService.Get(customerAccountId);
            CreateViewModel viewModel = new CreateViewModel
            {
                TransferFrom = customerAccount.Id,
                TransferBalance = customerAccount.Balance,
                TransferName = customerAccount.Customer.Name,
                ProjectId = customerAccount.Project.Id
            };
            return View("~/Views/Finance/CustomerAccountTransfer/Create.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult CreateAndSubmit(CreateAndSubmitCustomerAccountTransferCommand command)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.客户转账, Permission.转账))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            command.Operator = WebAppContext.Current.User;
            var result = _commandService.ExecuteFoResult(command);

            return RedirectToAction("Detail", "CustomerAccountTransfer", new { id = result.CustomerAccountTransferId });
        }

        /// <summary>
        /// 跳转到编辑页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.客户转账, Permission.转账))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var customerAccountTransfer = _fetcher.Get<Entities.CustomerAccountTransfer>(id);

            var viewModel = new EditViewModel(Url)
            {
                CustomerAccountTransferId = customerAccountTransfer.Id,
                TransferFrom = customerAccountTransfer.TransferFrom.Id,
                TransferTo = customerAccountTransfer.TransferTo.Id,
                TransferFromCustomerName = customerAccountTransfer.TransferFrom.Customer.Name,
                TransferToCustomerName = customerAccountTransfer.TransferTo.Customer.Name,
                TransferToCustomerNo = customerAccountTransfer.TransferTo.Customer.CustomerNo,
                TransferToProjectName = customerAccountTransfer.TransferTo.Project.Name,
                TransferAmount = customerAccountTransfer.TransferAmount,
                TransferBalance = customerAccountTransfer.TransferFrom.Balance,
                Reason = customerAccountTransfer.Reason,
                Status = customerAccountTransfer.Status,
                ProjectId = customerAccountTransfer.TransferFrom.Project.Id,
                FileName = customerAccountTransfer.FileName,
                FilePath = customerAccountTransfer.FilePath
            };
            viewModel.TrackingResult = new TrackingResult.TrackingResultViewModel
            {
                WorkflowHistoryTrackingResults = _workflow.GetWorkflowHistoryTrackingResults(customerAccountTransfer)
            };
            return View("~/Views/Finance/CustomerAccountTransfer/Edit.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult Detail(int id)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.客户转账, Permission.查看))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var customerAccountTransfer = _fetcher.Get<Entities.CustomerAccountTransfer>(id);

            var viewModel = new EditViewModel(Url)
            {
                CustomerAccountTransferId = customerAccountTransfer.Id,
                TransferFrom = customerAccountTransfer.TransferFrom.Id,
                TransferTo = customerAccountTransfer.TransferTo.Id,
                TransferFromCustomerName = customerAccountTransfer.TransferFrom.Customer.Name,
                TransferToCustomerName = customerAccountTransfer.TransferTo.Customer.Name,
                TransferToCustomerNo = customerAccountTransfer.TransferTo.Customer.CustomerNo,
                TransferToProjectName = customerAccountTransfer.TransferTo.Project.Name,
                TransferAmount = customerAccountTransfer.TransferAmount,
                TransferBalance = customerAccountTransfer.TransferFrom.Balance,
                Reason = customerAccountTransfer.Reason,
                Status = customerAccountTransfer.Status,
                ProjectId = customerAccountTransfer.TransferFrom.Project.Id,
                FileName = customerAccountTransfer.FileName,
                FilePath = customerAccountTransfer.FilePath
            };
            
            viewModel.CurrentWorkFlowStep = _workflow.GetCurrentWorkflowStep(customerAccountTransfer);
            viewModel.TrackingResult = new TrackingResult.TrackingResultViewModel
            {
                WorkflowTrackingResults = _workflow.GetWorkflowTrackingResults(customerAccountTransfer),
                WorkflowHistoryTrackingResults = _workflow.GetWorkflowHistoryTrackingResults(customerAccountTransfer)
            };

            if (viewModel.CurrentWorkFlowStep != null)
            {
                viewModel.CurrentWorkflowStepId = viewModel.CurrentWorkFlowStep.Id;
            }
            return View("~/Views/Finance/CustomerAccountTransfer/Detail.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult SubmitPrint(SubmitPrintCustomerAccountTransferCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Detail", new { id = command.CustomerAccountTransferId });
        }

        [HttpPost]
        public ActionResult Print(PrintCustomerAccountTransferWordToPdfCommand command)
        {
            var result = _commandService.ExecuteFoResult(command);

            return Json(new
            {
                success = true,
                redirect = $"{Url.Content("~/Attachments/Print/")}{result.FileName}"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UploadCustomerAccountTransfer(UploadCustomerAccountTransferCommand command)
        {
            if (Request.Files.Count > 0)
            {
                command.FileBytes = Request.Files[0].ReadBytes();
                command.FileName = Request.Files[0].FileName;
            }
            _commandService.Execute(command);
            return RedirectToAction("Detail", new { id = command.CustomerAccountTransferId });
        }
        
        [HttpPost]
        public ActionResult Submit(SubmitCustomerAccountTransferCommand command)
        {
            command.Operator = WebAppContext.Current.User;
            _commandService.Execute(command);
            return RedirectToAction("Detail", new { id = command.CustomerAccountTransferId });
        }

        /// <summary>
        /// 审批通过
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Approval(ApprovalCustomerAccountTransferCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Detail", new { id = command.CustomerAccountTransferId });
        }

        /// <summary>
        /// 确认附件上传
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LockedAttachment(LockedCustomerAccountTransferAttachmentCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Detail", new { id = command.CustomerAccountTransferId });
        }
    }
}