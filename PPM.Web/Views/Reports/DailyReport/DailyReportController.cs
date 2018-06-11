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

namespace PensionInsurance.Web.Views.Reports.DailyReport
{
    public class DailyReportController : AuthorizedController
    {
        private readonly IReportQueryService _reportQueryService;
        private readonly IProjectQueryService _projectQueryService;
        private readonly IFetcher _fetcher;
        private readonly ICommandService _commandService;
        public DailyReportController(IProjectQueryService projectQueryService, IReportQueryService reportQueryService, IFetcher fetcher, ICommandService commandService)
        {
            _projectQueryService = projectQueryService;
            _reportQueryService = reportQueryService;
            _fetcher = fetcher;
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

            var viewModel = new IndexViewModel(_fetcher)
            {
                Projects = _projectQueryService.QueryAllValidByProjectFilter().ToList(),
                ReportData = _reportQueryService.GetDailyReports(query),
                Query = query
            };

            return View("~/Views/Reports/DailyReport/Index.cshtml", viewModel);
        }

        public ActionResult GetProjectChangeByTime(int projectId, DateTime time)
        {
            var customerChangeDailyReport =
                _fetcher
                    .Query<Entities.CustomerChangeDailyReport>()
                    .FirstOrDefault(x => x.Project.Id == projectId && x.ReportDate.Date == time.Date);

            if (customerChangeDailyReport == null)
            {
                return Json(new { Result = false, }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                Result = true,
                MovedIn = customerChangeDailyReport.DailyMovedInPersonDesc.Replace("|", "<br/>"),
                MovedOut = customerChangeDailyReport.DailyMovedOutPersonDesc.Replace("|", "<br/>"),
                Leave = customerChangeDailyReport.LeaveDesc.Replace("|", "<br/>"),
                RoomChange = customerChangeDailyReport.DailyRoomChangeDesc.Replace("|", "<br/>"),
                ServiceChange = customerChangeDailyReport.DailyServiceChangeDesc.Replace("|", "<br/>")
            }, JsonRequestBehavior.AllowGet);
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