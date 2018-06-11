using System;
using System.Web.Mvc;
using System.Linq;
using Foundation.Data;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Query;
using PensionInsurance.Shared;
using PensionInsurance.Web.Common;
using PensionInsurance.Web.Views.Account;

namespace PensionInsurance.Web.Views.SystemSetting.Project
{
    public class
        ProjectController : AuthorizedController
    {
        private readonly ICommandService _commandService;
        private readonly IProjectQueryService _projectQueryService;
        private readonly IWorkflowQueryService _workflowQueryService;
        private readonly IUserQueryService _userQueryService;
        private readonly IJobTypeQueryService _jobTypeQueryService;
        private readonly IAreaQueryService _areaQueryService;
        private readonly IManagementRegionService _managementRegionService;
        private readonly IFetcher _fetcher;
        public ProjectController(ICommandService commandService, IProjectQueryService projectQueryService, IWorkflowQueryService workflowQueryService,
            IUserQueryService userQueryService, IJobTypeQueryService jobTypeQueryService, IAreaQueryService areaQueryService, IManagementRegionService managementRegionService, IFetcher fetcher)
        {
            _commandService = commandService;
            _projectQueryService = projectQueryService;
            _workflowQueryService = workflowQueryService;
            _userQueryService = userQueryService;
            _jobTypeQueryService = jobTypeQueryService;
            _areaQueryService = areaQueryService;
            _managementRegionService = managementRegionService;
            _fetcher = fetcher;
        }

        /// <summary>
        /// 查询页面
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public ActionResult Index(int page = 1, int pageSize = Web.Common.PaginationSetttings.PageSize,
            ProjectQuery query = null)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.项目管理, Permission.查看))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var viewModel = new IndexViewModel(Url)
            {
                Query = query,
                Projects = _projectQueryService.Query(page, pageSize, query),
                JobOccupations = _jobTypeQueryService.GeTypes().ToList()
            };
            return View("~/Views/SystemSetting/Project/Index.cshtml", viewModel);
        }

        [HttpGet]
        public PartialViewResult Configure(int projectId, ConfigureCategory configureCategory)
        {
            var viewModel = new EditConfigureViewModel
            {
                Category = configureCategory,
                Users = _userQueryService.GetUsersByProjectId(projectId).Select(x => new SelectListItem
                {
                    Text = x.RealName,
                    Value = x.Id.ToString()
                }),
            };
            viewModel.UserSelectListItems = _fetcher.Query<UserNotificationConfigure>()
                    .Where(x => x.ConfigureProject.Id == projectId && x.ConfigureCategory == configureCategory)
                    .ToList().Select(
                    x => new SelectListItem { Text = x.ConfigureUser.RealName, Value = x.ConfigureUser.Id.ToString() });

            return PartialView("~/Views/systemSetting/Project/_User.Configure.cshtml", viewModel);
        }

        [HttpPost]
        public void Configure(EditConfigureCommand command)
        {
            command.Operator = WebAppContext.Current.User;
            _commandService.Execute(command);
        }
        /// <summary>
        /// 跳转到新增页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create()
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.项目管理, Permission.新增))
            {
                return RedirectToAction("NoPermission", "Home");
            }
            var viewModel = new CreateViewModel();

            viewModel.Cities = _areaQueryService.QueryCities()
                .Select(x => new SelectListItem { Text = x.ShortName, Value = x.Id.ToString() });
            viewModel.ManagementRegions = _managementRegionService.QueryAll()
                .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });

            viewModel.ReminderDateOfDay = 21;

            return View("~/Views/SystemSetting/Project/Create.cshtml", viewModel);
        }

        /// <summary>
        /// 新增处理
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(CreateProjectCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 跳转到编辑页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.项目管理, Permission.编辑))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var projectInfo = _projectQueryService.Get(id);

            var viewModel = new EditViewModel
            {
                ProjectId = projectInfo.Id,
                ProjectNo = projectInfo.ProjectNo,
                Name = projectInfo.Name,
                Address = projectInfo.Address,
                City = projectInfo.City?.Id,
                ManagementRegion = projectInfo.ManagementRegion?.Id,
                CompanyName = projectInfo.CompanyName,
                CompanyAddress = projectInfo.CompanyAddress,
                CompanyCorporation = projectInfo.CompanyCorporation,
                CompanyTel = projectInfo.CompanyTel,
                CompanyAccountName = projectInfo.CompanyAccountName,
                CompanyAccount = projectInfo.CompanyAccount,
                CompanyAccountBank = projectInfo.CompanyAccountBank,
                PensionAddress = projectInfo.PensionAddress,
                Status = projectInfo.Status,
                ProjectType = projectInfo.ProjectType,
                ProjectFullName = projectInfo.ProjectFullName,
                BuildingCount = projectInfo.BuildingCount,
                UnitCount = projectInfo.UnitCount,
                FloorCount = projectInfo.FloorCount,
                PosCardRate = projectInfo.PosCardRate,
                MaxPosCardCost = projectInfo.MaxPosCardCost,
                SpecialExemptionLimit = projectInfo.SpecialExemptionLimit,
                IntegralLimit = projectInfo.IntegralLimit,
                RelocationCost = projectInfo.RelocationCost,
                RefundCost = projectInfo.RefundCost,
                ShortTermServiceCost = projectInfo.ShortTermServiceCost,
                ShortTermMealsCost = projectInfo.ShortTermMealsCost,
                LongTermMealsCost = projectInfo.LongTermMealsCost,
                LongTermServiceCost = projectInfo.LongTermServiceCost,
                SpecialExemptionLimitTotal = projectInfo.SpecialExemptionLimitTotal,
                PersonalSpecialExemptionLimitTotal = projectInfo.PersonalSpecialExemptionLimitTotal,
                LiquidatedDamagesRatio = projectInfo.LiquidatedDamagesRatio,
                BonusPoint = projectInfo.BonusPoint,
                RecommendPoint = projectInfo.RecommendPoint,
                Email = projectInfo.Email,
                PostCode = projectInfo.PostCode,
                FirstDeposit = projectInfo.FirstDeposit,
                SecondDeposit = projectInfo.SecondDeposit,
                NCCompanyCode = projectInfo.NCCompanyCode,
                NCDebitAccountCode = projectInfo.NCDebitAccountCode,
                NCCreditRoomAccountCode = projectInfo.NCCreditRoomAccountCode,
                NCCreditBasicallyAccountCode = projectInfo.NCCreditBasicallyAccountCode,
                NCCreditIncrementAccountCode = projectInfo.NCCreditIncrementAccountCode,
                NCCreditMealsAccountCode = projectInfo.NCCreditMealsAccountCode,
                NCCreditServiceAccountCode = projectInfo.NCCreditServiceAccountCode,
                NCEnter = projectInfo.NCEnter,
                NCDebitRelocationAccountCode = projectInfo.NCDebitRelocationAccountCode,
                NCCreditRelocationAccountCode = projectInfo.NCCreditRelocationAccountCode,
                ReminderDateOfDay = projectInfo.ReminderDateOfDay,
                Sort = projectInfo.Sort,
                HeaderText = "编辑"
            };

            viewModel.Cities = _areaQueryService.QueryCities()
                .Select(x => new SelectListItem { Text = x.ShortName, Value = x.Id.ToString() });
            viewModel.ManagementRegions = _managementRegionService.QueryAll()
                .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });

            return View("~/Views/SystemSetting/Project/Edit.cshtml", viewModel);
        }

        /// <summary>
        /// 编辑提交
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(EditProjectCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 删除处理
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(DeleteEntityCommand command)
        {
            Entities.Project project = _projectQueryService.Get(command.EntityId);
            if (project == null)
                throw new ApplicationException("Project cannot be found");
            _commandService.Execute(DeleteEntityCommand.Of(project));
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 启用
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ValidProjects(ValidProjectCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 失效
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InValidProjects(InvalidProjectCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public PartialViewResult EditWorkflowEmployee(int projectId, WorkflowCategory workflowCategory)
        {
            EditWorkflowContractViewModel viewModel = new EditWorkflowContractViewModel();
            viewModel.WorkflowCategory = workflowCategory;
            viewModel.WorkflowDetail = _workflowQueryService.QueryWorkflowDetail(projectId, workflowCategory, WorkflowStepCategory.审批);
            viewModel.Users = _userQueryService.GetUsersByProjectId(projectId).Select(x => new SelectListItem
            {
                Text = x.RealName,
                Value = x.Id.ToString()
            });
            var apartmentGeneralManagerStep = _workflowQueryService.QueryWorkflowDetail(projectId, workflowCategory, WorkflowStepCategory.公寓总经理审批)
                     .FirstOrDefault();
            if (apartmentGeneralManagerStep != null)
            {
                viewModel.ApartmentGeneralManagerUserId = apartmentGeneralManagerStep.UserId;
            }
            var districtGovernorStep = _workflowQueryService.QueryWorkflowDetail(projectId, workflowCategory, WorkflowStepCategory.大区总监审批)
                     .FirstOrDefault();
            if (districtGovernorStep != null)
            {
                viewModel.DistrictGovernorUserId = districtGovernorStep.UserId;
            }
            var centerSalaryManagerStep = _workflowQueryService.QueryWorkflowDetail(projectId, workflowCategory, WorkflowStepCategory.中心薪酬经理审批)
                     .FirstOrDefault();
            if (centerSalaryManagerStep != null)
            {
                viewModel.CenterSalaryManagerUserId = centerSalaryManagerStep.UserId;
            }

            var centerExecutiveStep = _workflowQueryService.QueryWorkflowDetail(projectId, workflowCategory, WorkflowStepCategory.中心人力行政总监审批)
                     .FirstOrDefault();
            if (centerExecutiveStep != null)
            {
                viewModel.CenterExecutiveUserId = centerExecutiveStep.UserId;
            }

            var generalManagerStep = _workflowQueryService.QueryWorkflowDetail(projectId, workflowCategory, WorkflowStepCategory.公司总经理审批)
                     .FirstOrDefault();
            if (generalManagerStep != null)
            {
                viewModel.GeneralManagerUserId = generalManagerStep.UserId;
            }
            var entryApprovalStep = _workflowQueryService.QueryWorkflowDetail(projectId, workflowCategory, WorkflowStepCategory.入职办理审批)
                     .FirstOrDefault();
            if (entryApprovalStep != null)
            {
                viewModel.EntryApprovalUserId = entryApprovalStep.UserId;
            }


            return PartialView("~/Views/SystemSetting/Project/_Workflow.Employee.cshtml", viewModel);
        }

        [HttpPost]
        public void EditWorkflowEmployee(EditWorkflowEmployeeCommand command)
        {
            command.Operator = WebAppContext.Current.User;
            _commandService.Execute(command);
        }


        [HttpGet]
        public PartialViewResult EditWorkflowContract(int projectId, WorkflowCategory workflowCategory)
        {
            EditWorkflowContractViewModel viewModel = new EditWorkflowContractViewModel();
            viewModel.WorkflowCategory = workflowCategory;
            viewModel.WorkflowDetail = _workflowQueryService.QueryWorkflowDetail(projectId, workflowCategory, WorkflowStepCategory.审批);

            if (workflowCategory == WorkflowCategory.客户退住通知单)
            {
                viewModel.UserSelectListItems = _fetcher.Query<UserNotificationConfigure>()
                    .Where(x => x.ConfigureProject.Id == projectId && x.ConfigureCategory == ConfigureCategory.退住通知单)
                    .ToList().Select(
                        x => new SelectListItem { Text = x.ConfigureUser.RealName, Value = x.ConfigureUser.Id.ToString() });
            }
            if (workflowCategory == WorkflowCategory.食材采购审批)
            {
                viewModel.UserSelectListItems = _fetcher.Query<UserNotificationConfigure>()
                    .Where(x => x.ConfigureProject.Id == projectId && x.ConfigureCategory == ConfigureCategory.食材采购)
                    .ToList().Select(
                        x => new SelectListItem { Text = x.ConfigureUser.RealName, Value = x.ConfigureUser.Id.ToString() });
            }
            if (workflowCategory == WorkflowCategory.营销类计划采购验收)
            {
                viewModel.UserSelectListItems = _fetcher.Query<UserNotificationConfigure>()
                    .Where(x => x.ConfigureProject.Id == projectId && x.ConfigureCategory == ConfigureCategory.营销类计划采购验收)
                    .ToList().Select(
                        x => new SelectListItem { Text = x.ConfigureUser.RealName, Value = x.ConfigureUser.Id.ToString() });
            }
            if (workflowCategory == WorkflowCategory.营销类特殊采购验收)
            {
                viewModel.UserSelectListItems = _fetcher.Query<UserNotificationConfigure>()
                    .Where(x => x.ConfigureProject.Id == projectId && x.ConfigureCategory == ConfigureCategory.营销类特殊采购验收)
                    .ToList().Select(
                        x => new SelectListItem { Text = x.ConfigureUser.RealName, Value = x.ConfigureUser.Id.ToString() });
            }
            if (workflowCategory == WorkflowCategory.营销类紧急采购验收)
            {
                viewModel.UserSelectListItems = _fetcher.Query<UserNotificationConfigure>()
                    .Where(x => x.ConfigureProject.Id == projectId && x.ConfigureCategory == ConfigureCategory.营销类紧急采购验收)
                    .ToList().Select(
                        x => new SelectListItem { Text = x.ConfigureUser.RealName, Value = x.ConfigureUser.Id.ToString() });
            }

            if (workflowCategory == WorkflowCategory.营销类计划采购审批)
            {
                viewModel.UserSelectListItems = _fetcher.Query<UserNotificationConfigure>()
                    .Where(x => x.ConfigureProject.Id == projectId && x.ConfigureCategory == ConfigureCategory.营销类计划采购审批)
                    .ToList().Select(
                        x => new SelectListItem { Text = x.ConfigureUser.RealName, Value = x.ConfigureUser.Id.ToString() });
            }
            if (workflowCategory == WorkflowCategory.营销类特殊采购审批)
            {
                viewModel.UserSelectListItems = _fetcher.Query<UserNotificationConfigure>()
                    .Where(x => x.ConfigureProject.Id == projectId && x.ConfigureCategory == ConfigureCategory.营销类特殊采购审批)
                    .ToList().Select(
                        x => new SelectListItem { Text = x.ConfigureUser.RealName, Value = x.ConfigureUser.Id.ToString() });
            }
            if (workflowCategory == WorkflowCategory.营销类紧急采购审批)
            {
                viewModel.UserSelectListItems = _fetcher.Query<UserNotificationConfigure>()
                    .Where(x => x.ConfigureProject.Id == projectId && x.ConfigureCategory == ConfigureCategory.营销类紧急采购审批)
                    .ToList().Select(
                        x => new SelectListItem { Text = x.ConfigureUser.RealName, Value = x.ConfigureUser.Id.ToString() });
            }

            viewModel.Users = _userQueryService.GetUsersByProjectId(projectId).Select(x => new SelectListItem
            {
                Text = x.RealName,
                Value = x.Id.ToString()
            });

            var uploadWorkflowStep =
                _workflowQueryService.QueryWorkflowDetail(projectId, workflowCategory, WorkflowStepCategory.扫描件上传)
                    .FirstOrDefault();
            if (uploadWorkflowStep != null)
            {
                viewModel.UploadUserId = uploadWorkflowStep.UserId;
            }

            var confirmWorkflowStep =
                _workflowQueryService.QueryWorkflowDetail(projectId, workflowCategory, WorkflowStepCategory.扫描件确认)
                    .FirstOrDefault();
            if (confirmWorkflowStep != null)
            {
                viewModel.ConfirmUserId = confirmWorkflowStep.UserId;
            }
            var printtWorkflowStep =
                _workflowQueryService.QueryWorkflowDetail(projectId, workflowCategory, WorkflowStepCategory.打印签字)
                    .FirstOrDefault();
            if (printtWorkflowStep != null)
            {
                viewModel.PrintUserId = printtWorkflowStep.UserId;
            }

            var effectWorkflowStep =
                _workflowQueryService.QueryWorkflowDetail(projectId, workflowCategory, WorkflowStepCategory.生效)
                    .FirstOrDefault();
            if (effectWorkflowStep != null)
            {
                viewModel.EffectUserId = effectWorkflowStep.UserId;
            }

            var refundWorkflowStep =
                _workflowQueryService.QueryWorkflowDetail(projectId, workflowCategory, WorkflowStepCategory.退款)
                    .FirstOrDefault();
            if (refundWorkflowStep != null)
            {
                viewModel.RefundUserId = refundWorkflowStep.UserId;
            }

            var livingHealthManagerStep =
                _workflowQueryService.QueryWorkflowDetail(projectId, workflowCategory, WorkflowStepCategory.填写退住总结)
                    .FirstOrDefault();
            if (livingHealthManagerStep != null)
            {
                viewModel.LivingHealthManagerUserId = livingHealthManagerStep.UserId;
            }

            var apartmentGeneralManagerStep = _workflowQueryService.QueryWorkflowDetail(projectId, workflowCategory, WorkflowStepCategory.填写退住意见)
                     .FirstOrDefault();
            if (apartmentGeneralManagerStep != null)
            {
                viewModel.ApartmentGeneralManagerUserId = apartmentGeneralManagerStep.UserId;
            }

            var apartmentApprovalWorkflowStep =
                _workflowQueryService.QueryWorkflowDetail(projectId, workflowCategory, WorkflowStepCategory.公寓审批)
                    .FirstOrDefault();
            if (apartmentApprovalWorkflowStep != null)
            {
                viewModel.ApartmentApprovalUserId = apartmentApprovalWorkflowStep.UserId;
            }

            var purchaseWorkflowStep =
               _workflowQueryService.QueryWorkflowDetail(projectId, workflowCategory, WorkflowStepCategory.运营采购负责人审批)
                   .FirstOrDefault();
            if (purchaseWorkflowStep != null)
            {
                viewModel.PurchaseUserId = purchaseWorkflowStep.UserId;
            }

            var housekeeperWorkflowStep =
              _workflowQueryService.QueryWorkflowDetail(projectId, workflowCategory, WorkflowStepCategory.管家入库)
                  .FirstOrDefault();
            if (housekeeperWorkflowStep != null)
            {
                viewModel.HousekeeperUserId = housekeeperWorkflowStep.UserId;
            }

            var financeWorkflowStep =
                _workflowQueryService.QueryWorkflowDetail(projectId, workflowCategory, WorkflowStepCategory.财务总监审批)
                    .FirstOrDefault();
            if (financeWorkflowStep != null)
            {
                viewModel.FinanceUserId = financeWorkflowStep.UserId;
            }
            //MarketingDirectorApprovalUserId
            var marketingDirectorWorkflowStep =
                _workflowQueryService.QueryWorkflowDetail(projectId, workflowCategory, WorkflowStepCategory.营销业务审批)
                    .FirstOrDefault();
            if (marketingDirectorWorkflowStep != null)
            {
                viewModel.MarketingDirectorApprovalUserId = marketingDirectorWorkflowStep.UserId;
            }

            var salesWorkflowStep =
                _workflowQueryService.QueryWorkflowDetail(projectId, workflowCategory, WorkflowStepCategory.销售总监审批)
                    .FirstOrDefault();
            if (salesWorkflowStep != null)
            {
                viewModel.SalesApprovalUserId = salesWorkflowStep.UserId;
            }

            var marketDirectorWorkflowStep =
                _workflowQueryService.QueryWorkflowDetail(projectId, workflowCategory, WorkflowStepCategory.市场总监审批)
                    .FirstOrDefault();
            if (marketDirectorWorkflowStep != null)
            {
                viewModel.MarketDirectorApprovalUserId = marketDirectorWorkflowStep.UserId;
            }

            var projectCaterersWorkflowStep =
                _workflowQueryService.QueryWorkflowDetail(projectId, workflowCategory, WorkflowStepCategory.项目餐饮负责人确认)
                    .FirstOrDefault();
            if (projectCaterersWorkflowStep != null)
            {
                viewModel.ProjectCaterersUserId = projectCaterersWorkflowStep.UserId;
            }

            var directorAcctouningConfirmWorkflowStep =
                _workflowQueryService.QueryWorkflowDetail(projectId, workflowCategory, WorkflowStepCategory.主管会计确认)
                    .FirstOrDefault();
            if (directorAcctouningConfirmWorkflowStep != null)
            {
                viewModel.DirectorAccountingConfirmUserId = directorAcctouningConfirmWorkflowStep.UserId;
            }

            var assetManagerWorkflowStep =
                _workflowQueryService.QueryWorkflowDetail(projectId, workflowCategory, WorkflowStepCategory.资产管理员入库)
                    .FirstOrDefault();
            if (assetManagerWorkflowStep != null)
            {
                viewModel.AssetManagerUserId = assetManagerWorkflowStep.UserId;
            }
            return PartialView("~/Views/SystemSetting/Project/_Workflow.Contract.cshtml", viewModel);
        }

        [HttpPost]
        public void EditWorkflowContract(EditWorkflowContractCommand command)
        {
            command.Operator = WebAppContext.Current.User;
            _commandService.Execute(command);
        }

        [HttpPost]
        // ReSharper disable once InconsistentNaming
        public void CreateHRProjectJobOccupation(CreateHRProjectJobOccupationCommand command)
        {
            _commandService.Execute(command);
        }

        [HttpGet]
        public JsonResult Detail(int id)
        {
            return new JsonNetResult
            {
                Data = _projectQueryService.Get(id),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public JsonResult GetMealsCost(int projectId, int? contractId)
        {
            MealsCost mealscost;
            if (contractId.HasValue)
            {
                var contract = _fetcher.Get<Entities.Contract>(contractId.Value);

                mealscost = _fetcher.Query<MealsCost>()
                    .FirstOrDefault(x => x.EnabledTime.Date == contract.MealsCostDate.Value.Date && x.Project.Id == projectId);
                return Json(new { LongTermMealsCost = mealscost.LongTermMealsAmount, ShortTermMealsCost = mealscost.ShortTermMealsAmount, MealsCostDate = mealscost.EnabledTime.ToString() }, JsonRequestBehavior.AllowGet);
            }

            mealscost = _fetcher.Query<MealsCost>()
                .FirstOrDefault(x => x.IsEnabled && x.Project.Id == projectId);
            return Json(new { LongTermMealsCost = mealscost.LongTermMealsAmount, ShortTermMealsCost = mealscost.ShortTermMealsAmount, MealsCostDate = mealscost.EnabledTime.ToString() }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetServiceCost(int projectId, int? contractId)
        {
            ServiceCost serviceCost;
            if (contractId.HasValue)
            {
                var contract = _fetcher.Get<Entities.Contract>(contractId.Value);

                serviceCost = _fetcher.Query<ServiceCost>()
                    .FirstOrDefault(x => x.EnabledTime.Date == contract.ServiceCostDate.Value.Date && x.Project.Id == projectId);
                return Json(new { LongTermServiceCost = serviceCost.LongTermServiceAmount, ShortTermServiceCost = serviceCost.ShortTermServiceAmount, ServiceCostDate = serviceCost.EnabledTime.ToString() }, JsonRequestBehavior.AllowGet);
            }

            serviceCost = _fetcher.Query<ServiceCost>()
                .FirstOrDefault(x => x.IsEnabled && x.Project.Id == projectId);
            return Json(new { LongTermServiceCost = serviceCost.LongTermServiceAmount, ShortTermServiceCost = serviceCost.ShortTermServiceAmount, ServiceCostDate = serviceCost.EnabledTime.ToString() }, JsonRequestBehavior.AllowGet);
        }
    }
}