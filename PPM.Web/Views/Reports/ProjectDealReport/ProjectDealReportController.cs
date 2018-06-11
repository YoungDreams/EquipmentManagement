using System.Linq;
using System.Web.Mvc;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Query;
using PensionInsurance.Web.Views.Account;

namespace PensionInsurance.Web.Views.Reports.ProjectDealReport
{
    public class ProjectDealReportController : AuthorizedController
    {
        private readonly IProjectDealReportQueryService _projectDealReportQueryService;
        private readonly IProjectQueryService _projectQueryService;
        private readonly ICommandService _commandService;
        public ProjectDealReportController(ICommandService commandService, IProjectDealReportQueryService projectDealReportQueryService, IProjectQueryService projectQueryService)
        {
            _commandService = commandService;
            _projectDealReportQueryService = projectDealReportQueryService;
            _projectQueryService = projectQueryService;
        }

        public ActionResult Index(ProjectDealReportQuery query = null)
        {
            IndexViewModel viewModel = new IndexViewModel
            {
                Query = query,
                ProjectDealReports = _projectDealReportQueryService.Query(query),
                ProjectList = _projectQueryService.QueryAllValidByProjectFilter().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            if (query != null && (query.StartTime.HasValue && query.EndTime.HasValue))
            {
                viewModel.QueryTimeMonths = (query.EndTime.Value.Year - query.StartTime.Value.Year) * 12 +
                                            (query.EndTime.Value.Month - query.StartTime.Value.Month) + 1;
            }

            return View("~/Views/Reports/ProjectDealReport/Index.cshtml", viewModel);
        }

        public ActionResult Export(ExportProjectDealReportCommand command)
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