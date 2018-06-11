using System.Linq;
using System.Web.Mvc;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Query;
using PensionInsurance.Web.Views.Account;

namespace PensionInsurance.Web.Views.Reports.RoomCostReport
{
    public class RoomCostReportController: AuthorizedController
    {
        private readonly IRoomCostReportQueryService _roomCostReportQueryService;
        private readonly IProjectQueryService _projectQueryService;
        private readonly ICommandService _commandService;
        public RoomCostReportController(ICommandService commandService, IProjectQueryService projectQueryService, IRoomCostReportQueryService roomCostReportQueryService)
        {
            _commandService = commandService;
            _projectQueryService = projectQueryService;
            _roomCostReportQueryService = roomCostReportQueryService;
        }

        public ActionResult Index(RoomCostReportQuery query = null)
        {
            IndexViewModel viewModel = new IndexViewModel
            {
                RoomCostReport = _roomCostReportQueryService.Query(query),
                Query = query,
                ProjectList = _projectQueryService.QueryAllValid().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };

            return View("~/Views/Reports/RoomCostReport/Index.cshtml", viewModel);
        }

        public ActionResult Export(ExportRoomCostReportCommand command)
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