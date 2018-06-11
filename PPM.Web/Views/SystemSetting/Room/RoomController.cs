using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Castle.Components.DictionaryAdapter;
using Foundation.Core;
using Foundation.Data;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Query;
using PensionInsurance.Web.Views.Account;
using PensionInsurance.Entities.DetailViews;

namespace PensionInsurance.Web.Views.SystemSetting.Room
{
    public class RoomController : AuthorizedController
    {
        private readonly ICommandService _commandService;
        private readonly IFetcher _fetcher;
        private readonly IRoomQueryService _roomQueryServiceService;
        private readonly IBuildingQueryService _buildingQueryService;
        private readonly IUnitQueryService _unitQueryService;
        private readonly IFloorQueryService _floorQueryService;
        private readonly IRoomTypeService _roomTypeService;
        private readonly IBedQueryService _bedQueryService;

        public RoomController(ICommandService commandService, IFetcher fetcher, IRoomQueryService roomQueryServiceService, IBuildingQueryService buildingQueryService, IUnitQueryService unitQueryService, IFloorQueryService floorQueryService, IRoomTypeService roomTypeService, IBedQueryService bedQueryService)
        {
            _commandService = commandService;
            _fetcher = fetcher;
            _roomQueryServiceService = roomQueryServiceService;
            _buildingQueryService = buildingQueryService;
            _unitQueryService = unitQueryService;
            _floorQueryService = floorQueryService;
            _roomTypeService = roomTypeService;
            _bedQueryService = bedQueryService;
        }

        /// <summary>
        /// 查询页面
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public ActionResult Index(int page = 1, int pageSize = Web.Common.PaginationSetttings.PageSize, RoomQuery query = null)
        {
            var project = _fetcher.Get<Entities.Project>(query.ProjectId);

            var viewModel = new IndexViewModel(Url)
            {
                Query = query,
                ProjectId = query.ProjectId,
                Items = _roomQueryServiceService.Query(page, pageSize, query),
                Buildings = _buildingQueryService.Query(new BuildingQuery { ProjectId = query.ProjectId }),
                FloorCount = project.FloorCount,
                UnitCount = project.UnitCount,
                RoomTypes = _roomTypeService.Query(new RoomTypeQuery {ProjectId = query.ProjectId} ).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList()
            };
            return View("~/Views/SystemSetting/Room/Index.cshtml", viewModel);
        }

        /// <summary>
        /// 根据projectId来获取FancyTree页面数据
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public JsonResult GetRoomData(int projectId)
        {
            var fancyTreeViewModel = GetFancyTreeForRoom(projectId);
            return Json(fancyTreeViewModel,JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 新增处理
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(CreateRoomCommand command, string returnUrl)
        {
            command.AddFlag = 0;
            command.RoomCount = 1;
            _commandService.Execute(command);
            return returnUrl.IsNullOrWhiteSpace() ? (ActionResult)RedirectToAction("Index") : Redirect(returnUrl);
        }

        /// <summary>
        /// 获取确认房间列表
        /// </summary>
        /// <param name="roomCount"></param>
        /// <param name="building"></param>
        /// <param name="unit"></param>
        /// <param name="floor"></param>
        /// <param name="remark"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreateListConfirm(int roomCount,int building,int unit,int floor,string remark,int projectId)
        {
            List<CreateRoomViewModel> createRoomViewModels = new List<CreateRoomViewModel>();
            for (int i = 0; i < roomCount; i++)
            {
                CreateRoomViewModel createRoomViewModel = new CreateRoomViewModel();
                createRoomViewModel.Id = i;
                createRoomViewModel.Building = building;
                createRoomViewModel.BuildingName = _buildingQueryService.Query(new BuildingQuery {ProjectId = projectId }).FirstOrDefault().Name;
                createRoomViewModel.Unit = unit;
                createRoomViewModel.UnitName = _unitQueryService.Query(new UnitQuery {BuildingId = building }).FirstOrDefault(m => m.Id == unit).Name;
                createRoomViewModel.Floor = floor;
                createRoomViewModel.FloorName = _floorQueryService.Query(new FloorQuery {UnitId = unit }).FirstOrDefault(m => m.Id == floor).Name;
                createRoomViewModel.Remark = remark;
                createRoomViewModels.Add(createRoomViewModel);
            }
            var createRoomListViewModel = new CreateRoomListViewModel
            {
                CreateRoomViewModels = createRoomViewModels,
                RoomTypes = _roomTypeService.Query(new RoomTypeQuery {ProjectId = projectId }).Select(x => new SelectListItem{
                        Text = x.Name,
                        Value = x.Id.ToString()
                    })
                    .ToList()
            };
            return View("~/Views/SystemSetting/Room/CreateRoomList.cshtml", createRoomListViewModel);
        }

        /// <summary>
        /// 批量新增确认
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateListConfirm(CreateRoomsCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Index", new RoomQuery { ProjectId = command.ProjectId });
        }

        /// <summary>
        /// 批量新增处理
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateList(CreateRoomCommand command, string returnUrl)
        {
            command.AddFlag = 1;
            _commandService.Execute(command);
            return returnUrl.IsNullOrWhiteSpace() ? (ActionResult)RedirectToAction("Index") : Redirect(returnUrl);
        }

        /// <summary>
        /// 跳转到编辑页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(int id)
        {
            RoomDetail roomDetail = (RoomDetail)_roomQueryServiceService.QueryByRoomId(id);
            Entities.Room room = _roomQueryServiceService.Get(id);
            var viewModel = new EditViewModel
            {
                RoomId = room.Id,
                RoomNo = room.RoomNo,
                Name = room.Name,
                TypeId = room.RoomType.Id,
                Status = room.Status,
                Remark = room.Remark,
                BuildingName = room.Floor.Unit.Building.Name,
                FloorName = room.Floor.Name,
                UnitName = room.Floor.Unit.Name,
                HeaderText = "编辑",
            };
            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 编辑提交
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(EditRoomCommand command, string returnUrl)
        {
            var room = _fetcher.Get<Entities.Room>(command.RoomId);
            //command.RoomNo = room.RoomNo;
            //command.Status = room.Status;
            _commandService.Execute(command);
            return returnUrl.IsNullOrWhiteSpace() ? (ActionResult)RedirectToAction("Index") : Redirect(returnUrl);
        }

        /// <summary>
        /// 删除处理
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(DeleteRoomCommand command)
        {
            _commandService.Execute(command);
            return command.ReturnUrl.IsNullOrWhiteSpace()
                ? (ActionResult)RedirectToAction("Index")
                : Redirect(command.ReturnUrl);
        }

        [HttpGet]
        public ActionResult GetUnits(int? buildingId)
        {
            var units = _unitQueryService.Query(new UnitQuery { BuildingId = buildingId });
            return Json(units.Select(x => new
            {
                Value = x.Id,
                Text = x.Name
            }), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetFloors(int? unitId)
        {
            var floors = _floorQueryService.Query(new FloorQuery { UnitId = unitId });
            return Json(floors.Select(x => new
            {
                Value = x.Id,
                Text = x.Name
            }), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 跳转到床位管理页面
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ManageBed(int roomId)
        {
            var room = _fetcher.Get<Entities.Room>(roomId);
            var viewModel = new EditViewModel
            {
                RoomId = room.Id,
                RoomNo = room.RoomNo,
                Type = room.Type,
                Status = room.Status,
                Remark = room.Remark,
                HeaderText = "编辑",
            };
            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 床位管理提交
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ManageBed(EditRoomCommand command, string returnUrl)
        {
            var room = _fetcher.Get<Entities.Room>(command.RoomId);
            command.RoomNo = room.RoomNo;
            command.Status = room.Status;
            _commandService.Execute(command);
            return returnUrl.IsNullOrWhiteSpace() ? (ActionResult)RedirectToAction("Index") : Redirect(returnUrl);
        }

        [HttpGet]
        public ActionResult GetRooms(int floorId, bool isFullRoom, int? contractId, bool isAgreement)
        {
            var rooms = contractId.HasValue
                ? _roomQueryServiceService.GetAvailableRooms(floorId, isFullRoom, contractId.Value)
                : _roomQueryServiceService.GetAvailableRooms(floorId, isFullRoom);



            Entities.Contract contract;
            if (contractId.HasValue && isAgreement)
            {
                contract = _fetcher.Get<Entities.Contract>(contractId.Value);

                if (isFullRoom)
                {
                    return Json(rooms.Select(x => new
                    {
                        Value = x.Id,
                        Type = x.RoomType.Name,
                        Text = x.Name,
                        RoomCostDate = contract.RoomCostDate.ToString(),
                        ShortCost =
                            x.RoomType.ActualRoomCost(contract.RoomCostDate).ShortTermCost*
                            x.Beds.Count(b => b.IsEnabled),
                        LongCost =
                            x.RoomType.ActualRoomCost(contract.RoomCostDate).LongTermCost*x.Beds.Count(b => b.IsEnabled)
                    }), JsonRequestBehavior.AllowGet);
                }

                return Json(rooms.Select(x => new
                {
                    Value = x.Id,
                    Type = x.RoomType.Name,
                    Text = x.Name,
                    RoomCostDate = contract.RoomCostDate.ToString(),
                    ShortCost = x.RoomType.ActualRoomCost(contract.RoomCostDate).ShortTermCost,
                    LongCost = x.RoomType.ActualRoomCost(contract.RoomCostDate).LongTermCost
                }), JsonRequestBehavior.AllowGet);
            }
            
            if (isFullRoom)
            {
                return Json(rooms.Select(x => new
                {
                    Value = x.Id,
                    Type = x.RoomType.Name,
                    Text = x.Name,
                    RoomCostDate= x.RoomType.ActualRoomCost(null).EnabledTime.ToString(),
                    ShortCost = x.RoomType.ActualRoomCost(null).ShortTermCost * x.Beds.Count(b => b.IsEnabled),
                    LongCost = x.RoomType.ActualRoomCost(null).LongTermCost * x.Beds.Count(b => b.IsEnabled)
                }), JsonRequestBehavior.AllowGet);
            }

            return Json(rooms.Select(x => new
            {
                Value = x.Id,
                Type = x.RoomType.Name,
                Text = x.Name,
                RoomCostDate = x.RoomType.ActualRoomCost(null).EnabledTime.ToString(),
                ShortCost = x.RoomType.ActualRoomCost(null).ShortTermCost,
                LongCost = x.RoomType.ActualRoomCost(null).LongTermCost
            }), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetBeds(int roomId, int? contractId)
        {
            var beds = contractId.HasValue
                ? _roomQueryServiceService.GetAvailableBeds(roomId, contractId.Value)
                : _roomQueryServiceService.GetAvailableBeds(roomId);

            return Json(beds.Select(x => new
            {
                Value = x.Id,
                Text = x.Name,
            }), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据ProjectId获取FancyTree模型
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        private FancyTreeViewModel GetFancyTreeForRoom(int projectId)
        {
            var fancyTreeViewModel = new FancyTreeViewModel() { title = "楼栋", children = new List<FancyTreeViewModel>() };
            var Buildings = _buildingQueryService.Query(new BuildingQuery { ProjectId = projectId });
            foreach (var building in Buildings)
            {
                var builingFancyTreeViewModel = new FancyTreeViewModel();
                builingFancyTreeViewModel.title = building.Name;
                builingFancyTreeViewModel.folder = true;
                builingFancyTreeViewModel.expanded = true;
                builingFancyTreeViewModel.data = new FancyTreeViewModelData { Id = building.Id, Kind = "Building" };
                builingFancyTreeViewModel.children = new EditableList<FancyTreeViewModel>();
                building.Units = _unitQueryService.Query(new UnitQuery { BuildingId = building.Id }).ToList();
                foreach (var unit in building.Units)
                {
                    var unitFancyTreeViewModel = new FancyTreeViewModel();
                    unitFancyTreeViewModel.title = unit.Name;
                    unitFancyTreeViewModel.folder = true;
                    unitFancyTreeViewModel.expanded = true;
                    unitFancyTreeViewModel.data = new FancyTreeViewModelData { Id = unit.Id, Kind = "Unit" };
                    unitFancyTreeViewModel.children = new EditableList<FancyTreeViewModel>();
                    unit.Floors = _floorQueryService.Query(new FloorQuery { UnitId = unit.Id }).ToList();
                    foreach (var floor in unit.Floors)
                    {
                        var floorFancyTreeViewModel = new FancyTreeViewModel();
                        floorFancyTreeViewModel.title = floor.Name;
                        floorFancyTreeViewModel.folder = true;
                        floorFancyTreeViewModel.data = new FancyTreeViewModelData { Id = floor.Id, Kind = "Floor" };
                        floorFancyTreeViewModel.children = new EditableList<FancyTreeViewModel>();
                        floor.Rooms = _roomQueryServiceService.QueryByFloorId(floor.Id).ToList();
                        foreach (var room in floor.Rooms)
                        {
                            var roomFancyTreeViewModel = new FancyTreeViewModel();
                            roomFancyTreeViewModel.title = room.RoomNo + " " + room.Type + " " + room.Status;
                            roomFancyTreeViewModel.data = new FancyTreeViewModelData { Id = room.Id, Kind = "Room" };
                            roomFancyTreeViewModel.children = new EditableList<FancyTreeViewModel>();
                            room.Beds = _bedQueryService.QueryByRoomId(new BedQuery { RoomId = room.Id });
                            foreach (var bed in room.Beds)
                            {
                                var bedFancyTreeViewModel = new FancyTreeViewModel();
                                bedFancyTreeViewModel.title = bed.BedNo + " " + bed.Status;
                                bedFancyTreeViewModel.data = new FancyTreeViewModelData { Id = bed.Id, Kind = "Bed" };
                                bedFancyTreeViewModel.children = new EditableList<FancyTreeViewModel>();
                                roomFancyTreeViewModel.children.Add(bedFancyTreeViewModel);
                            }
                            floorFancyTreeViewModel.children.Add(roomFancyTreeViewModel);
                        }
                        unitFancyTreeViewModel.children.Add(floorFancyTreeViewModel);
                    }
                    builingFancyTreeViewModel.children.Add(unitFancyTreeViewModel);
                }
                fancyTreeViewModel.children.Add(builingFancyTreeViewModel);
            }

            return fancyTreeViewModel;
        }
    }

}