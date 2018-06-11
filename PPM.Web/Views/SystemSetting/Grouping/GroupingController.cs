using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Foundation.Data.Implemention;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Query;
using PensionInsurance.Shared;
using PensionInsurance.Web.Views.Account;

namespace PensionInsurance.Web.Views.SystemSetting.Grouping
{
    public class GroupingController : AuthorizedController
    {
        private readonly IGroupingQueryService _groupingQueryService;
        private readonly IDepartmentQueryService _departmentQueryService;
        private readonly ICommandService _commandService;
        
        public GroupingController(ICommandService commandService, IGroupingQueryService groupingQueryService, IDepartmentQueryService departmentQueryService)
        {
            _commandService = commandService;
            _groupingQueryService = groupingQueryService;
            _departmentQueryService = departmentQueryService;
        }

        public ActionResult Index(int page = 1, int pageSize = Web.Common.PaginationSetttings.PageSize, GroupingQuery query = null)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.组织架构管理, Permission.查看))
            {
                return RedirectToAction("NoPermission", "Home");
            }
            var departments =
                _departmentQueryService.GetDepartments().ToList();
            var viewModel = new IndexViewModel(Url)
            {
                Query = query,
                Groupings = _groupingQueryService.Query(page, pageSize, query).Select(x => new GroupingItemViewmodel
                {
                    Code = x.Code,
                    Id = x.Id,
                    Name = x.Name,
                    Layer = x.Layer,
                    Sort = x.Sort,
                    DepartementName = departments.FirstOrDefault(d => d.Id == x.Department.Id)?.Name,
                    DepartementId = departments.FirstOrDefault(d => d.Id == x.Department.Id).Id
                }).ToPagedData(page, pageSize)
            };
            return View("~/Views/SystemSetting/Grouping/Index.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult Create(int departmentId)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.组织架构管理, Permission.新增))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var viewModel = new CreateViewModel
            {
                DepartmentId = departmentId,
                Departments = _departmentQueryService.GetDepartments().Where(x => x.Id == departmentId).Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList()
            };
            return View("~/Views/SystemSetting/Grouping/Create.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult Create(CreateGroupingCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Index", new { departmentId = command.DepartmentId });
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.组织架构管理, Permission.编辑))
            {
                return RedirectToAction("NoPermission", "Home");
            }
            var grouping = _groupingQueryService.Get(id);
            var viewModel = new EditViewModel
            {
                Id = grouping.Id,
                Name = grouping.Name,
                Code = grouping.Code,
                Sort = grouping.Sort,
                DepartmentId = grouping.Department.Id,
                Departments = _departmentQueryService.GetDepartments().Where(x => x.Id == grouping.Department.Id).Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList()
            };

            return View("~/Views/SystemSetting/Grouping/Edit.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult Edit(EditGroupingCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Index", new { departmentId = command.DepartmentId });
        }

        [HttpPost]
        public ActionResult Delete(DeleteGroupingCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Index", new { departmentId = command.DepartmentId });
        }
    }
}