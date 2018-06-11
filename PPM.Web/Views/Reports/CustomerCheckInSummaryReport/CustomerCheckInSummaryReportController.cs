using System.Linq;
using System.Web.Mvc;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Query;
using PensionInsurance.Web.Views.Account;

namespace PensionInsurance.Web.Views.Reports.CustomerCheckInSummaryReport
{
    public class CustomerCheckInSummaryReportController : AuthorizedController
    {
        private readonly ICustomerCheckInSummaryReportQueryService _checkInSummaryReportQueryService;
        private readonly IProjectQueryService _projectQueryService;
        private readonly ICommandService _commandService;
        public CustomerCheckInSummaryReportController(ICommandService commandService, IProjectQueryService projectQueryService, ICustomerCheckInSummaryReportQueryService checkInSummaryReportQueryService)
        {
            _commandService = commandService;
            _projectQueryService = projectQueryService;
            _checkInSummaryReportQueryService = checkInSummaryReportQueryService;
        }

        public ActionResult Index(CustomerCheckInSummaryReportQuery query = null)
        {
            if (query == null)
            {
                query = new CustomerCheckInSummaryReportQuery();
            }
            query.ProjectType = ProjectType.CB;
            IndexViewModel viewModel = new IndexViewModel
            {
                Query = query,
                CustomerCheckInSummaryReport = _checkInSummaryReportQueryService.Query(query),
                ProjectList = _projectQueryService.QueryAllValidByProjectFilter()
                    .Where(x => x.ProjectType == ProjectType.CB)
                    .Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }),
                Projects = _projectQueryService.QueryAllValidByProjectFilter()
                    .Where(x => x.ProjectType == ProjectType.CB)
            };

            if (query != null && query.ProjectIds != null && query.ProjectIds.Any())
            {
                viewModel.Projects = viewModel.Projects.Where(x => query.ProjectIds.Contains(x.Id));
            }

            return View("~/Views/Reports/CustomerCheckInSummaryReport/Index.cshtml", viewModel);
        }

        public ActionResult Export(ExportCustomerValidCheckInDaysReportCommand command)
        {
            var result = _commandService.ExecuteFoResult(command);

            return Json(new
            {
                success = result.IsSucceed,
                redirect = $"{Url.Content("~/Attachments/Report/")}{result.FileName}"
            }, JsonRequestBehavior.AllowGet);
        }
    }
}