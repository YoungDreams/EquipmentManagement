using Foundation.Core;
using Foundation.Data;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Query;
using PensionInsurance.Web.Views.Account;
using System.Linq;
using System.Web.Mvc;
using PensionInsurance.Shared;
using PensionInsurance.Web.Common;
using PensionInsurance.Workflows;

namespace PensionInsurance.Web.Views.ContractRoomChange
{
    public class ContractRoomChangeController : AuthorizedController
    {
        private readonly ICommandService _commandService;
        private readonly IFetcher _fetcher;
        private readonly IBuildingQueryService _buildingQueryService;
        private readonly IUnitQueryService _unitQueryService;
        private readonly IFloorQueryService _floorQueryService;
        private readonly IRoomQueryService _roomQueryService;
        private readonly ContractRoomChangeWorkflow _contractRoomChangeWorkflow;

        public ContractRoomChangeController(ICommandService commandService, IFetcher fetcher, IBuildingQueryService buildingQueryService, IUnitQueryService unitQueryService, IFloorQueryService floorQueryService, IRoomQueryService roomQueryService, IBedQueryService bedQueryService, ContractRoomChangeWorkflow contractRoomChangeWorkflow)
        {
            _commandService = commandService;
            _fetcher = fetcher;
            _buildingQueryService = buildingQueryService;
            _unitQueryService = unitQueryService;
            _floorQueryService = floorQueryService;
            _roomQueryService = roomQueryService;
            _contractRoomChangeWorkflow = contractRoomChangeWorkflow;
        }

        public ActionResult Create(int contractId)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.换房补充协议, Permission.新增))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            // 合同信息
            var contract = _fetcher.Get<Entities.Contract>(contractId);
            var previousRoomChange = _fetcher.Query<Entities.ContractRoomChange>().Single(x => x.Contract.Id == contractId && x.Status == ContractAddtionalStatus.生效 && x.ChangeEndDate == contract.EndTime);
            var viewModel = new CreateViewModel
            {
                StartDate = previousRoomChange.ChangeDate.AddDays(1),
                EndDate = previousRoomChange.ChangeEndDate.AddDays(-1),
                BuildingList = _buildingQueryService.Query(new BuildingQuery { ProjectId = contract.Project.Id }),
                CustomerName = contract.Customer.Name,
                ProjectId = contract.Project.Id,
                ContractId = contractId,
                CustomerAccountId = contract.CustomerAccount.Id,
                CurrentRoomId = previousRoomChange.NewRoom.Id,
                CurrentBedId = previousRoomChange.NewBed?.Id,
                ShortServiceFee = contract.ShortTermServiceCost,
                ShortMeals = contract.ShortTermMealsCost,
                LongMeals = contract.LongTermMealsCost,
                LongServiceFee = contract.LongTermServiceCost
            };
            viewModel.ChargeType = ChargeType.标准收费.ToString();
            viewModel.Contract = contract;
            return View("~/Views/ContractRoomChange/Create.cshtml", viewModel);
        }

        /// <summary>
        /// 跳转到编辑页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.换房补充协议, Permission.新增))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var contractRoomChange = _fetcher.Get<Entities.ContractRoomChange>(id);

            var viewModel = new EditViewModel(Url)
            {
                BuildingList =
                    _buildingQueryService.Query(new BuildingQuery { ProjectId = contractRoomChange.Contract.Project.Id }),
                UnitList = _unitQueryService.Query(new UnitQuery { BuildingId = contractRoomChange.NewBuildingId }),
                FloorList = _floorQueryService.Query(new FloorQuery { UnitId = contractRoomChange.NewUnitId }),
                RoomList =
                    _roomQueryService.GetAvailableRooms(contractRoomChange.NewFloorId,
                        contractRoomChange.NewIsCompartment, contractRoomChange.Contract.Id),
                BedList =
                    _roomQueryService.GetAvailableBeds(contractRoomChange.NewRoom.Id, contractRoomChange.Contract.Id),
                // 换房信息
                ProjectId = contractRoomChange.Contract.Project.Id,
                ContractRoomChangeId = contractRoomChange.Id,
                ContractId = contractRoomChange.Contract.Id,
                CustomerAccountId = contractRoomChange.Contract.CustomerAccount.Id,
                ContractRoomChangeNo = contractRoomChange.ContractRoomChangeNo,
                CustomerName = contractRoomChange.CustomerName,
                ChangeDate = contractRoomChange.ChangeDate,
                ChangeEndDate = contractRoomChange.ChangeEndDate,
                NewBuildingId = contractRoomChange.NewBuildingId,
                NewUnitId = contractRoomChange.NewUnitId,
                NewFloorId = contractRoomChange.NewFloorId,
                NewRoomId = contractRoomChange.NewRoom.Id,
                NewRoomType = contractRoomChange.NewRoomType,
                NewBedId = contractRoomChange.NewBed?.Id,
                NewIsCompartment = contractRoomChange.NewIsCompartment,
                ShortMonthlyAmount = contractRoomChange.ShortMonthlyAmount,
                ShortMeals = contractRoomChange.ShortMeals,
                ShortRoomRate = contractRoomChange.ShortRoomRate,
                ShortServiceFee = contractRoomChange.ShortServiceFee,
                LongMonthlyAmount = contractRoomChange.LongMonthlyAmount,
                LongMeals = contractRoomChange.LongMeals,
                LongRoomRate = contractRoomChange.LongRoomRate,
                LongServiceFee = contractRoomChange.LongServiceFee,
                FileName = contractRoomChange.FileName,
                FilePath = contractRoomChange.FilePath,
                Status = contractRoomChange.Status,
                IsLockedAttachment = contractRoomChange.IsLockedAttachment,
                CurrentRoomId = contractRoomChange.NewRoom.Id,
                CurrentBedId = contractRoomChange.NewBed?.Id,
                ChargeDescription = contractRoomChange.ChargeDescription,
                ChargeType = contractRoomChange.ChargeType
            };
            viewModel.Contract = contractRoomChange.Contract;
            viewModel.TrackingResult = new TrackingResult.TrackingResultViewModel
            {
                WorkflowHistoryTrackingResults = _contractRoomChangeWorkflow.GetWorkflowHistoryTrackingResults(contractRoomChange)
            };
            return View("~/Views/ContractRoomChange/Edit.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult Detail(int id)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.换房补充协议, Permission.查看))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var contractRoomChange = _fetcher.Get<Entities.ContractRoomChange>(id);

            var viewModel = new EditViewModel(Url)
            {
                // 换房信息
                ProjectId = contractRoomChange.Contract.Project.Id,
                ContractRoomChangeId = contractRoomChange.Id,
                ContractId = contractRoomChange.Contract.Id,
                CustomerAccountId = contractRoomChange.Contract.CustomerAccount.Id,
                ContractRoomChangeNo = contractRoomChange.ContractRoomChangeNo,
                CustomerName = contractRoomChange.CustomerName,
                ChangeDate = contractRoomChange.ChangeDate,
                ChangeEndDate = contractRoomChange.ChangeEndDate,
                NewBuildingId = contractRoomChange.NewBuildingId,
                NewUnitId = contractRoomChange.NewUnitId,
                NewFloorId = contractRoomChange.NewFloorId,
                NewRoomId = contractRoomChange.NewRoom.Id,
                NewRoomType = contractRoomChange.NewRoomType,
                NewBedId = contractRoomChange.NewBed?.Id,
                NewIsCompartment = contractRoomChange.NewIsCompartment,
                ShortMonthlyAmount = contractRoomChange.ShortMonthlyAmount,
                ShortMeals = contractRoomChange.ShortMeals,
                ShortRoomRate = contractRoomChange.ShortRoomRate,
                ShortServiceFee = contractRoomChange.ShortServiceFee,
                LongMonthlyAmount = contractRoomChange.LongMonthlyAmount,
                LongMeals = contractRoomChange.LongMeals,
                LongRoomRate = contractRoomChange.LongRoomRate,
                LongServiceFee = contractRoomChange.LongServiceFee,
                FileName = contractRoomChange.FileName,
                FilePath = contractRoomChange.FilePath,
                Status = contractRoomChange.Status,
                IsLockedAttachment = contractRoomChange.IsLockedAttachment,

                BuildingInfo = contractRoomChange.NewRoom.Floor.Unit.Building,
                UnitInfo = contractRoomChange.NewRoom.Floor.Unit,
                FloorInfo = contractRoomChange.NewRoom.Floor,
                RoomInfo = contractRoomChange.NewRoom,
                BedInfo = contractRoomChange.NewBed,
                ChargeDescription = contractRoomChange.ChargeDescription,
                ChargeType = contractRoomChange.ChargeType
            };
            viewModel.Contract = contractRoomChange.Contract;
            viewModel.CurrentWorkFlowStep = _contractRoomChangeWorkflow.GetCurrentWorkflowStep(contractRoomChange);//当前审批步骤
            viewModel.TrackingResult = new TrackingResult.TrackingResultViewModel
            {
                WorkflowTrackingResults = _contractRoomChangeWorkflow.GetWorkflowTrackingResults(contractRoomChange),
                WorkflowHistoryTrackingResults = _contractRoomChangeWorkflow.GetWorkflowHistoryTrackingResults(contractRoomChange)
            };

            if (viewModel.CurrentWorkFlowStep != null)
            {
                viewModel.CurrentWorkflowStepId = viewModel.CurrentWorkFlowStep.Id;
            }
            return View("~/Views/ContractRoomChange/Detail.cshtml", viewModel);
        }

        /// <summary>
        /// 打印换房协议
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Print(PrintContractRoomChangeWordToPdfCommand command)
        {
            var result = _commandService.ExecuteFoResult(command);

            return Json(new
            {
                success = true,
                redirect = $"{Url.Content("~/Attachments/Print/")}{result.FileName}"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SubmitPrint(SubmitPrintContractRoomChangeCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Detail", new { id = command.ContractRoomChangeId });
        }

        /// <summary>
        /// 上传换房协议
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadContractRoomChange(UploadContractRoomChangeCommand command)
        {
            if (Request.Files.Count > 0)
            {
                command.FileBytes = Request.Files[0].ReadBytes();
                command.FileName = Request.Files[0].FileName;
            }
            _commandService.Execute(command);
            return RedirectToAction("Detail", new { id = command.ContractRoomChangeId });
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Submit(SubmitContractRoomChangeCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Detail", new { id = command.ContractRoomChangeId });
        }

        [HttpPost]
        public ActionResult CreateAndSubmit(CreateAndSubmitContractRoomChangeCommand command)
        {
            var result = _commandService.ExecuteFoResult(command);
            return RedirectToAction("Detail", new { id = result.ContractRoomChangeId });
        }

        /// <summary>
        /// 确认
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Effective(EffectiveContractRoomChangeCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Detail", new { id = command.ContractRoomChangeId });
        }

        /// <summary>
        /// 审批通过
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Approval(ApprovalContractRoomChangeCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Detail", new { id = command.ContractRoomChangeId });
        }

        /// <summary>
        /// 作废
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DraftAndDelete(DraftAndDeleteContractRoomChangeCommand command)
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
        public ActionResult LockedAttachment(LockedContractRoomChangeAttachmentCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Detail", new { id = command.ContractRoomChangeId });
        }
    }
}