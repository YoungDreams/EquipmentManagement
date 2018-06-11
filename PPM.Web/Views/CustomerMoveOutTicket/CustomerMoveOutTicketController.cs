using System;
using System.Linq;
using System.Web.Mvc;
using Foundation.Core;
using Foundation.Data;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Query;
using PensionInsurance.Web.Views.Account;
using PensionInsurance.Web.Views.SystemSetting.Employee;
using PensionInsurance.Workflows;

namespace PensionInsurance.Web.Views.CustomerMoveOutTicket
{
    public class CustomerMoveOutTicketController : AuthorizedController
    {
        private readonly ICommandService _commandService;
        private readonly IProjectQueryService _projectQueryService;
        private readonly ICustomerMoveOutTicketQueryService _customerMoveOutTicketQueryService;
        private readonly CustomerMoveOutTicketWorkflow _customerMoveOutTicketWorkflow;
        private readonly IFetcher _fetcher;

        public CustomerMoveOutTicketController(ICommandService commandService, IFetcher fetcher, IProjectQueryService projectQueryService, ICustomerMoveOutTicketQueryService customerMoveOutTicketQueryService, CustomerMoveOutTicketWorkflow customerMoveOutTicketWorkflow)
        {
            _commandService = commandService;
            _fetcher = fetcher;
            _projectQueryService = projectQueryService;
            _customerMoveOutTicketQueryService = customerMoveOutTicketQueryService;
            _customerMoveOutTicketWorkflow = customerMoveOutTicketWorkflow;
        }
        public ActionResult Index(int page = 1, int pageSize = Web.Common.PaginationSetttings.PageSize, CustomerMoveOutTicketQuery query = null)
        {
            var viewModel = new IndexViewModel(Url)
            {
                Query = query,
                Items = _customerMoveOutTicketQueryService.Query(page, pageSize, query),
                ProjectList = _projectQueryService.QueryAllValidByProjectFilter().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            return View("~/Views/CustomerMoveOutTicket/Index.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult Create(int id)
        {
            var customerMoveOutTicket = _fetcher.Get<Entities.CustomerMoveOutTicket>(id);
            
            var viewModel = new CreateViewModel()
            {
                CustomerName = customerMoveOutTicket.Contract.Customer.Name,
                Age = customerMoveOutTicket.Contract.Customer.Birthday.ToAge(),
                ProjectName = customerMoveOutTicket.Contract.Project.Name,
                Sex = customerMoveOutTicket.Contract.Customer.Sex,
                CheckInTime = customerMoveOutTicket.CheckInTime,
                MoveOuTime = customerMoveOutTicket.MoveOuTime,
                ContractTerm = customerMoveOutTicket.ContractTerm,
                MonthCost = customerMoveOutTicket.MonthCost,
                Nursinglevel = customerMoveOutTicket.Nursinglevel,
                LivingWithChildren=customerMoveOutTicket.Contract.Customer.Consulting.LivingWithChildren,
                SpouseInfo= customerMoveOutTicket.Contract.Customer.Consulting.SpouseInfo,
                CheckInNursinglevel=customerMoveOutTicket.CheckInNursinglevel,
                ContactPhone = customerMoveOutTicket.ContactPhone,
                Contact = customerMoveOutTicket.Contact,
            };
            viewModel.CurrentWorkFlowStep = _customerMoveOutTicketWorkflow.GetCurrentWorkflowStep(customerMoveOutTicket);//当前审批步骤
            if (viewModel.CurrentWorkFlowStep != null)
            {
                viewModel.CurrentWorkflowStepId = viewModel.CurrentWorkFlowStep.Id;
            }

            viewModel.TrackingResult = new TrackingResult.TrackingResultViewModel
            {
                WorkflowTrackingResults = _customerMoveOutTicketWorkflow.GetWorkflowTrackingResults(customerMoveOutTicket),
                WorkflowHistoryTrackingResults = _customerMoveOutTicketWorkflow.GetWorkflowHistoryTrackingResults(customerMoveOutTicket)
            };
            return View("~/Views/CustomerMoveOutTicket/Create.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult Create(CreateCustomerMoveOutTicketCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Show", new { id = command.Id });
        }

        public ActionResult Opinion(int id)
        {
            var customerMoveOutTicket = _fetcher.Get<Entities.CustomerMoveOutTicket>(id);
            
            var viewModel = new EditViewModel()
            {
                CustomerName = customerMoveOutTicket.Contract.Customer.Name,
                Age = customerMoveOutTicket.Contract.Customer.Birthday.ToAge(),
                ProjectName = customerMoveOutTicket.Contract.Project.Name,
                Sex = customerMoveOutTicket.Contract.Customer.Sex,
                Id = customerMoveOutTicket.Id,
                ContractId = customerMoveOutTicket.Contract.Id,
                CheckInTime = customerMoveOutTicket.CheckInTime,
                MoveOuTime = customerMoveOutTicket.MoveOuTime,
                ContractTerm = customerMoveOutTicket.ContractTerm,
                MonthCost = customerMoveOutTicket.MonthCost,
                SpouseInfo = customerMoveOutTicket.SpouseInfo,
                Nursinglevel = customerMoveOutTicket.Nursinglevel,
                LivingWithChildren = customerMoveOutTicket.LivingWithChildren,

                CheckInNursinglevel = customerMoveOutTicket.CheckInNursinglevel,
                EldersCheckInIntention = customerMoveOutTicket.EldersCheckInIntention,
                FamiliesCheckInIntention = customerMoveOutTicket.FamiliesCheckInIntention,
                CustomerMoveOutStatus = customerMoveOutTicket.CustomerMoveOutStatus,
                CustomerMoveOutWhereAbouts = customerMoveOutTicket.CustomerMoveOutWhereAbouts,
                HasOtherAgencie = customerMoveOutTicket.HasOtherAgencie,
                Summarizes = customerMoveOutTicket.Summarizes,
                Opinion = customerMoveOutTicket.Opinion,
                ContactPhone = customerMoveOutTicket.ContactPhone,
                Contact = customerMoveOutTicket.Contact,
            };
           
            
            viewModel.SourceFunds = customerMoveOutTicket.SourceFunds.SplitToList<int>(',').Select(x => (SourceFunds)x).ToList();
            viewModel.MoveOutCauses = customerMoveOutTicket.MoveOutCauses.SplitToList<int>(',').Select(x => (MoveOutCause)x).ToList();

            viewModel.CurrentWorkFlowStep = _customerMoveOutTicketWorkflow.GetCurrentWorkflowStep(customerMoveOutTicket);//当前审批步骤
            if (viewModel.CurrentWorkFlowStep != null)
            {
                viewModel.CurrentWorkflowStepId = viewModel.CurrentWorkFlowStep.Id;
            }

            viewModel.TrackingResult = new TrackingResult.TrackingResultViewModel
            {
                WorkflowTrackingResults = _customerMoveOutTicketWorkflow.GetWorkflowTrackingResults(customerMoveOutTicket),
                WorkflowHistoryTrackingResults = _customerMoveOutTicketWorkflow.GetWorkflowHistoryTrackingResults(customerMoveOutTicket)
            };
            return View("~/Views/CustomerMoveOutTicket/Opinion.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult Opinion(OpinionCustomerMoveOutTicketCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Show", new { id = command.Id });
        }

        [HttpGet]
        public ActionResult Show(int id)
        {
            var customerMoveOutTicket = _fetcher.Get<Entities.CustomerMoveOutTicket>(id);

            var viewModel = new DetailViewModel()
            {
                CustomerName = customerMoveOutTicket.Contract.Customer.Name,
                Age = customerMoveOutTicket.Contract.Customer.Birthday.ToAge(),
                ProjectName = customerMoveOutTicket.Contract.Project.Name,
                Sex = customerMoveOutTicket.Contract.Customer.Sex,
                Id = customerMoveOutTicket.Id,
                ContractId = customerMoveOutTicket.Contract.Id,
                CheckInTime = customerMoveOutTicket.CheckInTime,
                MoveOuTime = customerMoveOutTicket.MoveOuTime,
                ContractTerm = customerMoveOutTicket.ContractTerm,
                MonthCost = customerMoveOutTicket.MonthCost,
                SpouseInfo = customerMoveOutTicket.SpouseInfo,
                Nursinglevel = customerMoveOutTicket.Nursinglevel,
                LivingWithChildren = customerMoveOutTicket.LivingWithChildren,
                HasOtherAgencie = customerMoveOutTicket.HasOtherAgencie,
                CheckInNursinglevel = customerMoveOutTicket.CheckInNursinglevel,
                EldersCheckInIntention = customerMoveOutTicket.EldersCheckInIntention,
                FamiliesCheckInIntention = customerMoveOutTicket.FamiliesCheckInIntention,
                CustomerMoveOutStatus = customerMoveOutTicket.CustomerMoveOutStatus,
                CustomerMoveOutWhereAbouts = customerMoveOutTicket.CustomerMoveOutWhereAbouts,

                Summarizes = customerMoveOutTicket.Summarizes,
                Opinion = customerMoveOutTicket.Opinion,
                ContactPhone = customerMoveOutTicket.ContactPhone,
                Contact = customerMoveOutTicket.Contact,
            };
            viewModel.SourceFunds = customerMoveOutTicket.SourceFunds.SplitToList<int>(',').Select(x => (SourceFunds)x).ToList();
            viewModel.MoveOutCauses = customerMoveOutTicket.MoveOutCauses.SplitToList<int>(',').Select(x => (MoveOutCause)x).ToList();

            viewModel.TrackingResult = new TrackingResult.TrackingResultViewModel
            {
                WorkflowTrackingResults = _customerMoveOutTicketWorkflow.GetWorkflowTrackingResults(customerMoveOutTicket),
                WorkflowHistoryTrackingResults = _customerMoveOutTicketWorkflow.GetWorkflowHistoryTrackingResults(customerMoveOutTicket)
            };
            return View("~/Views/CustomerMoveOutTicket/Show.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult Detail(int id, int page, int pageSize, CustomerMoveOutTicketQuery query = null)
        {
            var customerMoveOutTicket = _fetcher.Get<Entities.CustomerMoveOutTicket>(id);

            var viewModel = new DetailViewModel()
            {
                CustomerName = customerMoveOutTicket.Contract.Customer.Name,
                Age = customerMoveOutTicket.Contract.Customer.Birthday.ToAge(),
                ProjectName = customerMoveOutTicket.Contract.Project.Name,
                Sex = customerMoveOutTicket.Contract.Customer.Sex,
                Id = customerMoveOutTicket.Id,
                ContractId = customerMoveOutTicket.Contract.Id,
                CheckInTime = customerMoveOutTicket.CheckInTime,
                MoveOuTime = customerMoveOutTicket.MoveOuTime,
                ContractTerm = customerMoveOutTicket.ContractTerm,
                MonthCost = customerMoveOutTicket.MonthCost,
                SpouseInfo = customerMoveOutTicket.SpouseInfo,
                Nursinglevel = customerMoveOutTicket.Nursinglevel,
                LivingWithChildren = customerMoveOutTicket.LivingWithChildren,
                HasOtherAgencie = customerMoveOutTicket.HasOtherAgencie,
                CheckInNursinglevel = customerMoveOutTicket.CheckInNursinglevel,
                EldersCheckInIntention = customerMoveOutTicket.EldersCheckInIntention,
                FamiliesCheckInIntention = customerMoveOutTicket.FamiliesCheckInIntention,
                CustomerMoveOutStatus = customerMoveOutTicket.CustomerMoveOutStatus,
                CustomerMoveOutWhereAbouts = customerMoveOutTicket.CustomerMoveOutWhereAbouts,

                Summarizes = customerMoveOutTicket.Summarizes,
                Opinion = customerMoveOutTicket.Opinion,
                ContactPhone = customerMoveOutTicket.ContactPhone,
                Contact = customerMoveOutTicket.Contact,
            };
            viewModel.SourceFunds = customerMoveOutTicket.SourceFunds.SplitToList<int>(',').Select(x => (SourceFunds)x).ToList();
            viewModel.MoveOutCauses = customerMoveOutTicket.MoveOutCauses.SplitToList<int>(',').Select(x => (MoveOutCause)x).ToList();

            var customerMoveOutTickets = _customerMoveOutTicketQueryService.Query(page, pageSize, query);
            if (customerMoveOutTickets.Data.Any())
            {
                viewModel.NextId = customerMoveOutTickets.Data.First().CustomerMoveOutTicketId;
                viewModel.Page = page;
            }

            viewModel.TrackingResult = new TrackingResult.TrackingResultViewModel
            {
                WorkflowTrackingResults = _customerMoveOutTicketWorkflow.GetWorkflowTrackingResults(customerMoveOutTicket),
                WorkflowHistoryTrackingResults = _customerMoveOutTicketWorkflow.GetWorkflowHistoryTrackingResults(customerMoveOutTicket)
            };
            return View("~/Views/CustomerMoveOutTicket/Detail.cshtml", viewModel);
        }

        public Entities.Contract QueryReNewContract(Entities.Contract contract)
        {
            var reNewContract = _fetcher.Query<Entities.Contract>()
                .FirstOrDefault(
                    x =>
                        x.CustomerAccount.Id == contract.CustomerAccount.Id &&
                        x.Status == ContractStatus.失效 &&
                        x.ActualEndTime == contract.StartTime.Value.AddSeconds(-1));
            if (reNewContract != null)
            {
                return QueryReNewContract(reNewContract);
            }
            return contract;
        }
    }
}