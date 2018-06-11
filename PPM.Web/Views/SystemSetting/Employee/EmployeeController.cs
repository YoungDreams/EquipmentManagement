using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Foundation.Data;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Query;
using PensionInsurance.Web.Common;
using PensionInsurance.Web.Views.Account;
using Foundation.Core;
using PensionInsurance.Entities;
using PensionInsurance.Shared;
using PensionInsurance.Workflows;

namespace PensionInsurance.Web.Views.SystemSetting.Employee
{
    public class EmployeeController : AuthorizedController
    {
        private readonly ICommandService _commandService;
        private readonly IFetcher _fetcher;
        private readonly IEmployeeQueryService _employeeQueryService;
        private readonly IJobTypeQueryService _jobTypeQueryService;
        private readonly IDepartmentQueryService _departmentQueryService;
        private readonly IProjectQueryService _projectQueryService;
        private readonly EmployeeWorkflow _employeeWorkflow;
        private readonly EntryTakenOutWorkflow _entryTakenOutWorkflow;

        public EmployeeController(IFetcher fetcher, ICommandService commandService, IEmployeeQueryService employeeQueryService, IJobTypeQueryService jobTypeQueryService, IDepartmentQueryService departmentQueryService, IProjectQueryService projectQueryService, EmployeeWorkflow employeeWorkflow, EntryTakenOutWorkflow entryTakenOutWorkflow)
        {
            _fetcher = fetcher;
            _commandService = commandService;
            _employeeQueryService = employeeQueryService;
            _jobTypeQueryService = jobTypeQueryService;
            _departmentQueryService = departmentQueryService;
            _projectQueryService = projectQueryService;
            _employeeWorkflow = employeeWorkflow;
            _entryTakenOutWorkflow = entryTakenOutWorkflow;
        }
        // GET: Product
        public ActionResult Index(int page = 1, int pageSize = Web.Common.PaginationSetttings.PageSize, EmployeeQuery query = null)
        {
            var viewModel = new IndexViewModel(Url)
            {
                Query = query,
                Items = _employeeQueryService.Query(page, pageSize, query),
                JobTypes = _jobTypeQueryService.GeTypes().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                Departments = _departmentQueryService.GetDepartments().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
            };
            return View("~/Views/SystemSetting/Employee/Index.cshtml", viewModel);
        }

        public ActionResult Entry(int id)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.人事资料, Permission.编辑))
            {
                return RedirectToAction("NoPermission", "Home");
            }
            var employeea = _employeeQueryService.Get(id);

            var viewModel = new EntryViewModel
            {
                Projects = _projectQueryService.QueryAllValidByProjectFilter().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                DepartmentName = employeea.Department.Name,
                Id = employeea.Id,
                DepartmentId = employeea.Department.Id,
                EmployeeNo = employeea.EmployeeNo,
                EmployeeName = employeea.EmployeeName,
                Sex = employeea.Sex,
                Age = employeea.Age,
                Birthday = employeea.Birthday,
                IDCardType = employeea.IDCardType,
                IDCard = employeea.IDCard,
                PhoneNumber = employeea.PhoneNumber,
                Email = employeea.Email,
                Address = employeea.Address,
                FirstWorkDate = employeea.FirstWorkDate,
                NursingDate = employeea.NursingDate,
                FirstArrivaledDate = employeea.FirstArrivaledDate,
                EstablishedDate = employeea.EstablishedDate,
                LeaveOfficeDate = employeea.LeaveOfficeDate,
                ProjectId = employeea.Project.Id,
                ProjectName = employeea.Project.Name,
                JobTypeId = employeea.JobType.Id,
                JobTypeName = employeea.JobType.Name,
                JobLevel = employeea.JobLevel,
                JobContractType = employeea.JobContractType,
                Status = employeea.Status,
                WorkHourType = employeea.WorkHourType,
                FirstQualifications = employeea.FirstQualifications,
                FirstQualificationsSchool = employeea.FirstQualificationsSchool,
                FirstQualificationsProfessional = employeea.FirstQualificationsProfessional,
                HighestQualifications = employeea.HighestQualifications,
                HighestQualificationsSchool = employeea.HighestQualificationsSchool,
                HighestQualificationsProfessional = employeea.HighestQualificationsProfessional,
                Qualification = employeea.Qualification,
                HouseholdType = employeea.HouseholdType,
                HouseholdAddress = employeea.HouseholdAddress,
                UrgentContactor = employeea.UrgentContactor,
                UrgentContactorPhoneNumber = employeea.UrgentContactorPhoneNumber,
                UrgentContactorRelation = employeea.UrgentContactorRelation,
                AttendanceNo = employeea.AttendanceNo,
                Description = employeea.Description,
                Probation = employeea.Probation,
                SignedType = employeea.SignedType,
                MaritalStatus = employeea.MaritalStatus,
                ChildrenStatus = employeea.ChildrenStatus,
                ProbationPeriodFixedCost = employeea.ProbationPeriodFixedCost,
                ProbationPeriodSlidingCost = employeea.ProbationPeriodSlidingCost,
                PromotionFixedCost = employeea.PromotionFixedCost,
                PromotionSlidingCost = employeea.PromotionSlidingCost,
                OtherWelfareType = employeea.OtherWelfareType,
                InsuranceType = employeea.InsuranceType,
                ReserveStatus = employeea.ReserveStatus,
                WorkflowCategory = employeea.WorkflowCategory,
            };
            return View("~/Views/SystemSetting/Employee/Entry.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult Entry(EntryEmployeeCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("EntryDetail", new { id = command.Id });
        }

        public ActionResult EntryDetail(int id)
        {
            var employeea = _employeeQueryService.Get(id);

            var viewModel = new EntryViewModel
            {
                Projects = _projectQueryService.QueryAllValidByProjectFilter().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                
                DepartmentName = employeea.Department.Name,
                Id = employeea.Id,
                DepartmentId = employeea.Department.Id,
                EmployeeNo = employeea.EmployeeNo,
                EmployeeName = employeea.EmployeeName,
                Sex = employeea.Sex,
                Age = employeea.Age,
                Birthday = employeea.Birthday,
                IDCardType = employeea.IDCardType,
                IDCard = employeea.IDCard,
                PhoneNumber = employeea.PhoneNumber,
                Email = employeea.Email,
                Address = employeea.Address,
                FirstWorkDate = employeea.FirstWorkDate,
                NursingDate = employeea.NursingDate,
                FirstArrivaledDate = employeea.FirstArrivaledDate,
                EstablishedDate = employeea.EstablishedDate,
                LeaveOfficeDate = employeea.LeaveOfficeDate,
                ProjectId = employeea.Project.Id,
                ProjectName = employeea.Project.Name,
                JobTypeId = employeea.JobType.Id,
                JobTypeName = employeea.JobType.Name,
                JobLevel = employeea.JobLevel,
                JobContractType = employeea.JobContractType,
                Status = employeea.Status,
                WorkHourType = employeea.WorkHourType,
                FirstQualifications = employeea.FirstQualifications,
                FirstQualificationsSchool = employeea.FirstQualificationsSchool,
                FirstQualificationsProfessional = employeea.FirstQualificationsProfessional,
                HighestQualifications = employeea.HighestQualifications,
                HighestQualificationsSchool = employeea.HighestQualificationsSchool,
                HighestQualificationsProfessional = employeea.HighestQualificationsProfessional,
                Qualification = employeea.Qualification,
                HouseholdType = employeea.HouseholdType,
                HouseholdAddress = employeea.HouseholdAddress,
                UrgentContactor = employeea.UrgentContactor,
                UrgentContactorPhoneNumber = employeea.UrgentContactorPhoneNumber,
                UrgentContactorRelation = employeea.UrgentContactorRelation,
                AttendanceNo = employeea.AttendanceNo,
                Description = employeea.Description,
                Probation = employeea.Probation,
                SignedType = employeea.SignedType,
                MaritalStatus = employeea.MaritalStatus,
                ChildrenStatus = employeea.ChildrenStatus,
                ProbationPeriodFixedCost = employeea.ProbationPeriodFixedCost,
                ProbationPeriodSlidingCost = employeea.ProbationPeriodSlidingCost,
                PromotionFixedCost = employeea.PromotionFixedCost,
                PromotionSlidingCost = employeea.PromotionSlidingCost,
                OtherWelfareType = employeea.OtherWelfareType,
                InsuranceType = employeea.InsuranceType,
                ReserveStatus = employeea.ReserveStatus,
                WorkflowCategory = employeea.WorkflowCategory,
                CurrentWorkFlowStep = _entryTakenOutWorkflow.GetCurrentWorkflowStep(employeea),
                TrackingResult = new TrackingResult.TrackingResultViewModel
                {
                    WorkflowTrackingResults = _entryTakenOutWorkflow.GetWorkflowTrackingResults(employeea),
                    WorkflowHistoryTrackingResults = _entryTakenOutWorkflow.GetWorkflowHistoryTrackingResults(employeea)
                }
            };
            if (viewModel.CurrentWorkFlowStep != null)
            {
                viewModel.CurrentWorkflowStepId = viewModel.CurrentWorkFlowStep.Id;
            }
            return View("~/Views/SystemSetting/Employee/EntryDetail.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult Create(WorkflowCategory workflowCategory)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.人事资料, Permission.新增))
            {
                return RedirectToAction("NoPermission", "Home");
            }
            var viewModel = new CreateViewModel
            {

                Projects = _projectQueryService.QueryAllValidByProjectFilter().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
            };
            viewModel.EmployeeNo = _employeeQueryService.GetEmployeeNo();
            viewModel.WorkflowCategory = workflowCategory;
            return View("~/Views/SystemSetting/Employee/Create.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult Create(CreateAndSubmitEmployeeCommand command)
        {
            var result = _commandService.ExecuteFoResult(command);
            return RedirectToAction("Detail", new { id = result.Id });
        }

        public ActionResult Detail(int id)
        {
            var employeea = _employeeQueryService.Get(id);
            var viewModel = new DetailViewModel
            {
                DepartmentName = employeea.Department.Name,
                Id = employeea.Id,
                DepartmentId = employeea.Department.Id,
                EmployeeNo = employeea.EmployeeNo,
                EmployeeName = employeea.EmployeeName,
                Sex = employeea.Sex,
                Age = employeea.Age,
                Birthday = employeea.Birthday,
                IDCardType = employeea.IDCardType,
                IDCard = employeea.IDCard,
                PhoneNumber = employeea.PhoneNumber,
                Email = employeea.Email,
                Address = employeea.Address,
                FirstWorkDate = employeea.FirstWorkDate,
                NursingDate = employeea.NursingDate,
                FirstArrivaledDate = employeea.FirstArrivaledDate,
                Probation = employeea.Probation,
                LeaveOfficeDate = employeea.LeaveOfficeDate,
                ProjectId = employeea.Project.Id,
                ProjectName = employeea.Project.Name,
                JobTypeId = employeea.JobType.Id,
                JobTypeName = employeea.JobType.Name,
                JobLevel = employeea.JobLevel,
                JobContractType = employeea.JobContractType,
                Status = employeea.Status,
                WorkHourType = employeea.WorkHourType,
                HouseholdType = employeea.HouseholdType,
                SignedType = employeea.SignedType,
                HighestQualifications = employeea.HighestQualifications,
                MaritalStatus = employeea.MaritalStatus,
                ChildrenStatus = employeea.ChildrenStatus,
                ProbationPeriodFixedCost = employeea.ProbationPeriodFixedCost,
                ProbationPeriodSlidingCost = employeea.ProbationPeriodSlidingCost,
                PromotionFixedCost = employeea.PromotionFixedCost,
                PromotionSlidingCost = employeea.PromotionSlidingCost,
                OtherWelfareType = employeea.OtherWelfareType,
                InsuranceType = employeea.InsuranceType,
                ReserveStatus = employeea.ReserveStatus,
                CurrentWorkFlowStep = _employeeWorkflow.GetCurrentWorkflowStep(employeea),
                TrackingResult = new TrackingResult.TrackingResultViewModel
                {
                    WorkflowTrackingResults = _employeeWorkflow.GetWorkflowTrackingResults(employeea),
                    WorkflowHistoryTrackingResults = _employeeWorkflow.GetWorkflowHistoryTrackingResults(employeea)
                }
            };

            if (viewModel.CurrentWorkFlowStep != null)
            {
                viewModel.CurrentWorkflowStepId = viewModel.CurrentWorkFlowStep.Id;
            }

            return View("~/Views/SystemSetting/Employee/Detail.cshtml", viewModel);
        }

        public ActionResult Edit(int id)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.人事资料, Permission.编辑))
            {
                return RedirectToAction("NoPermission", "Home");
            }
            var employeea = _employeeQueryService.Get(id);

            var viewModel = new EditViewModel
            {
                Projects = _projectQueryService.QueryAllValidByProjectFilter().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                Id = employeea.Id,
                DepartmentId = employeea.Department.Id,
                EmployeeNo = employeea.EmployeeNo,
                EmployeeName = employeea.EmployeeName,
                Sex = employeea.Sex,
                Age = employeea.Age,
                Birthday = employeea.Birthday,
                IDCardType = employeea.IDCardType,
                IDCard = employeea.IDCard,
                PhoneNumber = employeea.PhoneNumber,
                Email = employeea.Email,
                Address = employeea.Address,
                FirstWorkDate = employeea.FirstWorkDate,
                NursingDate = employeea.NursingDate,
                FirstArrivaledDate = employeea.FirstArrivaledDate,
                Probation = employeea.Probation,
                LeaveOfficeDate = employeea.LeaveOfficeDate,
                ProjectId = employeea.Project.Id,
                JobTypeId = employeea.JobType.Id,
                JobLevel = employeea.JobLevel,
                JobContractType = employeea.JobContractType,
                Status = employeea.Status,
                WorkHourType = employeea.WorkHourType,
                HouseholdType = employeea.HouseholdType,
                WorkflowCategory = employeea.WorkflowCategory,
                SignedType = employeea.SignedType,
                HighestQualifications = employeea.HighestQualifications,
                MaritalStatus = employeea.MaritalStatus,
                ChildrenStatus = employeea.ChildrenStatus,
                ProbationPeriodFixedCost = employeea.ProbationPeriodFixedCost,
                ProbationPeriodSlidingCost = employeea.ProbationPeriodSlidingCost,
                PromotionFixedCost = employeea.PromotionFixedCost,
                PromotionSlidingCost = employeea.PromotionSlidingCost,
                OtherWelfareType = employeea.OtherWelfareType,
                InsuranceType = employeea.InsuranceType,
                ReserveStatus = employeea.ReserveStatus,
            };
            viewModel.TrackingResult = new TrackingResult.TrackingResultViewModel
            {
                WorkflowHistoryTrackingResults = _employeeWorkflow.GetWorkflowHistoryTrackingResults(employeea)
            };

            return View("~/Views/SystemSetting/Employee/Edit.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult Edit(EditEmployeeCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Detail", new { id = command.Id });
        }

        [HttpGet]
        public ActionResult Adjust(int id)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.人事资料, Permission.员工调动))
            {
                return RedirectToAction("NoPermission", "Home");
            }
            var employeea = _employeeQueryService.Get(id);
            var viewModel = new AdjustViewModel()
            {
                Projects = _projectQueryService.QueryAllValidByProjectFilter().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                Employee = employeea,
                EmployeeId = employeea.Id,
                OriginalJobTypeId = employeea.JobType.Id,
                OriginalProjectId = employeea.Project.Id,
                OriginalDepartmentId = employeea.Department.Id,
            
            };
            return View("~/Views/SystemSetting/Employee/Adjust.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult Adjust(AdjustEmployeeCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Detail", new { id = command.EmployeeId });
        }

        public ActionResult Delete(DeleteEmployeeCommand command)
        {
            _commandService.Execute(command);
            return command.ReturnUrl.IsNullOrWhiteSpace() ? (ActionResult)RedirectToAction("Index") : Redirect(command.ReturnUrl);
        }

        [HttpPost]
        public ActionResult Approval(ApprovalEmployeeCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Detail", new { id = command.EmployeeId });
        }

        [HttpPost]
        public ActionResult EntryApproval(EntryApprovalEmployeeCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("EntryDetail", new { id = command.EmployeeId });
        }

        public ActionResult Print(PrintEmployeeCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Detail", new { id = command.EmployeeId });
        }

        public ActionResult GetSex(string idCard)
        {
            return Json(new { success = true, Sex = idCard.ToSex(), Birthday = idCard.ToBirthday().Value.ToString("yyyy-MM-dd"), Age = idCard.ToAge() }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDepartmentsByProjectId(int projectId)
        {
           var departments = _departmentQueryService.GetDepartments(projectId).Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            return Json(departments, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetJobOccupationByProjectId(int projectId)
        {
            var jobOccupations =_fetcher.Query<HRProjectJobOccupation>().Where(x=>x.Project.Id==projectId).Select(x => new SelectListItem
            {
                Text = x.HRJobType.Name,
                Value = x.HRJobType.Id.ToString()
            });
            return Json(jobOccupations, JsonRequestBehavior.AllowGet);
        }
    }
}