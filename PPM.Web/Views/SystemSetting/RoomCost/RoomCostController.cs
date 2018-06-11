using System;
using System.Linq;
using System.Web.Mvc;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Query;
using PensionInsurance.Shared;
using PensionInsurance.Web.Views.Account;

namespace PensionInsurance.Web.Views.SystemSetting.RoomCost
{
    public class RoomCostController : AuthorizedController
    {
        private readonly ICommandService _commandService;
        private readonly IRoomTypeService _roomTypeService;
        private readonly IRoomCostService _roomCostService;
        private readonly IProjectQueryService _projectQueryService;

        public RoomCostController(ICommandService commandService, IRoomTypeService roomTypeService, IRoomCostService roomCostService, IProjectQueryService projectQueryService)
        {
            _commandService = commandService;
            _roomTypeService = roomTypeService;
            _roomCostService = roomCostService;
            _projectQueryService = projectQueryService;
        }

        /// <summary>
        /// 房型查询
        /// </summary>
        /// <param name="page">当前页</param>
        /// <param name="pageSize">分页数</param>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public ActionResult Index(int page = 1, int pageSize = Web.Common.PaginationSetttings.PageSize, RoomCostQuery query = null)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.房型费用管理, Permission.查看))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var viewModel = new RoomCost.IndexViewModel(Url)
            {
                Query = query,
                RoomCosts = _roomCostService.Query(page, pageSize, query),
                Projects = _projectQueryService.QueryAllValidByProjectFilter().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            return View("~/Views/SystemSetting/RoomCost/Index.cshtml", viewModel);
        }

        /// <summary>
        /// 跳转到新增页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create()
        {
            var roomTypes = _roomTypeService.QueryAll().Select(x => new SelectListItem
            {
                Text = x.Project.Name + " - " + x.Name,
                Value = x.Id.ToString()
            }).ToList();
            var viewModel = new CreateViewModel()
            {
                RoomTypes = roomTypes
            };
            return View("~/Views/SystemSetting/RoomCost/Create.cshtml", viewModel);
        }

        /// <summary>
        /// 房型费用新增处理
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(CreateRoomCostCommand command)
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
            var roomCost = _roomCostService.Get(id);
            var roomTypes = _roomTypeService.QueryAll().Select(x => new SelectListItem
            {
                Text = x.Project.Name + " - " + x.Name,
                Value = x.Id.ToString()
            }).ToList();

            var viewModel = new EditViewModel
            {
                Id = roomCost.Id,
                LongTermCost = roomCost.LongTermCost,
                ShortTermCost = roomCost.ShortTermCost,
                IsEnabled = roomCost.IsEnabled,
                RoomTypes = roomTypes,
                RoomTypeId = roomCost.RoomType.Id
            };
            return View("~/Views/SystemSetting/RoomCost/Edit.cshtml", viewModel);
        }

        /// <summary>
        /// 编辑页面提交
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(EditRoomCostCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 删除房型
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(DeleteEntityCommand command)
        {
            Entities.RoomCost roomCost = _roomCostService.Get(command.EntityId);
            if (roomCost == null)
                throw new ApplicationException("RoomCost cannot be found");

            _commandService.Execute(DeleteEntityCommand.Of(roomCost));

            return RedirectToAction("Index");
        }

    }
}