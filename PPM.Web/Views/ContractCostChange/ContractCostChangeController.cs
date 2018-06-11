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

namespace PensionInsurance.Web.Views.ContractCostChange
{
    public class ContractCostChangeController : AuthorizedController
    {
        private readonly ICommandService _commandService;
        private readonly IFetcher _fetcher;
        private readonly ICurrentDiscountQueryService _currentDiscountQuery;
        private readonly ContractCostChangeWorkflow _contractCostChangeWorkflow;
        public ContractCostChangeController(ICommandService commandService, IFetcher fetcher, ICurrentDiscountQueryService currentDiscountQuery, ContractCostChangeWorkflow contractCostChangeWorkflow)
        {
            _commandService = commandService;
            _fetcher = fetcher;
            _currentDiscountQuery = currentDiscountQuery;
            _contractCostChangeWorkflow = contractCostChangeWorkflow;
        }

        [HttpGet]
        public ActionResult Create(int contractId)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.费用变更补充协议, Permission.新增))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var contract = _fetcher.Get<Entities.Contract>(contractId);
            var viewModel = new CreateViewModel
            {
                CustomerName = contract.Customer.Name,
                ContractId = contractId,
            };
            var discountAmount =
                _fetcher.Query<CustomerAccountDiscountLog>()
                    .Where(
                        x => x.CustomerAccount.Project == contract.Project && x.DiscountTime.Year == DateTime.Now.Year)
                    .ToList();
            viewModel.CustomerCurrentYearDiscount =
                discountAmount.Where(x => x.CustomerAccount == contract.CustomerAccount).Sum(s => s.DiscountAmount);
            viewModel.ProjectYearDiscount = discountAmount.Sum(x => x.DiscountAmount);
            return View("~/Views/ContractCostChange/Create.cshtml", viewModel);
        }

        /// <summary>
        /// 跳转到编辑页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.费用变更补充协议, Permission.新增))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var contractCostChange = _fetcher.Get<Entities.ContractCostChange>(id);

            var viewModel = new EditContractCostViewModel(Url)
            {
                Id = contractCostChange.Id,
                ContractCostChangeNo = contractCostChange.ContractCostChangeNo,
                CustomerName = contractCostChange.CustomerName,
                ContractId = contractCostChange.Contract.Id,
                ChangeDate = contractCostChange.ChangeDate,
                ChangeEndDate = contractCostChange.ChangeEndDate,
                ChangeLimit = contractCostChange.ChangeLimit,
                FileName = contractCostChange.FileName,
                FilePath = contractCostChange.FilePath,
                Status = contractCostChange.Status,
                Description = contractCostChange.Description,
                IsLockedAttachment = contractCostChange.IsLockedAttachment,
                CurrentDiscount = contractCostChange.CurrentDiscount,
                ChangeType = contractCostChange.ChangeType,
                ChangeRoomCost = contractCostChange.ChangeRoomCost,
                ChangeMealsCost = contractCostChange.ChangeMealsCost,
                ChangeRatio = contractCostChange.ChangeRatio,
                IsIncluded = contractCostChange.IsIncluded
            };
            var discountAmount =
                _fetcher.Query<CustomerAccountDiscountLog>()
                    .Where(
                        x => x.CustomerAccount.Project == contractCostChange.Contract.Project && x.DiscountTime.Year == DateTime.Now.Year)
                    .ToList();
            viewModel.CustomerCurrentYearDiscount =
                discountAmount.Where(x => x.CustomerAccount == contractCostChange.Contract.CustomerAccount).Sum(s => s.DiscountAmount);
            viewModel.ProjectYearDiscount = discountAmount.Sum(x => x.DiscountAmount);

            viewModel.TrackingResult = new TrackingResult.TrackingResultViewModel
            {
                WorkflowHistoryTrackingResults = _contractCostChangeWorkflow.GetWorkflowHistoryTrackingResults(contractCostChange)
            };

            return View("~/Views/ContractCostChange/Edit.cshtml", viewModel);
        }
        /// <summary>
        /// 跳转到查看页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Detail(int id)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.费用变更补充协议, Permission.查看))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var contractCostChange = _fetcher.Get<Entities.ContractCostChange>(id);

            var viewModel = new EditContractCostViewModel(Url)
            {
                Id = contractCostChange.Id,
                ContractCostChangeNo = contractCostChange.ContractCostChangeNo,
                CustomerName = contractCostChange.CustomerName,
                ContractId = contractCostChange.Contract.Id,
                CustomerAccountId = contractCostChange.Contract.CustomerAccount.Id,
                ChangeDate = contractCostChange.ChangeDate,
                ChangeEndDate = contractCostChange.ChangeEndDate,
                ChangeLimit = contractCostChange.ChangeLimit,
                FileName = contractCostChange.FileName,
                FilePath = contractCostChange.FilePath,
                Status = contractCostChange.Status,
                Description = contractCostChange.Description,
                IsLockedAttachment = contractCostChange.IsLockedAttachment,
                CurrentDiscount = contractCostChange.CurrentDiscount,
                ChangeType = contractCostChange.ChangeType,
                ChangeRoomCost = contractCostChange.ChangeRoomCost,
                ChangeMealsCost = contractCostChange.ChangeMealsCost,
                ChangeRatio = contractCostChange.ChangeRatio,
                IsIncluded = contractCostChange.IsIncluded
            };
            var discountAmount =
                _fetcher.Query<CustomerAccountDiscountLog>()
                    .Where(
                        x => x.CustomerAccount.Project == contractCostChange.Contract.Project && x.DiscountTime.Year == DateTime.Now.Year)
                    .ToList();
            viewModel.CustomerCurrentYearDiscount =
                discountAmount.Where(x => x.CustomerAccount == contractCostChange.Contract.CustomerAccount).Sum(s => s.DiscountAmount);
            viewModel.ProjectYearDiscount = discountAmount.Sum(x => x.DiscountAmount);

            viewModel.CurrentWorkFlowStep = _contractCostChangeWorkflow.GetCurrentWorkflowStep(contractCostChange);//当前审批步骤
            if (viewModel.CurrentWorkFlowStep != null)
            {
                viewModel.CurrentWorkflowStepId = viewModel.CurrentWorkFlowStep.Id;
            }
            
            viewModel.TrackingResult = new TrackingResult.TrackingResultViewModel
            {
                WorkflowTrackingResults = _contractCostChangeWorkflow.GetWorkflowTrackingResults(contractCostChange),
                WorkflowHistoryTrackingResults = _contractCostChangeWorkflow.GetWorkflowHistoryTrackingResults(contractCostChange)
            };
            return View("~/Views/ContractCostChange/Detail.cshtml", viewModel);
        }

        /// <summary>
        /// 打印费用变更
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Print(PrintContractCostChangeWordToPdfCommand command)
        {
            var result = _commandService.ExecuteFoResult(command);

            return Json(new
            {
                success = true,
                redirect = $"{Url.Content("~/Attachments/Print/")}{result.FileName}"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SubmitPrint(SubmitPrintContractCostChangeCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Detail", new { id = command.ContractCostChangeId });
        }

        public ActionResult GetChangeLimit(DateTime changeDate, int contractId, decimal changeRatio)
        {
            CurrentDiscountQuery query = new CurrentDiscountQuery()
            {
                ChangeDate = changeDate,
                ContractId = contractId,
                ChangeRatio = changeRatio
            };
            var changeCost = _currentDiscountQuery.GetChangeLimit(query);
            return Json(new { success = true, ChangeLimit = changeCost }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetChangeCost(DateTime changeDate, DateTime changeEndDate, decimal changeLimit)
        {
            CurrentDiscountQuery query = new CurrentDiscountQuery()
            {
                ChangeDate = changeDate,
                ChangeEndDate = changeEndDate,
                ChangeLimit = changeLimit
            };
            var changeCost = _currentDiscountQuery.GetCurrentDiscount(query);
            return Json(new { success = true, ChangeCost = changeCost }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PreviewPdf(string url)
        {
            string htmlUrl = Server.UrlDecode(url);
            return Redirect(Url.Content(htmlUrl));
        }

        /// <summary>
        /// 上传费用变更
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadContractCostChange(UploadContractCostChangeCommand command)
        {
            if (Request.Files.Count > 0)
            {
                command.FileBytes = Request.Files[0].ReadBytes();
                command.FileName = Request.Files[0].FileName;
            }
            _commandService.Execute(command);
            return RedirectToAction("Detail", new { id = command.ContractCostChangeId });
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Submit(SubmitContractCostChangeCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Detail", new { id = command.Id });
        }

        /// <summary>
        /// 创建并提交
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateAndSubmit(CreateAndSubmitContractCostChangeCommand command)
        {
            var result = _commandService.ExecuteFoResult(command);
            return RedirectToAction("Detail", new { id = result.ContractCostChangeId });
        }

        /// <summary>
        /// 确认
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Effective(EffectiveContractCostChangeCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Detail", new { id = command.ContractCostChangeId });
        }

        /// <summary>
        /// 审批通过
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Approval(ApprovalContractCostChangeCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Detail", new { id = command.ContractCostChangeId });
        }

        /// <summary>
        /// 资料合规性审批
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult FileApproval(FileApprovalContractCostChangeCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Detail", new { id = command.ContractCostChangeId });
        }
        
        /// <summary>
        /// 作废
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DraftAndDelete(DraftAndDeleteContractCostChangeCommand command)
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
        public ActionResult LockedAttachment(LockedContractCostChangeAttachmentCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Detail", new { id = command.ContractCostChangeId });
        }

        /// <summary>
        /// 全部附加协议上传
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UploadAdditional(int ralatedId, AdditionalType type,int customerAccountId)
        {
            var createCustomerLeaveViewModel = new CreateUploadProtocolPartialViewModel
            {
               Title = $"{type}附件补录上传",
               RalatedId = ralatedId,
               Type = type,
               CustomerAccountId = customerAccountId
            };
            return View("_UploadProtocol", createCustomerLeaveViewModel);
        }
        [HttpPost]
        public ActionResult UploadAdditional(CreateUploadProtocolPartialCommand command)
        {
            if (Request.Files.Count > 0)
            {
                command.FileBytes = Request.Files[0].ReadBytes();
                command.FileName = Request.Files[0].FileName;
            }
            _commandService.Execute(command);
            return RedirectToAction("Detail", "Customer", new { customerAccountId = command.CustomerAccountId });
        }
    }
}