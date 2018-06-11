using System;
using System.Linq;
using System.Web.Mvc;
using Foundation.Data;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Query;
using PensionInsurance.Shared;
using PensionInsurance.Web.Views.Account;

namespace PensionInsurance.Web.Views.Reports.CustomerChangeDailyReport
{
    public class CustomerChangeDailyReportController : AuthorizedController
    {
        private readonly ICustomerChangeDailyReportQueryService _customerChangeDailyReportQueryService;
        private readonly IProjectQueryService _projectQueryService;
        private readonly ICommandService _commandService;
        public CustomerChangeDailyReportController(IProjectQueryService projectQueryService, ICustomerChangeDailyReportQueryService customerChangeDailyReportQueryService, ICommandService commandService)
        {
            _projectQueryService = projectQueryService;
            _customerChangeDailyReportQueryService = customerChangeDailyReportQueryService;
            _commandService = commandService;
        }

        public ActionResult Index(CustomerChangeDailyReportQuery query)
        {
            if (query.EndDate.HasValue && query.EndDate.Value.Date >= DateTime.Now.Date)
            {
                query.EndDate = DateTime.Now.Date;
            }

            var viewModel = new IndexViewModel
            {
                Projects = _projectQueryService.QueryAllValidByProjectFilter().ToList(),
                ReportData = _customerChangeDailyReportQueryService.Query(query).ToList(),
                Query = query,
                ProjectSelectListItems = _projectQueryService.QueryAllValidByProjectFilter().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };

            return View("~/Views/Reports/CustomerChangeDailyReport/Index.cshtml", viewModel);
        }

        public ActionResult Export(ExportCustomerChangeDailyReportCommand command)
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