using Foundation.Core;
using Foundation.Data;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Web.Views.Account;
using System;
using System.Linq;
using System.Web.Mvc;
using PensionInsurance.Query;
using PensionInsurance.Shared;
using PensionInsurance.Web.Common;
using PensionInsurance.Workflows;

namespace PensionInsurance.Web.Views.ContractServicePackChange
{
    public class ContractServicePackChangeController : AuthorizedController
    {
        private readonly ICommandService _commandService;
        private readonly IFetcher _fetcher;
        private readonly ContractServicePackChangeWorkflow _contractServicePackChangeWorkflow;
        public ContractServicePackChangeController(ICommandService commandService, IFetcher fetcher, ContractServicePackChangeWorkflow contractServicePackChangeWorkflow)
        {
            _commandService = commandService;
            _fetcher = fetcher;
            _contractServicePackChangeWorkflow = contractServicePackChangeWorkflow;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        public ActionResult Create(int contractId)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.服务包补充协议, Permission.新增))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            // 合同信息
            var contract = _fetcher.Get<Entities.Contract>(contractId);
            var previousServicePackChange = _fetcher.Query<Entities.ContractServicePackChange>().Single(x => x.Contract.Id == contractId && x.Status == ContractAddtionalStatus.生效 && x.ChangeEndDate == contract.EndTime);
            // 客户信息
            var viewModel = new CreateViewModel
            {
                StartDate = previousServicePackChange.ChangeDate.AddDays(1),
                EndDate = previousServicePackChange.ChangeEndDate.AddDays(-1),
                CustomerName = contract.Customer.Name,
                ProjectId = contract.Project.Id,
                ContractId = contractId,
                CustomerAccountId = contract.CustomerAccount.Id,
                Contarct=contract
            };
            return View("~/Views/ContractServicePackChange/Create.cshtml", viewModel);
        }
        /// <summary>
        /// 跳转到编辑页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.服务包补充协议, Permission.新增))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var contractServicePackChange = _fetcher.Get<Entities.ContractServicePackChange>(id);

            var viewModel = new EditViewModel(Url)
            {
                ProjectId = contractServicePackChange.Contract.Project.Id,
                ContractServicePackChangeId = contractServicePackChange.Id,
                ContractId = contractServicePackChange.Contract.Id,
                CustomerAccountId = contractServicePackChange.Contract.CustomerAccount.Id,
                ContractServicePackChangeNo = contractServicePackChange.ContractServicePackChangeNo,
                CustomerName = contractServicePackChange.CustomerName,
                ChangeDate = contractServicePackChange.ChangeDate,
                ChangeEndDate = contractServicePackChange.ChangeEndDate,
                ConcernType = contractServicePackChange.ConcernType,
                ServiceLevel = contractServicePackChange.ServiceLevel,
                ShortServiceMonthlyAmount = contractServicePackChange.ShortServiceMonthlyAmount,
                ShortConcernAmount = contractServicePackChange.ShortConcernAmount,
                LongServiceMonthlyAmount = contractServicePackChange.LongServiceMonthlyAmount,
                LongConcernAmount = contractServicePackChange.LongConcernAmount,
                FileName = contractServicePackChange.FileName,
                FilePath = contractServicePackChange.FilePath,
                Status = contractServicePackChange.Status,
                IsLockedAttachment = contractServicePackChange.IsLockedAttachment,
                LongAttachAmount = contractServicePackChange.LongAttachAmount,
                ShortAttachAmount = contractServicePackChange.ShortAttachAmount
            };
            viewModel.Contract = contractServicePackChange.Contract;
            viewModel.TrackingResult = new TrackingResult.TrackingResultViewModel
            {
                WorkflowHistoryTrackingResults = _contractServicePackChangeWorkflow.GetWorkflowHistoryTrackingResults(contractServicePackChange)
            };
            return View("~/Views/ContractServicePackChange/Edit.cshtml", viewModel);
        }
        /// <summary>
        /// 跳转到查看页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Detail(int id)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.服务包补充协议, Permission.查看))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var contractServicePackChange = _fetcher.Get<Entities.ContractServicePackChange>(id);

            var viewModel = new EditViewModel(Url)
            {
                ProjectId = contractServicePackChange.Contract.Project.Id,
                ContractServicePackChangeId = contractServicePackChange.Id,
                ContractId = contractServicePackChange.Contract.Id,
                CustomerAccountId = contractServicePackChange.Contract.CustomerAccount.Id,
                ContractServicePackChangeNo = contractServicePackChange.ContractServicePackChangeNo,
                CustomerName = contractServicePackChange.CustomerName,
                ChangeDate = contractServicePackChange.ChangeDate,
                ChangeEndDate = contractServicePackChange.ChangeEndDate,
                ConcernType = contractServicePackChange.ConcernType,
                ServiceLevel = contractServicePackChange.ServiceLevel,
                ShortServiceMonthlyAmount = contractServicePackChange.ShortServiceMonthlyAmount,
                ShortConcernAmount = contractServicePackChange.ShortConcernAmount,
                LongServiceMonthlyAmount = contractServicePackChange.LongServiceMonthlyAmount,
                LongConcernAmount = contractServicePackChange.LongConcernAmount,
                FileName = contractServicePackChange.FileName,
                FilePath = contractServicePackChange.FilePath,
                Status = contractServicePackChange.Status,
                IsLockedAttachment = contractServicePackChange.IsLockedAttachment,
                LongAttachAmount = contractServicePackChange.LongAttachAmount,
                ShortAttachAmount = contractServicePackChange.ShortAttachAmount
            };
            viewModel.Contract = contractServicePackChange.Contract;
            viewModel.CurrentWorkFlowStep = _contractServicePackChangeWorkflow.GetCurrentWorkflowStep(contractServicePackChange);//当前审批步骤
            viewModel.TrackingResult = new TrackingResult.TrackingResultViewModel
            {
                WorkflowTrackingResults = _contractServicePackChangeWorkflow.GetWorkflowTrackingResults(contractServicePackChange),
                WorkflowHistoryTrackingResults = _contractServicePackChangeWorkflow.GetWorkflowHistoryTrackingResults(contractServicePackChange)
            };
            if (viewModel.CurrentWorkFlowStep != null)
            {
                viewModel.CurrentWorkflowStepId = viewModel.CurrentWorkFlowStep.Id;
            }
            return View("~/Views/ContractServicePackChange/Detail.cshtml", viewModel);
        }

        /// <summary>
        /// 打印服务包协议
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Print(PrintContractServicePackChangeWordToPdfCommand command)
        {
            var result = _commandService.ExecuteFoResult(command);
            
            return Json(new
            {
                success = true,
                redirect = $"{Url.Content("~/Attachments/Print/")}{result.FileName}"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SubmitPrint(SubmitPrintContractServicePackChangeCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Detail", new { id = command.ContractServicePackChangeId });
        }

        /// <summary>
        /// 上传服务包协议
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadContractServicePackChange(UploadContractServicePackChangeCommand command)
        {
            if (Request.Files.Count > 0)
            {
                command.FileBytes = Request.Files[0].ReadBytes();
                command.FileName = Request.Files[0].FileName;
            }
            _commandService.Execute(command);
            return RedirectToAction("Detail", new { id = command.ContractServicePackChangeId });
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Submit(SubmitContractServicePackChangeCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Detail", new { id = command.ContractServicePackChangeId });
        }

        [HttpPost]
        public ActionResult CreateAndSubmit(CreateAndSubmitContractServicePackChangeCommand command)
        {
            var result = _commandService.ExecuteFoResult(command);
            return RedirectToAction("Detail", new { id = result.ContractServicePackChangeId });
        }

        /// <summary>
        /// 确认
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Effective(EffectiveContractServicePackChangeCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Detail", new { id = command.ContractServicePackChangeId });
        }
        
        /// <summary>
        /// 审批通过
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Approval(ApprovalContractServicePackChangeCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Detail", new { id = command.ContractServicePackChangeId });
        }
        
        /// <summary>
        /// 作废
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DraftAndDelete(DraftAndDeleteContractServicePackChangeCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("IndexCheckIn", "Customer");
        }

        /// <summary>
        /// 确认附件上传
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LockedAttachment(LockedContractServicePackChangeAttachmentCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Detail", new { id = command.ContractServicePackChangeId });
        }
    }
}