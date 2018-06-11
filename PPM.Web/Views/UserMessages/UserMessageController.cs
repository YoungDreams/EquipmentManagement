using System.Web.Mvc;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Query;
using PensionInsurance.Web.Views.Account;

namespace PensionInsurance.Web.Views.UserMessages
{
    public class UserMessageController : AuthorizedController
    {
        private readonly IUserNotificationQueryService _userNotificationQueryService;
        private readonly ICommandService _commandService;
        public UserMessageController(IUserNotificationQueryService userNotificationQueryService, ICommandService commandService)
        {
            _userNotificationQueryService = userNotificationQueryService;
            _commandService = commandService;
        }

        /// <summary>
        /// 未查阅
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UnreadList()
        {
            IndexViewModel viewModel = new IndexViewModel();
            var unReadlist =
                _userNotificationQueryService.GetCurrentUnDoUserNotifications(Shared.WebAppContext.Current.User.Id);
            viewModel.UndoMessages = unReadlist;
            return View("~/views/usermessages/index.Unread.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult ReadList()
        {
            IndexViewModel viewModel = new IndexViewModel();
            var readlist =
                _userNotificationQueryService.GetCurrentDoneUserNotifications(Shared.WebAppContext.Current.User.Id);
            viewModel.DoneMessages = readlist;

            return View("~/views/usermessages/index.Read.cshtml", viewModel);
        }

        [HttpPost]
        public void ReadMessage(SetUserNotificationReadCommand command)
        {
            _commandService.Execute(command);
        }
    }
}