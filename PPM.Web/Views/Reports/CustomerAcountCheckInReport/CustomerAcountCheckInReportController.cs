using System.Linq;
using System.Web.Mvc;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Query;
using PensionInsurance.Web.Views.Account;

namespace PensionInsurance.Web.Views.Reports.CustomerAcountCheckInReport
{
    public class CustomerAcountCheckInReportController : AuthorizedController
    {
        private readonly ICustomerAcountCheckInReportQueryService _customerAcountCheckInReportQueryService;
        private readonly IProjectQueryService _projectQueryService;
        private readonly ICommandService _commandService;
        public CustomerAcountCheckInReportController(ICommandService commandService, ICustomerAcountCheckInReportQueryService customerAcountCheckInReportQueryService, IProjectQueryService projectQueryService)
        {
            _commandService = commandService;
            _customerAcountCheckInReportQueryService = customerAcountCheckInReportQueryService;
            _projectQueryService = projectQueryService;
        }

        public ActionResult Index(CustomerAcountCheckInReportQuery query = null)
        {
            IndexViewModel viewModel = new IndexViewModel
            {
                Query = query,
                Report = _customerAcountCheckInReportQueryService.QueryCustomerAcountCheckInReport(query),
                ProjectList = _projectQueryService.QueryAllValidByProjectFilter().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };

            return View("~/Views/Reports/CustomerAcountCheckInReport/Index.cshtml", viewModel);
        }

        public ActionResult Export(ExportCustomerAcountCheckInReportCommand command)
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