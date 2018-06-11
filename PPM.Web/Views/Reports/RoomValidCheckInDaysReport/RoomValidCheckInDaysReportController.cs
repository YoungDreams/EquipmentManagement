using System.Linq;
using System.Web.Mvc;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Query;
using PensionInsurance.Web.Views.Account;

namespace PensionInsurance.Web.Views.Reports.RoomValidCheckInDaysReport
{
    public class RoomValidCheckInDaysReportController : AuthorizedController
    {
        private readonly IRoomValidCheckInDaysReportQueryService _roomValidCheckInDaysReportQueryService;
        private readonly IProjectQueryService _projectQueryService;
        private readonly ICommandService _commandService;
        public RoomValidCheckInDaysReportController(ICommandService commandService, IRoomValidCheckInDaysReportQueryService roomValidCheckInDaysReportQueryService, IProjectQueryService projectQueryService)
        {
            _commandService = commandService;
            _roomValidCheckInDaysReportQueryService = roomValidCheckInDaysReportQueryService;
            _projectQueryService = projectQueryService;
        }

        public ActionResult Index(RoomValidCheckInDaysReportQuery query = null)
        {
            IndexViewModel viewModel = new IndexViewModel
            {
                Query = query,
                RoomValidCheckInDays = _roomValidCheckInDaysReportQueryService.Query(query),
                ProjectList = _projectQueryService.QueryAllValidByProjectFilter().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };

            return View("~/Views/Reports/RoomValidCheckInDaysReport/Index.cshtml", viewModel);
        }

        public ActionResult Export(ExportRoomValidCheckInDaysReportCommand command)
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