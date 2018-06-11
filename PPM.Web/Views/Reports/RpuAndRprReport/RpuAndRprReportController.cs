using System.Linq;
using System.Web.Mvc;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Query;
using PensionInsurance.Web.Views.Account;

namespace PensionInsurance.Web.Views.Reports.RpuAndRprReport
{
    public class RpuAndRprReportController : AuthorizedController
    {
        private readonly IRpuAndRprReportQueryService _rpuAndRprReportQueryService;
        private readonly IProjectQueryService _projectQueryService;
        private readonly ICommandService _commandService;
        public RpuAndRprReportController(ICommandService commandService, IProjectQueryService projectQueryService, IRpuAndRprReportQueryService rpuAndRprReportQueryService)
        {
            _commandService = commandService;
            _projectQueryService = projectQueryService;
            _rpuAndRprReportQueryService = rpuAndRprReportQueryService;
        }

        public ActionResult Index(RpuAndRprReportQuery query = null)
        {
            IndexViewModel viewModel = new IndexViewModel
            {
                Query = query,
                RpuAndRprReports = _rpuAndRprReportQueryService.Query(query),
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

            return View("~/Views/Reports/RpuAndRprReport/Index.cshtml", viewModel);
        }

        public ActionResult Export(ExportRpuAndRprReportCommand command)
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