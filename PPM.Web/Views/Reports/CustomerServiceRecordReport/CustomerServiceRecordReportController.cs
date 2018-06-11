using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Foundation.Data;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Query;
using PensionInsurance.Web.Views.Account;

namespace PensionInsurance.Web.Views.Reports.CustomerServiceRecordReport
{
    public class CustomerServiceRecordReportController : AuthorizedController
    {
        private readonly IProjectQueryService _projectQueryService;
        private readonly ICommandService _commandService;
        private readonly ICustomerServiceRecordReportQueryService _customerServiceRecordReportQueryService;
        private readonly IFetcher _fetcher;
        public CustomerServiceRecordReportController(IProjectQueryService projectQueryService, IFetcher fetcher, ICustomerServiceRecordReportQueryService customerServiceRecordReportQueryService, ICommandService commandService)
        {
            _projectQueryService = projectQueryService;
            _fetcher = fetcher;
            _customerServiceRecordReportQueryService = customerServiceRecordReportQueryService;
            _commandService = commandService;
        }

        public ActionResult Index(CustomerServiceRecordReportQuery  query = null)
        {
            IndexViewModel viewModel = new IndexViewModel
            {
                CustomerServiceRecords = _customerServiceRecordReportQueryService.QueryCustomerServiceRecordReport(query),
                Query = query,
                ProjectList = _projectQueryService.QueryAllValidByProjectFilter().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            return View("~/Views/Reports/CustomerServiceRecordReport/Index.cshtml", viewModel);
        }

        public ActionResult Export(ExportCustomerServiceRecordReportCommand command)
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