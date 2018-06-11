using System;
using System.Linq;
using System.Web.Mvc;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Query;
using PensionInsurance.Shared;
using PensionInsurance.Web.Views.Account;

namespace PensionInsurance.Web.Views.SystemSetting.RoomType
{
    public class RoomTypeController : AuthorizedController
    {
        private readonly ICommandService _commandService;
        private readonly IRoomTypeService _roomTypeService;
        private readonly IProjectQueryService _projectQueryService;

        public RoomTypeController(ICommandService commandService, IRoomTypeService roomTypeService, IProjectQueryService projectQueryService)
        {
            _commandService = commandService;
            _roomTypeService = roomTypeService;
            _projectQueryService = projectQueryService;
        }

        /// <summary>
        /// 房型查询
        /// </summary>
        /// <param name="page">当前页</param>
        /// <param name="pageSize">分页数</param>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public ActionResult Index(int page = 1, int pageSize = Web.Common.PaginationSetttings.PageSize, RoomTypeQuery query = null)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.房型管理, Permission.查看))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var viewModel = new IndexViewModel(Url)
            {
                Query = query,
                RoomTypes = _roomTypeService.Query(page, pageSize, query),
                Projects = _projectQueryService.QueryAllValidByProjectFilter().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            return View("~/Views/SystemSetting/RoomType/Index.cshtml", viewModel);
        }

        /// <summary>
        /// 跳转到新增页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create()
        {
            var projects = _projectQueryService.QueryAllValidByProjectFilter().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();
            var houseTypes = Enum.GetNames(typeof(HouseType)).Select(x => new SelectListItem
            {
                Value = x,
                Text = x.ToString()
            }).ToList();
            var viewModel = new CreateViewModel()
            {
                Projects = projects,
                HouseTypes = houseTypes,
                BedCount = 0
            };
            return View("~/Views/SystemSetting/RoomType/Create.cshtml", viewModel);
        }

        /// <summary>
        /// 房型新增处理
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(CreateRoomTypeCommand command)
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
            var roomType = _roomTypeService.Get(id);
            var projects = _projectQueryService.QueryAllValidByProjectFilter().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();
            var houseTypes = Enum.GetNames(typeof(HouseType)).Select(x => new SelectListItem
            {
                Value = x,
                Text = x.ToString()
            }).ToList();
            var viewModel = new EditViewModel
            {
                Id = roomType.Id,
                Name = roomType.Name,
                HouseType = roomType.HouseType,
                ProjectId = roomType.Project.Id,
                Projects = projects,
                BedCount = roomType.BedCount,
                HouseTypes = houseTypes
            };
            return View("~/Views/SystemSetting/RoomType/Edit.cshtml", viewModel);
        }

        /// <summary>
        /// 编辑页面提交
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(EditRoomTypeCommand command)
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
            Entities.RoomType roomType = _roomTypeService.Get(command.EntityId);
            if (roomType == null)
                throw new ApplicationException("RoomType cannot be found");

            _commandService.Execute(DeleteEntityCommand.Of(roomType));

            return RedirectToAction("Index");
        }
    }
}