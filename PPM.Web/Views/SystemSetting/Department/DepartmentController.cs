using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Query;
using PensionInsurance.Shared;
using PensionInsurance.Web.Views.Account;

namespace PensionInsurance.Web.Views.SystemSetting.Department
{
    public class DepartmentController : AuthorizedController
    {
        private readonly ICommandService _commandService;
        private readonly IDepartmentQueryService _departmentQuery;
        private readonly IRoleQueryService _roleQuery;
        private readonly IProjectQueryService _projectQueryService;
        private readonly IUserQueryService _userQueryService;

        public DepartmentController(ICommandService commandService, IDepartmentQueryService departmentQuery, IRoleQueryService roleQuery, IProjectQueryService projectQuery, IUserQueryService userQueryService)
        {
            _commandService = commandService;
            _departmentQuery = departmentQuery;
            _roleQuery = roleQuery;
            _projectQueryService = projectQuery;
            _userQueryService = userQueryService;
        }

        /// <summary>
        /// 部门列表查询
        /// </summary>
        /// <param name="page">当前页</param>
        /// <param name="pageSize">分页数</param>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public ActionResult Index(int page = 1, int pageSize = Web.Common.PaginationSetttings.PageSize, DepartmentQuery query = null)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.部门管理, Permission.查看))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var viewModel = new IndexViewModel(Url)
            {
                Query = query,
                Items = _departmentQuery.Query(page, pageSize, query),
                Projects  = _projectQueryService.QueryAllValidByProjectFilter().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            return View("~/Views/SystemSetting/Department/Index.cshtml", viewModel);
        }

        /// <summary>
        /// render department create action
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create()
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.部门管理, Permission.新增))
            {
                return RedirectToAction("NoPermission", "Home");
            }
            var projects = _projectQueryService.QueryAllValid().Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();
            var viewmodel = new CreateViewModel
            {
                Projects = projects,
                Sort = 99,
            };

            return View("~/Views/SystemSetting/Department/Create.cshtml", viewmodel);
        }

        public JsonResult GetUserbyProjectId(int projectId)
        {
            var users = _userQueryService.GetUsersByProjectId(projectId).Select(x => new SelectListItem
            {
                Text = x.RealName,
                Value = x.Id.ToString()
            });
            return Json(users, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 创建用户操作
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(CreateDepartmentCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 编辑部门-查询单个用户信息
        /// </summary>
        /// <param name="id">用户编号</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.部门管理, Permission.编辑))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var department = _departmentQuery.Get(id);
            var projects = _projectQueryService.QueryAllValid().Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();
            if (department == null)
                throw new ApplicationException("部门不存在");

            var viewModel = new EditViewModel
            {
                Id = department.Id,
                Name = department.Name,
                Code = department.Code,
                Sort = department.Sort,
                ManagementUserId = department.ManagementUser?.Id,
                ProjectId = department.Project?.Id,
                Projects = projects
            };

            return View("~/Views/SystemSetting/Department/Edit.cshtml", viewModel);
        }

        /// <summary>
        /// 编辑部门操作
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(EditDepartmentCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(DeleteDepartmentCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Index");
        }
    }
}

