using System.Linq;
using System.Web.Mvc;
using PensionInsurance.Query;
using PensionInsurance.Web.Views.Account;

namespace PensionInsurance.Web.Views.Reports.CustomerLeaveReport
{
    public class CustomerLeaveReportController : AuthorizedController
    {
        private readonly IProjectQueryService _projectQueryService;
        private readonly ICustomerLeaveReportQuerySerivce _customerLeaveReportQueryService;

        public CustomerLeaveReportController(IProjectQueryService projectQueryService, ICustomerLeaveReportQuerySerivce customerLeaveReportQueryService)
        {
            _projectQueryService = projectQueryService;
            _customerLeaveReportQueryService = customerLeaveReportQueryService;
        }

        public ActionResult Index(int page = 1, int pageSize = Web.Common.PaginationSetttings.PageSize, CustomerLeaveReportQuery query = null)
        {
            IndexViewModel viewModel = new IndexViewModel
            {
                CustomerLeaves = _customerLeaveReportQueryService.QueryCustomerLeaveReport(page, pageSize,query),
                Query = query,
                ProjectList = _projectQueryService.QueryAllValidByProjectFilter().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            return View("~/Views/Reports/CustomerLeaveReport/Index.cshtml", viewModel);
        }
    }
}