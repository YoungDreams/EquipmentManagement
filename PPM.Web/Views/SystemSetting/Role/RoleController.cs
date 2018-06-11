using System;
using System.Linq;
using System.Web.Mvc;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Query;
using PensionInsurance.Shared;
using PensionInsurance.Web.Views.Account;

namespace PensionInsurance.Web.Views.SystemSetting.Role
{
    public class RoleController : AuthorizedController
    {
        private readonly IRoleQueryService _roleQueryService;
        private readonly ICommandService _commandService;
        
        public RoleController(IRoleQueryService roleQueryService, ICommandService commandService)
        {
            _commandService = commandService;
            _roleQueryService = roleQueryService;
        }
        /// <summary>
        /// 角色列表查询
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public ActionResult Index(int page = 1, int pageSize = Web.Common.PaginationSetttings.PageSize, RoleQuery query = null)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.角色管理, Permission.查看))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var viewModel = new IndexViewModel
            {
                Query = query,
                Items = _roleQueryService.Query(page, pageSize, query)
            };
            return View("~/Views/SystemSetting/Role/Index.cshtml", viewModel);
        }
        /// <summary>
        /// 创建角色返回视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create()
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.角色管理, Permission.新增))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var viewModel = new CreateViewModel();
            return View("~/Views/SystemSetting/Role/Create.cshtml", viewModel);
        }
        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(CreateRoleCommand command)
        {
            _commandService.Execute(command);

            return RedirectToAction("Index");
        }
        /// <summary>
        /// 修改角色 返回单个角色信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.角色管理, Permission.编辑))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            Entities.Role role = _roleQueryService.Get(id);
            if (role == null)
                throw new ApplicationException("Role cannot be found");

            var viewModel = new EditViewModel
            {
                RoleId = role.Id,
                RoleName = role.Name,
                Description = role.Description,
                RoleType = role.RoleType
            };

            return View("~/Views/SystemSetting/Role/Edit.cshtml", viewModel);
        }
        /// <summary>
        /// 执行角色修改操作
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(EditRoleCommand command)
        {
            _commandService.Execute(command);

            return RedirectToAction("Index");
        }
        /// <summary>
        /// 权限分配
        /// </summary>
        /// <param name="query"></param>
        /// <returns>视图</returns>
        [HttpGet]
        public ActionResult EditPermission(RolePermissionQuery query)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.角色管理, Permission.编辑))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var permissionEditViewModel = new EditPermissionViewModel()
            {
                RoleId = query.RoleId,
                RolePermissions = _roleQueryService.QueryRolePermissions(query).ToList()
            };
            return View("~/Views/SystemSetting/Role/EditPermission.cshtml", permissionEditViewModel);
        }
        /// <summary>
        /// 权限分配保存操作
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditPermission(EditPermissionCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Index");
        }
    }
}