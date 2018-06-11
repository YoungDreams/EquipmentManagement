using System.Linq;
using System.Web.Mvc;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Query;
using PensionInsurance.Web.Views.Account;

namespace PensionInsurance.Web.Views.Reports.CustomerMonthlyFeeDailyReport
{
    public class CustomerMonthlyFeeDailyReportController : AuthorizedController
    {
        private readonly ICustomerMonthlyFeeDailyReportQueryService _CustomerMonthlyFeeDailyReportQueryService;
        private readonly IProjectQueryService _projectQueryService;
        private readonly ICommandService _commandService;
        public CustomerMonthlyFeeDailyReportController(ICommandService commandService, ICustomerMonthlyFeeDailyReportQueryService customerMonthlyFeeDailyReportQueryService, IProjectQueryService projectQueryService)
        {
            _commandService = commandService;
            _CustomerMonthlyFeeDailyReportQueryService = customerMonthlyFeeDailyReportQueryService;
            _projectQueryService = projectQueryService;
        }

        public ActionResult Index(CustomerMonthlyFeeDailyReportQuery query = null)
        {
            IndexViewModel viewModel = new IndexViewModel
            {
                Query = query,
                CustomerMonthlyFeeDailyReports = _CustomerMonthlyFeeDailyReportQueryService.Query(query),
                ProjectList = _projectQueryService.QueryAllValidByProjectFilter().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            if (query != null && (query.StartTime.HasValue && query.EndTime.HasValue))
            {
                viewModel.QueryTimeDays = query.EndTime.Value.Date.Subtract(query.StartTime.Value.Date).Days+1;
            }

            return View("~/Views/Reports/CustomerMonthlyFeeDailyReport/Index.cshtml", viewModel);
        }
    }
}