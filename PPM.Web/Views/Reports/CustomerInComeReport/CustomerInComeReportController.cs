using System.Linq;
using System.Web.Mvc;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Query;
using PensionInsurance.Web.Views.Account;

namespace PensionInsurance.Web.Views.Reports.CustomerInComeReport
{
    public class CustomerInComeReportController : AuthorizedController
    {
        private readonly ICustomerInComeReportQueryService _customerInComeReportQueryService;
        private readonly IProjectQueryService _projectQueryService;
        private readonly ICommandService _commandService;
        public CustomerInComeReportController(ICommandService commandService, ICustomerInComeReportQueryService customerInComeReportQueryService, IProjectQueryService projectQueryService)
        {
            _commandService = commandService;
            _customerInComeReportQueryService = customerInComeReportQueryService;
            _projectQueryService = projectQueryService;
        }

        public ActionResult Index(CustomerInComeReportQuery query = null)
        {
            IndexViewModel viewModel = new IndexViewModel
            {
                Query = query,
                CustomerInComeReports = _customerInComeReportQueryService.Query(query),
                ProjectList = _projectQueryService.QueryAllValidByProjectFilter().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            if (query != null && (query.StartTime.HasValue && query.EndTime.HasValue))
            {
                viewModel.QueryTimeMonths = (query.EndTime.Value.Year - query.StartTime.Value.Year)*12 +
                                            (query.EndTime.Value.Month - query.StartTime.Value.Month)+1;
            }

            return View("~/Views/Reports/CustomerInComeReport/Index.cshtml", viewModel);
        }

        //public ActionResult Export(ExportContractLedgerReportCommand command)
        //{
        //    var result = _commandService.ExecuteFoResult(command);

        //    return Json(new
        //    {
        //        success = result.IsSucceed,
        //        redirect = $"{Url.Content("~/Attachments/Report/")}{result.FileName}"
        //    }, JsonRequestBehavior.AllowGet);
        //}
    }
}