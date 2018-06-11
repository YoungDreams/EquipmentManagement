using System;
using System.Linq;
using System.Web.Mvc;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Query;
using PensionInsurance.Web.Views.Account;

namespace PensionInsurance.Web.Views.Reports.UndoWorkflowProgressReport
{
    public class UndoWorkflowProgressReportController : AuthorizedController
    {
        private readonly IUserBacklogQueryService _backlogQueryService;
        private readonly IProjectQueryService _projectQueryService;
        private readonly ICommandService _commandService;
        public UndoWorkflowProgressReportController(IUserBacklogQueryService backlogQueryService, ICommandService commandService, IProjectQueryService projectQueryService)
        {
            _backlogQueryService = backlogQueryService;
            _commandService = commandService;
            _projectQueryService = projectQueryService;
        }

        public ActionResult Index(int page = 1, int pageSize = Web.Common.PaginationSetttings.PageSize, UserBacklogQuery query = null)
        {
            IndexViewModel viewModel = new IndexViewModel
            {
                Query = query,
                UndoWorkflowProgresses =
                    _backlogQueryService.GetCurrentUserProjectWorkflowProgress(Shared.WebAppContext.Current.User.Id,
                        page, pageSize, query),
                ProjectList = _projectQueryService.QueryAllValidByProjectFilter().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                WorkflowCategories = Enum.GetNames(typeof(WorkflowCategory)).Select(x => new SelectListItem
                {
                    Text = x,
                    Value = x
                })
            };
            return View("~/Views/Reports/UndoWorkflowProgressReport/Index.cshtml", viewModel);
        }

        public ActionResult Export()
        {
            var result = _commandService.ExecuteFoResult(new ExportUserBacklogCommand());

            return Json(new
            {
                success = result.IsSucceed,
                redirect = result.UrlPath
            }, JsonRequestBehavior.AllowGet);
        }
    }
}