using System.Linq;
using System.Web.Mvc;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Query;
using PensionInsurance.Query.Implemention;
using PensionInsurance.Web.Views.Account;

namespace PensionInsurance.Web.Views.Reports.CustomerCheckInRoomAndMealsComparisonReport
{
    public class CustomerCheckInRoomAndMealsComparisonReportController : AuthorizedController
    {
        private readonly ICustomerCheckInRoomAndMealsComparisonReportQueryService _customerCheckInRoomAndMealsComparisonReportQueryService;
        private readonly IProjectQueryService _projectQueryService;
        private readonly ICommandService _commandService;
        public CustomerCheckInRoomAndMealsComparisonReportController(ICommandService commandService, ICustomerCheckInRoomAndMealsComparisonReportQueryService customerCheckInRoomAndMealsComparisonReportQueryService, IProjectQueryService projectQueryService)
        {
            _commandService = commandService;
            _customerCheckInRoomAndMealsComparisonReportQueryService = customerCheckInRoomAndMealsComparisonReportQueryService;
            _projectQueryService = projectQueryService;
        }

        public ActionResult Index(CustomerCheckInRoomAndMealsComparisonReportQuery query = null)
        {
            IndexViewModel viewModel = new IndexViewModel
            {
                Query = query,
                CustomerCheckInRoomAndMealsComparisonReport = _customerCheckInRoomAndMealsComparisonReportQueryService.Query(query),
                ProjectList = _projectQueryService.QueryAllValidByProjectFilter().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };

            return View("~/Views/Reports/CustomerCheckInRoomAndMealsComparisonReport/Index.cshtml", viewModel);
        }

        public ActionResult Export(ExportCustomerCheckInRoomAndMealsComparisonReportCommand command)
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