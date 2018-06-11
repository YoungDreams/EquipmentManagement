using System.Linq;
using System.Web.Mvc;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Query;
using PensionInsurance.Web.Views.Account;

namespace PensionInsurance.Web.Views.Reports.CustomerValidCheckInDaysReport
{
    public class CustomerValidCheckInDaysReportController : AuthorizedController
    {
        private readonly ICustomerValidCheckInDaysReportQueryService _customerValidCheckInDaysReportQueryService;
        private readonly IProjectQueryService _projectQueryService;
        private readonly ICommandService _commandService;
        public CustomerValidCheckInDaysReportController(ICommandService commandService, ICustomerValidCheckInDaysReportQueryService customerValidCheckInDaysReportQueryService, IProjectQueryService projectQueryService)
        {
            _commandService = commandService;
            _customerValidCheckInDaysReportQueryService = customerValidCheckInDaysReportQueryService;
            _projectQueryService = projectQueryService;
        }

        public ActionResult Index(CustomerValidCheckInDaysReportQuery query = null)
        {
            IndexViewModel viewModel = new IndexViewModel
            {
                Query = query,
                CustomerValidCheckInDays = _customerValidCheckInDaysReportQueryService.Query(query),
                ProjectList = _projectQueryService.QueryAllValidByProjectFilter().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };

            return View("~/Views/Reports/CustomerValidCheckInDaysReport/Index.cshtml", viewModel);
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