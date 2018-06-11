using System.Web.Mvc;
using System.Web.Security;
using Foundation.Messaging;
using PensionInsurance.Commands;

namespace PensionInsurance.Web.Views.Account
{
    public class AccountController : MvcController
    {
        private readonly ICommandService _commandService;

        public AccountController(ICommandService commandService)
        {
            _commandService = commandService;
        }
        /// <summary>
        /// 登陆视图
        /// </summary>
        /// <returns></returns>
        // GET: Account/LogOn
        public ActionResult LogOn()
        {
            return View();
        }
        /// <summary>
        /// 登陆操作
        /// </summary>
        /// <param name="command"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        // POST: Account/LogOn
        [HttpPost]
        public ActionResult LogOn(LoginCommand command, string returnUrl)
        {
            _commandService.Execute(command);
            FormsAuthentication.SetAuthCookie(command.Username, createPersistentCookie: false);
            if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                           && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

      /// <summary>
      /// 注销登陆
      /// </summary>
      /// <returns></returns>
        // GET: Account/LogOff
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("LogOn");
        }
    }
}
