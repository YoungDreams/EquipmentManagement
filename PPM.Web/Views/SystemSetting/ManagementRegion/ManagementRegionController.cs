using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Query;
using PensionInsurance.Shared;
using PensionInsurance.Web.Views.Account;

namespace PensionInsurance.Web.Views.SystemSetting.ManagementRegion
{
    public class ManagementRegionController : AuthorizedController
    {
        private readonly ICommandService _commandService;
        private readonly IManagementRegionService _managementRegionService;
        private readonly IAreaQueryService _areaQueryService;
        private readonly IUserQueryService _userQueryService;

        public ManagementRegionController(ICommandService commandService, IManagementRegionService managementRegionService, IUserQueryService userQueryService, IAreaQueryService areaQueryService)
        {
            _commandService = commandService;
            _managementRegionService = managementRegionService;
            _userQueryService = userQueryService;
            _areaQueryService = areaQueryService;
        }

        /// <summary>
        /// 部门列表查询
        /// </summary>
        /// <param name="page">当前页</param>
        /// <param name="pageSize">分页数</param>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public ActionResult Index(int page = 1, int pageSize = Web.Common.PaginationSetttings.PageSize, ManagementRegionQuery query = null)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.管理区域管理, Permission.查看))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var viewModel = new IndexViewModel(Url)
            {
                Query = query,
                ManagementRegions = _managementRegionService.Query(page, pageSize, query),
            };
            return View("~/Views/SystemSetting/ManagementRegion/Index.cshtml", viewModel);
        }

        /// <summary>
        /// 跳转到新增页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create()
        {
            var users = _userQueryService.QueryAllValid().Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.RealName
            }).ToList();
            var cities = _areaQueryService.QueryCities().Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();
            var viewModel = new CreateViewModel()
            {
                Users = users,
                Cities = cities,
                Sort = 99
            };
            return View("~/Views/SystemSetting/ManagementRegion/Create.cshtml", viewModel);
        }

        /// <summary>
        /// 服务包新增处理
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(CreateManagementRegionCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 跳转到编辑页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var managementRegion = _managementRegionService.Get(id);
            var users = _userQueryService.QueryAllValid().Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.RealName
            }).ToList();
            var cities = _areaQueryService.QueryCities().Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();
            var viewModel = new EditViewModel
            {
                Id = managementRegion.Id,
                Name = managementRegion.Name,
                AreaId = managementRegion.City.Id,
                Sort = managementRegion.Sort,
                ManagementUserId = managementRegion.Manager.Id,
                Cities = cities,
                Users = users,
            };
            return View("~/Views/SystemSetting/ManagementRegion/Edit.cshtml", viewModel);
        }

        /// <summary>
        /// 编辑页面提交
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(EditManagementRegionCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 删除服务包
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(DeleteEntityCommand command)
        {
            Entities.ManagementRegion managementRegion = _managementRegionService.Get(command.EntityId);
            if (managementRegion == null)
                throw new ApplicationException("ManagementRegion cannot be found");

            _commandService.Execute(DeleteEntityCommand.Of(managementRegion));

            return RedirectToAction("Index");
        }
    }
}