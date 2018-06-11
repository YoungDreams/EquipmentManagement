using System.Web.Mvc;
using PensionInsurance.Query;
using PensionInsurance.Web.Views.Account;

namespace PensionInsurance.Web.Views.UserBacklog
{
    public class UserBacklogController: AuthorizedController
    {
        private readonly IUserBacklogQueryService _backlogQueryService;
        public UserBacklogController(IUserBacklogQueryService backlogQueryService)
        {
            _backlogQueryService = backlogQueryService;
        }

        public ActionResult UndoList()
        {
            IndexViewModel viewModel = new IndexViewModel();
            viewModel.UndoUserBacklogs = _backlogQueryService.GetCurrentUnDoUserBacklogs();
            return View("Index.Undo",viewModel);
        }

        public ActionResult DoneList(int page = 1, int pageSize = Web.Common.PaginationSetttings.PageSize)
        {
            IndexViewModel viewModel = new IndexViewModel();
            viewModel.DoneUserBacklogs = _backlogQueryService.GetCurrentDoneUserBacklogs(Shared.WebAppContext.Current.User.Id, page, pageSize);
            return View("Index.Done",viewModel);
        }

        public ActionResult WorkflowProgressUndoList(int page = 1, int pageSize = Web.Common.PaginationSetttings.PageSize)
        {
            IndexViewModel viewModel = new IndexViewModel();
            viewModel.UndoWorkflowProgresses = _backlogQueryService.GetCurrentUndoWorkflowProgress(Shared.WebAppContext.Current.User.Id, page, pageSize);
            return View("Index.WorkflowProgress.Undo", viewModel);
        }

        public ActionResult WorkflowProgressDoneList(int page = 1, int pageSize = Web.Common.PaginationSetttings.PageSize)
        {
            IndexViewModel viewModel = new IndexViewModel();
            viewModel.DoneWorkflowProgresses = _backlogQueryService.GetCurrentDoneWorkflowProgress(Shared.WebAppContext.Current.User.Id, page, pageSize);
            return View("Index.WorkflowProgress.Done", viewModel);
        }
    }
}