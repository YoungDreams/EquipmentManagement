using System;
using System.Linq;
using System.Web.Mvc;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Query;
using PensionInsurance.Web.Views.Account;

namespace PensionInsurance.Web.Views.Reports.ExpectMoveInDailyReport
{
    public class ExpectMoveInDailyReportController : AuthorizedController
    {
        private readonly IReportQueryService _reportQueryService;
        private readonly IProjectQueryService _projectQueryService;
        private readonly ICommandService _commandService;
        public ExpectMoveInDailyReportController(IProjectQueryService projectQueryService, IReportQueryService reportQueryService, ICommandService commandService)
        {
            _projectQueryService = projectQueryService;
            _reportQueryService = reportQueryService;
            _commandService = commandService;
        }

        public ActionResult Index(SalesDailyReportQuery query)
        {
            var now = DateTime.Now;
            query.StartDate = query.StartDate ?? now.Date;
            query.EndDate = query.EndDate ?? query.StartDate;
            if (query.EndDate.Value.Date >= DateTime.Now.Date)
            {
                query.EndDate = DateTime.Now.Date;
            }

            var viewModel = new IndexViewModel
            {
                Projects = _projectQueryService.QueryAllValidByProjectFilter().ToList(),
                ReportData = _reportQueryService.GetDailyReports(query),
                Query = query
            };

            return View("~/Views/Reports/ExpectMoveInDailyReport/Index.cshtml", viewModel);
        }
        
        public ActionResult Export(ExportDailyReportCommand command)
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