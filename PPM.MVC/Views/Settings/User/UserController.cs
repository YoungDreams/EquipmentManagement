using System;
using System.Linq;
using System.Web.Mvc;
using Foundation.Messaging;
using PPM.Commands;
using PPM.Entities;
using PPM.MVC.Views.Account;
using PPM.Query;
using PPM.Shared;

namespace PPM.MVC.Views.Settings.User
{
    public class UserController : AuthorizedController
    {
        private readonly ICommandService _commandService;
        private readonly IUserQueryService _userQuery;
        private readonly IRoleQueryService _roleQuery;
        public UserController(ICommandService commandService, IUserQueryService userQuery, IRoleQueryService roleQuery)
        {
            _commandService = commandService;
            _userQuery = userQuery;
            _roleQuery = roleQuery;
        }
        /// <summary>
        /// 用户列表查询
        /// </summary>
        /// <param name="page">当前页</param>
        /// <param name="pageSize">分页数</param>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public ActionResult Index(int page = 1, int pageSize = PPM.Web.Common.PaginationSetttings.PageSize, UserQuery query = null)
        {
            //if (!WebAppContext.Current.User.HasPermission(ModuleType.用户管理, Permission.查看))
            //{
            //    return RedirectToAction("NoPermission", "Home");
            //}
            if (query.IsEnabled == null)
            {
                query.IsEnabled = true;
            }
            var viewModel = new IndexViewModel(Url)
            {
                Query = query,
                Items = _userQuery.Query(page, pageSize, query),
                Roles = Enum.GetNames(typeof(RoleType)).Select(x => new SelectListItem { Text = x, Value = x })
            };
            return View("~/Views/Settings/User/Index.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult ChangePassword(int id)
        {
            Entities.User user = _userQuery.Get(id);

            if (user == null)
                throw new ApplicationException("用户信息不存在");

            var viewmodel = new ChangePasswordViewModel
            {
                UserId = user.Id,
                Username = user.Username
            };

            return View("~/Views/Settings/User/ChangePassword.cshtml", viewmodel);

        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordCommand command)
        {
            _commandService.Execute(command);

            return RedirectToAction("LogOff", "Account");
        }
        /// <summary>
        /// 返回创建视图与viewmodel
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create()
        {
            var viewModel = new CreateViewModel
            {
                Roles = Enum.GetNames(typeof(RoleType)).Select(x => new SelectListItem { Text = x, Value = x })
            };
            return View("~/Views/Settings/User/Create.cshtml", viewModel);
        }
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(CreateUserCommand command)
        {
            _commandService.Execute(command);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// 编辑用户-查询单个用户信息
        /// </summary>
        /// <param name="id">用户编号</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Entities.User user = _userQuery.Get(id);
            if (user == null)
                throw new ApplicationException("用户不存在");

            var viewModel = new EditViewModel
            {
                UserId = user.Id,
                Username = user.Username,
                Email = user.Email,
                IsEnabled = user.IsEnabled,
                Phone = user.Phone,
                RealName = user.RealName,
                RoleType = user.RoleType,
                RoleIds = Enum.GetNames(typeof(RoleType)).ToList(),
                Roles = Enum.GetNames(typeof(RoleType)).Select(x => new SelectListItem { Text = x, Value = x })
            };

            return View("~/Views/Settings/User/Edit.cshtml", viewModel);
        }
        /// <summary>
        /// 编辑修改用户
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(EditUserCommand command)
        {
            _commandService.Execute(command);

            return RedirectToAction("Index");
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(DeleteUserCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 启用用户-修改用户状态
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ValidUsers(ValidUsersCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 用户状态失效操作
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InvalidUsers(InvalidUsersCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 用户重置密码
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Index");
        }
    }
}
