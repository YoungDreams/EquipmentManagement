using System;
using System.Collections;
using System.Collections.Generic;
using Foundation.Data;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Query;
using PensionInsurance.Web.Views.Account;
using System.Linq;
using System.Web.Mvc;
using Foundation.Core;
using PensionInsurance.CommandHandlers;
using PensionInsurance.Entities.Exceptions;
using PensionInsurance.Shared;
using PensionInsurance.Web.Common;
using PensionInsurance.Workflows;

namespace PensionInsurance.Web.Views.Contract
{
    public class ContractController : AuthorizedController
    {
        private readonly ICommandService _commandService;
        private readonly IFetcher _fetcher;
        private readonly IContractQueryService _contractQueryService;
        private readonly IBuildingQueryService _buildingQueryService;
        private readonly IUnitQueryService _unitQueryService;
        private readonly IFloorQueryService _floorQueryService;
        private readonly IRoomQueryService _roomQueryService;
        private readonly IBedQueryService _bedQueryService;
        private readonly IServicePackCatalogQueryService _serviceProjectQueryService;
        private readonly IContractCostChangeQueryService _contractCostChangeQueryService;
        private readonly IContractRoomChangeQueryService _contractRoomChangeQueryService;
        private readonly IContractServicePackChangeQueryService _contractServicePackChangeQueryService;
        private readonly ISettingQueryService _settingQueryService;
        private readonly IProjectQueryService _projectQueryService;
        private readonly IUserQueryService _userQueryService;
        private readonly ContractWorkflow _contractWorkflow;
        private readonly ICustomerQueryService _customerQueryService;

        public ContractController(ICommandService commandService, IFetcher fetcher, IBuildingQueryService buildingQueryService, IUnitQueryService unitQueryService, IFloorQueryService floorQueryService, IRoomQueryService roomQueryService, IBedQueryService bedQueryService, IServicePackCatalogQueryService serviceProjectQueryService, IContractCostChangeQueryService contractCostChangeQueryService, IContractRoomChangeQueryService contractRoomChangeQueryService, IContractServicePackChangeQueryService contractServicePackChangeQueryService, IContractQueryService contractQueryService, ISettingQueryService settingQueryService, IProjectQueryService projectQueryService, IUserQueryService userQueryService, ContractWorkflow contractWorkflow, ICustomerQueryService customerQueryService)
        {
            _commandService = commandService;
            _fetcher = fetcher;
            _buildingQueryService = buildingQueryService;
            _unitQueryService = unitQueryService;
            _floorQueryService = floorQueryService;
            _roomQueryService = roomQueryService;
            _bedQueryService = bedQueryService;
            _serviceProjectQueryService = serviceProjectQueryService;
            _contractCostChangeQueryService = contractCostChangeQueryService;
            _contractRoomChangeQueryService = contractRoomChangeQueryService;
            _contractServicePackChangeQueryService = contractServicePackChangeQueryService;
            _contractQueryService = contractQueryService;
            _settingQueryService = settingQueryService;
            _projectQueryService = projectQueryService;
            _userQueryService = userQueryService;
            _contractWorkflow = contractWorkflow;
            _customerQueryService = customerQueryService;
        }

        public ActionResult Index(int page = 1, int pageSize = Web.Common.PaginationSetttings.PageSize, ContractQuery query = null)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.客户合同管理, Permission.查看))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var viewModel = new IndexViewModel(Url)
            {
                Query = query,
                Contractlist = _contractQueryService.Query(page, pageSize, query),
                ProjectList = _projectQueryService.QueryAllValidByProjectFilter().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                SaleUserList = _userQueryService.GetSalesUsers().Select(x => new SelectListItem
                {
                    Text = x.RealName,
                    Value = x.Id.ToString()
                })
            };
            return View("~/Views/Contract/Index.cshtml", viewModel);
        }

        /// <summary>
        /// 新建合同
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="contarctTemplate"></param>
        /// <param name="previousContractId"></param>
        /// <returns></returns>
        public ActionResult Create(int customerId, ContractTemplate contarctTemplate, int? previousContractId)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.客户合同管理, Permission.新增))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var customer = _customerQueryService.Get(customerId);

            var viewModel = new CreateViewModel
            {
                Customer = customer,
                CustomerId = customer.Id,
                ProjectId = null,
                BName = customer.Name,
                BAddress = customer.Address,
                BIDcard = customer.IDCard,
                BCredentialType = customer.CredentialType,
                UserId = Shared.WebAppContext.Current.User.Id,
                UserName = Shared.WebAppContext.Current.User.RealName,
                ContractTemplate = contarctTemplate,
                LongTermRoomCost = 0,
                ShortTermRoomCost = 0,
                ProjectViewModel = new ProjectViewModel
                {
                    Project = null,
                    Projects = _projectQueryService.QueryValidProjectsByType(ProjectType.CB).Select(x => x.MapToEntity<Project>())
                },
                PreviousContractId = previousContractId
            };
            if (previousContractId.HasValue)
            {
                var contract = _contractQueryService.Get(previousContractId.Value);
                viewModel.ProjectViewModel.Project = contract.Project.MapToEntity<Project>();
                viewModel.ProjectViewModel.PreviousContractId = previousContractId;
                viewModel.SignedType = SignedType.续签;
                viewModel.StartTime = contract.EndTime.Value.Date.AddDays(1);
                viewModel.EndTime = contract.EndTime.Value.Date.AddYears(1);

                if (contract.ContractTemplate == viewModel.ContractTemplate)
                {
                    viewModel.BSex = contract.BSex;
                    viewModel.BPhone = contract.BPhone;
                    viewModel.BEmail = contract.BEmail;
                    viewModel.BAge = contract.BAge;
                    viewModel.CName = contract.CName;
                    viewModel.CSex = contract.CSex;
                    viewModel.CRelationship = contract.CRelationship;
                    viewModel.CIDcard = contract.CIDcard;
                    viewModel.CPhone = contract.CPhone;
                    viewModel.CTel = contract.CTel;
                    viewModel.CCompany = contract.CCompany;
                    viewModel.CEmail = contract.CEmail;
                    viewModel.CPostcode = contract.CPostcode;
                    viewModel.CAddress = contract.CAddress;
                    viewModel.CliveAddress = contract.CliveAddress;
                    viewModel.CLegalPersonCompany = contract.CLegalPersonCompany;
                    viewModel.CLegalPersonContactName = contract.CLegalPersonContactName;
                    viewModel.CLegalPersonEmail = contract.CLegalPersonEmail;
                    viewModel.CLegalPersonName = contract.CLegalPersonName;
                    viewModel.CLegalPersonPhone = contract.CLegalPersonPhone;
                    viewModel.CLegalPersonTel = contract.CLegalPersonTel;
                    viewModel.CLegalPersonAddress = contract.CLegalPersonAddress;
                    viewModel.CLegalPersonPostcode = contract.CLegalPersonPostcode;
                    viewModel.DName = contract.DName;
                    viewModel.DSex = contract.DSex;
                    viewModel.DRelationship = contract.DRelationship;
                    viewModel.DIDcard = contract.DIDcard;
                    viewModel.DPhone = contract.DPhone;
                    viewModel.DTel = contract.DTel;
                    viewModel.DCompany = contract.DCompany;
                    viewModel.DEmail = contract.DEmail;
                    viewModel.DAddress = contract.DAddress;
                    viewModel.DliveAddress = contract.DliveAddress;
                    viewModel.DPostcode = contract.DPostcode;
                    viewModel.DLegalPersonCompany = contract.DLegalPersonCompany;
                    viewModel.DLegalPersonName = contract.DLegalPersonName;
                    viewModel.DLegalPersonEmail = contract.DLegalPersonEmail;
                    viewModel.DLegalPersonContactName = contract.DLegalPersonContactName;
                    viewModel.DLegalPersonPhone = contract.DLegalPersonPhone;
                    viewModel.DLegalPersonTel = contract.DLegalPersonTel;
                    viewModel.DLegalPersonAddress = contract.DLegalPersonAddress;
                    viewModel.DLegalPersonPostcode = contract.DLegalPersonPostcode;
                    viewModel.CCredentialType = contract.CCredentialType;
                    viewModel.DCredentialType = contract.DCredentialType;
                }
            }
            else
            {
                var contract = _fetcher.Query<Entities.Contract>().FirstOrDefault(x => x.Customer.Id == customerId && x.Status == ContractStatus.生效);
                if (contract != null)
                {
                    var project = contract.Project.MapToEntity<Project>();
                    viewModel.ProjectViewModel.Projects =
                        viewModel.ProjectViewModel.Projects.Where(x => x.Id != project.Id);
                }
            }

            if (viewModel.BCredentialType == CredentialType.居民身份证号 && !string.IsNullOrWhiteSpace(viewModel.BIDcard))
            {
                viewModel.BSex = viewModel.BIDcard.ToSex();
                viewModel.BAge = viewModel.BIDcard.ToAge();
            }
            else
            {
                viewModel.BSex = customer.Sex;
            }
            viewModel.ChargeType = ChargeType.标准收费.ToString();

            switch (contarctTemplate)
            {
                case ContractTemplate.养老机构服务合同20160101:
                    return View("~/Views/Contract/Create.cshtml", viewModel);
                case ContractTemplate.养老机构服务合同20170301:
                case ContractTemplate.养老机构服务合同20170801新签:
                case ContractTemplate.养老机构服务合同20170801换签:
                    return View("~/Views/Contract/Create20170301.cshtml", viewModel);
                default:
                    return View("~/Views/Contract/Create.cshtml", viewModel);
            }
        }

        /// <summary>
        /// 保存合同数据
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(CreateContractCommand command)
        {
            var result = _commandService.ExecuteFoResult(command);

            return RedirectToAction("Edit", new { id = result.ContractId });
        }

        /// <summary>
        /// 保存提交合同数据
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateSubmit(CreateAndSubmitContractCommand command)
        {
            var result = _commandService.ExecuteFoResult(command);
            return RedirectToAction("Detail", new { id = result.ContractId });
        }


        [HttpGet]
        public ActionResult DetailAll(int id)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.客户合同管理, Permission.查看))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var viewModel = PrepareDetailViewModel(id);

            viewModel.ContractCostChangeList = _contractCostChangeQueryService.Query(new ContractCostChangeQuery { ContractId = viewModel.ContractId });
            viewModel.ContractServicePackChangeList = _contractServicePackChangeQueryService.Query(new ContractServicePackChangeQuery { ContractId = viewModel.ContractId });
            viewModel.ContractRoomChangeList = _contractRoomChangeQueryService.QueryDetail(new ContractRoomChangeQuery { ContractId = viewModel.ContractId });

            return View("~/Views/Contract/DetailAll.cshtml", viewModel);
        }

        /// <summary>
        /// 合同详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Detail(int id)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.客户合同管理, Permission.查看))
            {
                return RedirectToAction("NoPermission", "Home");
            }
            return View("~/Views/Contract/Detail.cshtml", PrepareDetailViewModel(id));
        }

        /// <summary>
        /// 跳转到编辑页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.客户合同管理, Permission.新增))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var contract = _contractQueryService.Get(id);

            var viewModel = new EditViewModel(Url)
            {
                TypeList =
                    _settingQueryService.GetSettingsByType(SettingType.护理类型)
                        .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                ServiceLevelList =
                    _settingQueryService.GetSettingsByType(SettingType.服务包级别)
                        .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                Customer = contract.Customer,
                CustomerId = contract.Customer.Id,
                ProjectId = contract.Project.Id,
                ContractId = contract.Id,
                ContractNo = contract.ContractNo,
                SignedType = contract.SignedType,
                SignedOn = contract.SignedOn,
                ContractTemplate = contract.ContractTemplate,
                StartTime = contract.StartTime,
                EndTime = contract.EndTime.HasValue ? contract.EndTime.Value.Date : contract.EndTime,
                BName = contract.BName,
                BIDcard = contract.BIDcard,
                BSex = contract.BSex,
                BAddress = contract.BAddress,
                BPhone = contract.BPhone,
                BEmail = contract.BEmail,
                BAge = contract.BAge,
                CName = contract.CName,
                CSex = contract.CSex,
                CRelationship = contract.CRelationship,
                CIDcard = contract.CIDcard,
                CPhone = contract.CPhone,
                CTel = contract.CTel,
                CCompany = contract.CCompany,
                CEmail = contract.CEmail,
                CPostcode = contract.CPostcode,
                CAddress = contract.CAddress,
                CliveAddress = contract.CliveAddress,
                CLegalPersonCompany = contract.CLegalPersonCompany,
                CLegalPersonContactName = contract.CLegalPersonContactName,
                CLegalPersonEmail = contract.CLegalPersonEmail,
                CLegalPersonName = contract.CLegalPersonName,
                CLegalPersonPhone = contract.CLegalPersonPhone,
                CLegalPersonTel = contract.CLegalPersonTel,
                CLegalPersonAddress = contract.CLegalPersonAddress,
                CLegalPersonPostcode = contract.CLegalPersonPostcode,
                DName = contract.DName,
                DSex = contract.DSex,
                DRelationship = contract.DRelationship,
                DIDcard = contract.DIDcard,
                DPhone = contract.DPhone,
                DTel = contract.DTel,
                DCompany = contract.DCompany,
                DEmail = contract.DEmail,
                DAddress = contract.DAddress,
                DliveAddress = contract.DliveAddress,
                DPostcode = contract.DPostcode,
                DLegalPersonCompany = contract.DLegalPersonCompany,
                DLegalPersonName = contract.DLegalPersonName,
                DLegalPersonEmail = contract.DLegalPersonEmail,
                DLegalPersonContactName = contract.DLegalPersonContactName,
                DLegalPersonPhone = contract.DLegalPersonPhone,
                DLegalPersonTel = contract.DLegalPersonTel,
                DLegalPersonAddress = contract.DLegalPersonAddress,
                DLegalPersonPostcode = contract.DLegalPersonPostcode,
                BuildingId = contract.BuildingId,
                UnitId = contract.UnitId,
                FloorId = contract.FloorId,
                RoomId = contract.Room?.Id,
                BedId = contract.Bed?.Id,
                RoomType = contract.RoomType,
                IsCompartment = contract.IsCompartment,
                NursingType = contract.NursingType,
                ServicePackageLevel = contract.ServicePackageLevel,
                CheckinType = contract.CheckinType,
                DepositCost = contract.DepositCost,
                RelocationCost = contract.RelocationCost,
                ShortTermRoomCost = contract.ShortTermRoomCost,
                ShortTermMealsCost = contract.ShortTermMealsCost,
                ShortTermServiceCost = contract.ShortTermServiceCost,
                ShortTermServiceMonthlyCost = contract.ShortTermServiceMonthlyCost,
                ShortTermNursingCost = contract.ShortTermNursingCost,
                LongTermRoomCost = contract.LongTermRoomCost,
                LongTermMealsCost = contract.LongTermMealsCost,
                LongTermServiceCost = contract.LongTermServiceCost,
                LongTermServiceMonthlyCost = contract.LongTermServiceMonthlyCost,
                LongTermNursingCost = contract.LongTermNursingCost,
                RefundCost = contract.RefundCost,
                CreatedBy = contract.CreatedBy,
                Status = contract.Status,
                UserId = contract.User.Id,
                UserName = contract.User.RealName,
                LongTermAttachCost = contract.LongTermAttachCost,
                ShortTermAttachCost = contract.ShortTermAttachCost,
                LiquidatedDamages = contract.LiquidatedDamages,
                CAsTypes = contract.CAsTypes.SplitToList<int>(',').Select(x => (CAsType)x).ToList(),
                BCredentialType = contract.BCredentialType,
                CCredentialType = contract.CCredentialType,
                DCredentialType = contract.DCredentialType,
                DepositType = contract.DepositType,
                ChargeType = contract.ChargeType,
                ChargeDescription = contract.ChargeDescription,
                ServicePackCostDate = contract.ServicePackCostDate,
                ServiceCostDate = contract.ServiceCostDate,
                RoomCostDate = contract.RoomCostDate,
                MealsCostDate = contract.MealsCostDate,
                ProjectViewModel = new ProjectViewModel
                {
                    Project = contract.Project.MapToEntity<Project>(),
                    Projects = _projectQueryService.QueryValidProjectsByType(ProjectType.CB).Select(x => x.MapToEntity<Project>())
                },
                TrackingResult = new TrackingResult.TrackingResultViewModel
                {
                    WorkflowHistoryTrackingResults = _contractWorkflow.GetWorkflowHistoryTrackingResults(contract)
                }
            };
            if (contract.PreviousContract != null)
            {
                viewModel.PreviousContractId = contract.PreviousContract.Id;
                viewModel.ProjectViewModel.PreviousContractId = contract.PreviousContract.Id;
            }
            else
            {
                var oldContract = _fetcher.Query<Entities.Contract>().FirstOrDefault(x => x.Customer.Id == contract.Customer.Id && x.Status == ContractStatus.生效);
                if (oldContract != null)
                {
                    var prject = oldContract.Project.MapToEntity<Project>();
                    viewModel.ProjectViewModel.Projects =
                        viewModel.ProjectViewModel.Projects.Where(x => x.Id != prject.Id);
                }
            }
            PrepareRecommendCustomerAccount(contract, viewModel);

            if (contract.ContractTemplate == ContractTemplate.养老机构服务合同20160101)
            {
                return View("~/Views/Contract/Edit.cshtml", viewModel);
            }
            return View("~/Views/Contract/Edit20170301.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult EditBasic(int id)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.客户合同管理, Permission.编辑合同基础信息))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var contract = _contractQueryService.Get(id);

            var viewModel = new EditViewModel(Url)
            {
                TypeList =
                    _settingQueryService.GetSettingsByType(SettingType.护理类型)
                        .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                ServiceLevelList =
                    _settingQueryService.GetSettingsByType(SettingType.服务包级别)
                        .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                Customer = contract.Customer,
                CustomerId = contract.Customer.Id,
                ProjectId = contract.Project.Id,
                ContractId = contract.Id,
                ContractNo = contract.ContractNo,
                SignedType = contract.SignedType,
                SignedOn = contract.SignedOn,
                ContractTemplate = contract.ContractTemplate,
                StartTime = contract.StartTime,
                EndTime = contract.EndTime.Value.Date,
                BName = contract.BName,
                BIDcard = contract.BIDcard,
                BSex = contract.BSex,
                BAddress = contract.BAddress,
                BPhone = contract.BPhone,
                BEmail = contract.BEmail,
                BAge = contract.BAge,
                CName = contract.CName,
                CSex = contract.CSex,
                CRelationship = contract.CRelationship,
                CIDcard = contract.CIDcard,
                CPhone = contract.CPhone,
                CTel = contract.CTel,
                CCompany = contract.CCompany,
                CEmail = contract.CEmail,
                CPostcode = contract.CPostcode,
                CAddress = contract.CAddress,
                CliveAddress = contract.CliveAddress,
                CLegalPersonCompany = contract.CLegalPersonCompany,
                CLegalPersonContactName = contract.CLegalPersonContactName,
                CLegalPersonEmail = contract.CLegalPersonEmail,
                CLegalPersonName = contract.CLegalPersonName,
                CLegalPersonPhone = contract.CLegalPersonPhone,
                CLegalPersonTel = contract.CLegalPersonTel,
                CLegalPersonAddress = contract.CLegalPersonAddress,
                CLegalPersonPostcode = contract.CLegalPersonPostcode,
                DName = contract.DName,
                DSex = contract.DSex,
                DRelationship = contract.DRelationship,
                DIDcard = contract.DIDcard,
                DPhone = contract.DPhone,
                DTel = contract.DTel,
                DCompany = contract.DCompany,
                DEmail = contract.DEmail,
                DAddress = contract.DAddress,
                DliveAddress = contract.DliveAddress,
                DPostcode = contract.DPostcode,
                DLegalPersonCompany = contract.DLegalPersonCompany,
                DLegalPersonName = contract.DLegalPersonName,
                DLegalPersonEmail = contract.DLegalPersonEmail,
                DLegalPersonContactName = contract.DLegalPersonContactName,
                DLegalPersonPhone = contract.DLegalPersonPhone,
                DLegalPersonTel = contract.DLegalPersonTel,
                DLegalPersonAddress = contract.DLegalPersonAddress,
                DLegalPersonPostcode = contract.DLegalPersonPostcode,
                BuildingId = contract.BuildingId,
                UnitId = contract.UnitId,
                FloorId = contract.FloorId,
                RoomId = contract.Room?.Id,
                BedId = contract.Bed?.Id,
                RoomType = contract.RoomType,
                IsCompartment = contract.IsCompartment,
                NursingType = contract.NursingType,
                ServicePackageLevel = contract.ServicePackageLevel,
                CheckinType = contract.CheckinType,
                DepositCost = contract.DepositCost,
                RelocationCost = contract.RelocationCost,
                ShortTermRoomCost = contract.ShortTermRoomCost,
                ShortTermMealsCost = contract.ShortTermMealsCost,
                ShortTermServiceCost = contract.ShortTermServiceCost,
                ShortTermServiceMonthlyCost = contract.ShortTermServiceMonthlyCost,
                ShortTermNursingCost = contract.ShortTermNursingCost,
                LongTermRoomCost = contract.LongTermRoomCost,
                LongTermMealsCost = contract.LongTermMealsCost,
                LongTermServiceCost = contract.LongTermServiceCost,
                LongTermServiceMonthlyCost = contract.LongTermServiceMonthlyCost,
                LongTermNursingCost = contract.LongTermNursingCost,
                RefundCost = contract.RefundCost,
                CreatedBy = contract.CreatedBy,
                Status = contract.Status,
                UserId = contract.User.Id,
                UserName = contract.User.RealName,
                LongTermAttachCost = contract.LongTermAttachCost,
                ShortTermAttachCost = contract.ShortTermAttachCost,
                LiquidatedDamages = contract.LiquidatedDamages,
                CAsTypes = contract.CAsTypes.SplitToList<int>(',').Select(x => (CAsType)x).ToList(),
                BCredentialType = contract.BCredentialType,
                CCredentialType = contract.CCredentialType,
                DCredentialType = contract.DCredentialType,
                DepositType = contract.DepositType,
                ChargeType = contract.ChargeType,
                ChargeDescription = contract.ChargeDescription,
                ProjectViewModel = new ProjectViewModel
                {
                    Project = contract.Project.MapToEntity<Project>(),
                    Projects = _projectQueryService.QueryValidProjectsByType(ProjectType.CB).Select(x => x.MapToEntity<Project>()),
                    SignedType = contract.SignedType,
                }
            };
            if (contract.PreviousContract != null)
            {
                viewModel.PreviousContractId = contract.PreviousContract.Id;
                viewModel.ProjectViewModel.PreviousContractId = contract.PreviousContract.Id;
            }
            else
            {
                var oldContract = _fetcher.Query<Entities.Contract>().FirstOrDefault(x => x.Customer.Id == contract.Customer.Id && x.Status == ContractStatus.生效);
                if (oldContract != null)
                {
                    var prject = oldContract.Project.MapToEntity<Project>();
                    viewModel.ProjectViewModel.Projects =
                        viewModel.ProjectViewModel.Projects.Where(x => x.Id != prject.Id);
                }
            }
            PrepareRecommendCustomerAccount(contract, viewModel);

            viewModel.BuildingInfo = contract.Room?.Floor?.Unit?.Building;
            viewModel.UnitInfo = contract.Room?.Floor?.Unit;
            viewModel.FloorInfo = contract.Room?.Floor;
            viewModel.RoomInfo = contract.Room;
            viewModel.BedInfo = contract.Bed;
            viewModel.IsCompartmentInfo = contract.IsCompartment.Value;
            if (contract.ContractTemplate == ContractTemplate.养老机构服务合同20160101)
            {
                return View("~/Views/Contract/Edit.basic.cshtml", viewModel);
            }
            return View("~/Views/Contract/Edit.basic20170301.cshtml", viewModel);
        }

        private static void PrepareRecommendCustomerAccount(Entities.Contract contract, EditViewModel viewModel)
        {
            if (contract.RecommendCustomerAccount != null)
            {
                viewModel.RecommendCustomerAccount = contract.RecommendCustomerAccount;
                viewModel.CustomerAccountId = contract.RecommendCustomerAccount.Id;
                viewModel.RecommendPoint = contract.RecommendPoint;
                viewModel.BonusPoint = contract.BonusPoint;
            }
        }

        [HttpPost]
        public ActionResult Edit(EditContractCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Edit", new { id = command.ContractId });
        }

        [HttpPost]
        public ActionResult EditBasic(EditBasicContractCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("EditBasic", new { id = command.ContractId });
        }

        [HttpGet]
        public ActionResult GetBuildings(int? projectId)
        {
            var buildings = _buildingQueryService.Query(new BuildingQuery { ProjectId = projectId });
            return Json(buildings.Select(x => new
            {
                Value = x.Id,
                Text = x.Name
            }), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetUnits(int? buildingId)
        {
            var units = _unitQueryService.Query(new UnitQuery { BuildingId = buildingId });
            return Json(units.Select(x => new
            {
                Value = x.Id,
                Text = x.Name
            }), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetFloors(int? unitId)
        {
            var floors = _floorQueryService.Query(new FloorQuery { UnitId = unitId });
            return Json(floors.Select(x => new
            {
                Value = x.Id,
                Text = x.Name
            }), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetRooms(int projectId, int building, int unit, int floor, bool isCompartment, int contractId)
        {
            var rooms = _roomQueryService.Query(new RoomQuery { ProjectId = projectId, Building = building, Unit = unit, Floor = floor, IsCompartment = isCompartment, ContractId = contractId });

            return Json(rooms.Select(x => new
            {
                Value = x.RoomId,
                Text = x.RoomName,
                Type = x.RoomType
            }), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetBeds(int roomId, int contractId)
        {
            var beds = _bedQueryService.Query(new BedQuery { RoomId = roomId, ContractId = contractId });
            return Json(beds.Select(x => new
            {
                Value = x.Id,
                Text = x.Name,
            }), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetServicePackage(int projectId, string level,int? contractId)
        {
            ServicePackCost servicePackCost;
            if (contractId.HasValue)
            {
                var contract = _fetcher.Get<Entities.Contract>(contractId.Value);
                servicePackCost =_fetcher.Query<ServicePackCost>()
                        .FirstOrDefault(x => x.Project.Id == projectId && x.EnabledTime.Date == contract.ServicePackCostDate &&
                                x.Level == level);
                return Json(new
                {
                    ShortPrice = servicePackCost.ShortTermServicePackAmount,
                    LongPrice = servicePackCost.LongTermServicePackAmount,
                    MCLongPrice = servicePackCost.LongTermConcernAmount,
                    MCShortPrice = servicePackCost.ShortTermConcernAmount,
                    ServicePackCostDate=servicePackCost.EnabledTime.ToString()
                }, JsonRequestBehavior.AllowGet);
            }
            servicePackCost = _fetcher.Query<ServicePackCost>()
                       .FirstOrDefault(x => x.Project.Id == projectId && x.IsEnabled && x.Level == level);
            return Json(new
            {
                ShortPrice = servicePackCost.ShortTermServicePackAmount,
                LongPrice = servicePackCost.LongTermServicePackAmount,
                MCLongPrice = servicePackCost.LongTermConcernAmount,
                MCShortPrice = servicePackCost.ShortTermConcernAmount,
                ServicePackCostDate = servicePackCost.EnabledTime.ToString()
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SubmitContract(SubmitContractCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Detail", new { id = command.ContractId });
        }

        [HttpPost]
        public ActionResult ApprovalContract(ApprovalContractCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Detail", new { id = command.ContractId });
        }

        /// <summary>
        /// 打印合同
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PrintContract(PrintContractWordToPdfCommand command)
        {
            var result = _commandService.ExecuteFoResult(command);

            return Json(new
            {
                success = true,
                redirect = $"{Url.Content("~/Attachments/Print/")}{result.FileName}"
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 上传合同
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadContract(UploadContractCommand command)
        {
            if (Request.Files.Count > 0)
            {
                command.FileBytes = Request.Files[0].ReadBytes();
                command.FileName = Request.Files[0].FileName;
            }
            _commandService.Execute(command);
            return RedirectToAction("Detail", new { id = command.ContractId });
        }

        /// <summary>
        /// 上传提交合同
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EffectiveContract(EffectiveContractCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Detail", new { id = command.ContractId });
        }

        /// <summary>
        /// 确认附件上传
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LockedAttachment(LockedContractAttachmentCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Detail", new { id = command.ContractId });
        }

        /// <summary>
        /// 提交上传附件
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public ActionResult SubmitAttachment(SubmitContractAttachmentCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Detail", new { id = command.ContractId });
        }

        public ActionResult SubmitPrintContract(SubmitPrintContractCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Detail", new { id = command.ContractId });
        }

        [HttpPost]
        public ActionResult MoveOut(MoveOutCommand command)
        {
            command.Operator = Shared.WebAppContext.Current.User;
            _commandService.Execute(command);
            return RedirectToAction("Detail", "Customer", new { customerAccountId = command.CustomerAccountId });
        }

        [HttpGet]
        public PartialViewResult MoveOut(int id)
        {
            var contract = _contractQueryService.Get(id);

            var viewModel = new MoveOutViewModel(Url)
            {
                Contract = contract,
                MoveOutCommand = new MoveOutCommand
                {
                    ContractId = id
                },
                CustomerAccount = contract.CustomerAccount,
                Customer = contract.CustomerAccount.Customer
            };

            return PartialView("~/Views/Customer/_Customer.MoveOut.cshtml", viewModel);
        }


        [HttpPost]
        public ActionResult DraftAndDelete(DraftAndDeleteContractCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("IndexCheckIn", "Customer");
        }

        [HttpPost]
        public ActionResult DefrayCustomerMoneyProxy(DefrayCustomerMoneyProxyCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("MoveOut", new { id = command.ContractId });
        }

        [HttpPost]
        public ActionResult CalculateContractBill(CalculateContractBillCommand command)
        {
            var result = _commandService.ExecuteFoResult(command);
            return Json(new
            {
                success = true,
                ContractCount = result.ContractCount,
                BillCount = result.BillCount,
                RecalculateBillCount = result.RecalculateBillCount
            });
        }

        [HttpGet]
        public JsonResult ValidateContractAddtional(int customerAccountId, ContractAddtionalType? addtionalType)
        {
            var contract = _fetcher.Query<Entities.Contract>().FirstOrDefault(x => x.Status == ContractStatus.生效 && x.CustomerAccount.Id == customerAccountId);

            if (contract == null)
            {
                return Json(new { success = false, message = "未找到生效的合同信息" }, JsonRequestBehavior.AllowGet);
            }

            switch (addtionalType)
            {
                case ContractAddtionalType.费用变更补充协议:
                    if (_fetcher.Query<Entities.ContractCostChange>().Any(x => x.Contract.Id == contract.Id && x.Status != ContractAddtionalStatus.生效))
                    {
                        return Json(new { success = false, message = "已存在未确认的费用变更补充协议，请先确认" }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                case ContractAddtionalType.服务包补充协议:
                    if (_fetcher.Query<Entities.ContractServicePackChange>().Any(x => x.Contract.Id == contract.Id && x.Status != ContractAddtionalStatus.生效))
                    {
                        return Json(new { success = false, message = "已存在未确认的服务包补充协议，请先确认" }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                case ContractAddtionalType.换房补充协议:
                    if (_fetcher.Query<Entities.ContractRoomChange>().Any(x => x.Contract.Id == contract.Id && x.Status != ContractAddtionalStatus.生效))
                    {
                        return Json(new { success = false, message = "已存在未确认的换房补充协议，请先确认" }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                default:
                    return Json(new { success = false, message = "未知错误" }, JsonRequestBehavior.AllowGet);
            }
        }

        private DetailViewModel PrepareDetailViewModel(int id)
        {
            var contract = _contractQueryService.Get(id);
            var contractAttachment = _fetcher.Query<ContractAttachment>("SELECT * FROM ContractAttachment WHERE ContractId=@ContractId", new { ContractId = id }).FirstOrDefault();

            var viewModel = new DetailViewModel(Url)
            {
                TypeList =
                    _settingQueryService.GetSettingsByType(SettingType.护理类型)
                        .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                ServiceLevelList =
                    _settingQueryService.GetSettingsByType(SettingType.服务包级别)
                        .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),

                Customer = contract.Customer,
                CustomerAccount = contract.CustomerAccount,
                CustomerId = contract.Customer.Id,
                ProjectId = contract.Project.Id,
                ContractId = contract.Id,
                ContractNo = contract.ContractNo,
                SignedType = contract.SignedType,
                SignedOn = contract.SignedOn,
                ContractTemplate = contract.ContractTemplate,
                StartTime = contract.StartTime,
                EndTime = contract.EndTime,
                BName = contract.BName,
                BIDcard = contract.BIDcard,
                BSex = contract.BSex,
                BAddress = contract.BAddress,
                BAge = contract.BAge,
                BPhone = contract.BPhone,
                BEmail = contract.BEmail,
                CName = contract.CName,
                CSex = contract.CSex,
                CRelationship = contract.CRelationship,
                CIDcard = contract.CIDcard,
                CPhone = contract.CPhone,
                CTel = contract.CTel,
                CCompany = contract.CCompany,
                CEmail = contract.CEmail,
                CAddress = contract.CAddress,
                CliveAddress = contract.CliveAddress,
                CPostcode = contract.CPostcode,
                CLegalPersonCompany = contract.CLegalPersonCompany,
                CLegalPersonName = contract.CLegalPersonName,
                CLegalPersonEmail = contract.CLegalPersonEmail,
                CLegalPersonPhone = contract.CLegalPersonPhone,
                CLegalPersonTel = contract.CLegalPersonTel,
                CLegalPersonAddress = contract.CLegalPersonAddress,
                CLegalPersonContactName = contract.CLegalPersonContactName,
                CLegalPersonPostcode = contract.CLegalPersonPostcode,
                DName = contract.DName,
                DSex = contract.DSex,
                DRelationship = contract.DRelationship,
                DIDcard = contract.DIDcard,
                DPhone = contract.DPhone,
                DTel = contract.DTel,
                DCompany = contract.DCompany,
                DEmail = contract.DEmail,
                DAddress = contract.DAddress,
                DPostcode = contract.DPostcode,
                DliveAddress = contract.DliveAddress,
                DLegalPersonCompany = contract.DLegalPersonCompany,
                DLegalPersonName = contract.DLegalPersonName,
                DLegalPersonEmail = contract.DLegalPersonEmail,
                DLegalPersonContactName = contract.DLegalPersonContactName,
                DLegalPersonPhone = contract.DLegalPersonPhone,
                DLegalPersonTel = contract.DLegalPersonTel,
                DLegalPersonAddress = contract.DLegalPersonAddress,
                DLegalPersonPostcode = contract.DLegalPersonPostcode,
                BuildingId = contract.Room?.Floor?.Unit?.Building?.Id,
                UnitId = contract.Room?.Floor?.Unit?.Id,
                FloorId = contract.Room?.Floor?.Id,
                RoomId = contract.Room?.Id,
                BedId = contract.Bed?.Id,
                RoomType = contract.RoomType,
                IsCompartment = contract.IsCompartment,
                NursingType = contract.NursingType,
                ServicePackageLevel = contract.ServicePackageLevel,

                CheckinType = contract.CheckinType,
                DepositCost = contract.DepositCost,
                RelocationCost = contract.RelocationCost,
                ShortTermRoomCost = contract.ShortTermRoomCost,
                ShortTermMealsCost = contract.ShortTermMealsCost,
                ShortTermServiceCost = contract.ShortTermServiceCost,
                ShortTermServiceMonthlyCost = contract.ShortTermServiceMonthlyCost,
                ShortTermNursingCost = contract.ShortTermNursingCost,
                LongTermRoomCost = contract.LongTermRoomCost,
                LongTermMealsCost = contract.LongTermMealsCost,
                LongTermServiceCost = contract.LongTermServiceCost,
                LongTermServiceMonthlyCost = contract.LongTermServiceMonthlyCost,
                LongTermNursingCost = contract.LongTermNursingCost,
                RefundCost = contract.RefundCost,
                CreatedBy = contract.CreatedBy,
                Status = contract.Status,
                Project = contract.Project.MapToEntity<Project>(),
                UserName = contract.User.RealName,
                ShortTermAttachCost = contract.ShortTermAttachCost,
                LongTermAttachCost = contract.LongTermAttachCost,
                LiquidatedDamages = contract.LiquidatedDamages,
                CAsTypes = contract.CAsTypes.SplitToList<int>(',').Select(x => (CAsType)x).ToList(),
                BCredentialType = contract.BCredentialType,
                CCredentialType = contract.CCredentialType,
                DCredentialType = contract.DCredentialType,
                DepositType = contract.DepositType,
                ChargeType = contract.ChargeType,
                ChargeDescription = contract.ChargeDescription,
                ServicePackCostDate = contract.ServicePackCostDate,
                ServiceCostDate = contract.ServiceCostDate,
                RoomCostDate = contract.RoomCostDate,
                MealsCostDate = contract.MealsCostDate,
            };

            if (contract.RecommendCustomerAccount != null)
            {
                viewModel.CustomerAccountId = contract.RecommendCustomerAccount.Id;
            }

            viewModel.BuildingInfo = contract.Room?.Floor?.Unit?.Building;
            viewModel.UnitInfo = contract.Room?.Floor?.Unit;
            viewModel.FloorInfo = contract.Room?.Floor;
            viewModel.RoomInfo = contract.Room;
            viewModel.TrackingResult = new TrackingResult.TrackingResultViewModel
            {
                WorkflowTrackingResults = _contractWorkflow.GetWorkflowTrackingResults(contract),
                WorkflowHistoryTrackingResults = _contractWorkflow.GetWorkflowHistoryTrackingResults(contract)
            };

            viewModel.CurrentWorkFlowStep = _contractWorkflow.GetCurrentWorkflowStep(contract); //当前审批步骤
            if (viewModel.CurrentWorkFlowStep != null)
            {
                viewModel.CurrentWorkflowStepId = viewModel.CurrentWorkFlowStep.Id;
            }
            viewModel.BedInfo = contract.Bed;

            if (contractAttachment != null)
            {
                viewModel.AttachmentName = contractAttachment.Name;
                viewModel.AttachmentPath = contractAttachment.Path;
            }
            viewModel.CreateTime = contract.CreatedOn;
            viewModel.IsLockedAttachment = contract.IsLockedAttachment;

            viewModel.ActualEndTime = contract.ActualEndTime;
            viewModel.MoveOutReason = contract.MoveOutReason;

            PrepareRecommendCustomerAccount(contract, viewModel);

            return viewModel;
        }

        [HttpGet]
        public PartialViewResult ChooseContractTemplate(int customerId, bool? isReNew, int? customerAccountId)
        {
            var viewModel = new ChooseContractTemplateViewModel
            {
                CustomerId = customerId
            };
            if (customerAccountId.HasValue && isReNew.HasValue && isReNew.Value)
            {
                var contract =
                    _fetcher.Query<Entities.Contract>()
                        .FirstOrDefault(x => x.Status == ContractStatus.生效 && x.CustomerAccount.Id == customerAccountId);
                if (contract == null)
                {
                    throw new DomainValidationException("操作失败，当前客户不存在生效合同");
                }
                viewModel.PreviousContractId = contract.Id;
            }
            return PartialView("_Contract.Template", viewModel);
        }

        [HttpPost]
        public ActionResult ChooseContractTemplate(ChooseContractTemplateCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Create", "Contract", new { customerId = command.CustomerId, contarctTemplate = command.ContractTemplate, previousContractId = command.PreviousContractId });
        }

        [HttpGet]
        public ActionResult Delete(int contractId)
        {
            var contract = _contractQueryService.Get(contractId);
            DeleteContractViewModel viewModel = new DeleteContractViewModel
            {
                ContractId = contract.Id,
                ContractNo = contract.ContractNo,
                CustomerAccountId = contract.CustomerAccount.Id,
                CustomerName = contract.BName,
                StartTime = contract.StartTime,
                EndTime = contract.EndTime
            };

            return PartialView("_Contract.Delete", viewModel);
        }

        [HttpPost]
        public ActionResult Delete(DeleteContractCommand command)
        {
            command.Operator = Shared.WebAppContext.Current.User;
            _commandService.Execute(command);
            return RedirectToAction("Detail", "Customer", new { customerAccountId = command.CustomerAccountId });
        }

        public ActionResult GetLiquidatedDamages(decimal cost, int projectId)
        {
            var project = _projectQueryService.Get(projectId);
            var liquidatedDamages = project.LiquidatedDamagesRatio * cost * 12;
            return Json(new { success = true, LiquidatedDamages = liquidatedDamages }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSex(string idCard)
        {
            return Json(new { success = true, Sex = idCard.ToSex() }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSignedType(DateTime dt, int customerId)
        {
            var contarcts = _fetcher.Query<Entities.Contract>().Where(x => x.Customer.Id == customerId).ToList();

            if (contarcts.Where(x => x.Status == ContractStatus.生效).ToList().Any())
            {
                var contract = contarcts.Where(x => x.Status == ContractStatus.生效).OrderByDescending(o => o.EndTime).FirstOrDefault();
                if (contract != null && contract.EndTime.Value.Date.AddDays(1) >= dt.Date)
                {
                    return Json(new { success = true, SignedType = SignedType.续签.ToString() }, JsonRequestBehavior.AllowGet);
                }
            }
            if (contarcts.Where(x => x.Status == ContractStatus.失效).ToList().Any())
            {
                var contract = contarcts.Where(x => x.Status == ContractStatus.失效).OrderByDescending(o => o.ActualEndTime).FirstOrDefault();

                if (contract != null && contract.ActualEndTime.Value.Date.AddDays(1) >= dt.Date)
                {
                    return Json(new { success = true, SignedType = SignedType.续签.ToString() }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { success = true, SignedType = SignedType.新签.ToString() }, JsonRequestBehavior.AllowGet);
        }
    }
}