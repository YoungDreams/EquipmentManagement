using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Foundation.Core;
using Foundation.Data;
using Foundation.Messaging;
using PensionInsurance.CommandHandlers;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Query;
using PensionInsurance.Shared;
using PensionInsurance.Web.Views.Account;
using ConsultingFamily = PensionInsurance.Commands.ConsultingFamily;

namespace PensionInsurance.Web.Views.Sales.Consulting
{
    public class ConsultingController : AuthorizedController
    {
        private readonly ICommandService _commandService;
        private readonly IFetcher _fetcher;
        private readonly IConsultingQueryService _consultingQueryService;
        private readonly IFavoriteFolderQueryService _favoriteFolderQueryService;
        private readonly IUserQueryService _userQueryService;
        private readonly IProjectQueryService _projectQueryService;
        private readonly ISettingQueryService _settingQueryService;
        private readonly IRoomQueryService _roomQueryService;
        public ConsultingController(ICommandService commandService, IFetcher fetcher,
            IFavoriteFolderQueryService favoriteFolderQueryService, IUserQueryService userQueryService,
            IProjectQueryService projectQueryService, IConsultingQueryService consultingQueryService, ISettingQueryService settingQueryService, IRoomQueryService roomQueryService)
        {
            _commandService = commandService;
            _fetcher = fetcher;
            _favoriteFolderQueryService = favoriteFolderQueryService;
            _userQueryService = userQueryService;
            _projectQueryService = projectQueryService;
            _consultingQueryService = consultingQueryService;
            _settingQueryService = settingQueryService;
            _roomQueryService = roomQueryService;
        }

        public ActionResult Index(int page = 1, int pageSize = Web.Common.PaginationSetttings.PageSize, ConsultingQuery query = null)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.咨询接待管理, Permission.查看))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var viewModel = new IndexViewModel(Url)
            {
                Query = query,
                Items = _consultingQueryService.Query(page, pageSize, query),
                Sales = _userQueryService.GetTransmitUsers(WebAppContext.Current.User.Id).ToList(),
                FavoriteFolderSelectList = _favoriteFolderQueryService.GetFavoriteFoldersByUser(WebAppContext.Current.User.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                ProjectList = _projectQueryService.QueryAll().Where(x => x.Status == ProjectStatus.有效).Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
            };

            return View("~/Views/Sales/Consulting/Index.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult Edit(int id, int page = 1, int pageSize = 1, ConsultingQuery query = null)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.咨询接待管理, Permission.编辑))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var viewModel = PrepareConsulting(id);
            viewModel.HeaderText = "编辑";
            var consulting = _consultingQueryService.Query(page, pageSize, query);
            if (consulting.Data.Any())
            {
                viewModel.NextConsultingId = consulting.Data.First().ConsultingId;
                viewModel.Page = page;
            }

            return View("~/Views/Sales/Consulting/Create.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult Detail(int id, int page, int pageSize, ConsultingQuery query = null)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.咨询接待管理, Permission.查看))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var viewModel = PrepareConsulting(id);
            viewModel.HeaderText = "查看";

            var consulting = _consultingQueryService.Query(page, pageSize, query);
            if (consulting.Data.Any())
            {
                viewModel.NextConsultingId = consulting.Data.First().ConsultingId;
                viewModel.Page = page;
            }

            return View("~/Views/Sales/Consulting/Detail.cshtml", viewModel);
        }


        public ActionResult Create()
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.咨询接待管理, Permission.新增))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var viewModel = new CreateViewModel(Url)
            {
                ProjectList = _projectQueryService.QueryAll()
                    .Where(x => x.Status == ProjectStatus.有效)
                    .Select(x => new SelectListItem {Text = x.Name, Value = x.Id.ToString()}),
                FocusOnList = _settingQueryService.GetSettingsByType(SettingType.关注点)
                    .Select(x => new SelectListItem {Text = x.Name, Value = x.Name}),
                MoveInRequirementList = _settingQueryService.GetSettingsByType(SettingType.入住需求)
                    .Select(x => new SelectListItem {Text = x.Name, Value = x.Name}),
                HouseTypeList = new List<SelectListItem>(),
                HeaderText = "添加"
            };

            return View("~/Views/Sales/Consulting/Create.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult Create(CreateConsultingCommand command)
        {
            command.UserId = WebAppContext.Current.User.Id;
            command.CreatedBy = WebAppContext.Current.User.Username;

            var result = _commandService.ExecuteFoResult(command);
            return RedirectToAction("Edit", new {id = result.ConsultingId, page = 2, isEditNext = true});
        }

        [HttpPost]
        public ActionResult Edit(EditConsultingCommand command)
        {
            _commandService.Execute(command);

            if (Request != null && Request.Url != null)
            {
                return Redirect(Request.Url.ToString());
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult CheckIn(int id)
        {
            var viewModel = PrepareConsulting(id);
            viewModel.HeaderText = "入住";
            return View("~/Views/Sales/Consulting/Create.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult CheckIn(StartMoveInCommand command)
        {
            var result =  _commandService.ExecuteFoResult(command);
            return RedirectToAction("CustomerDetail", "Customer", new { customerId = result.CustomerId });
        }

        [HttpPost]
        public ActionResult TransmitConsultings(TransmitConsultingsCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult FavoriteConsultings(FavoriteConsultingsCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteConsultingTracking(DeleteConsultingTrackingCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Edit", new { id = command.ConsultingId });
        }

        [HttpGet]
        public ActionResult EditTracking(int id)
        {
            var consultingTracking = _fetcher.Get<Entities.ConsultingTracking>(id);
            var viewModel = new EditTrackingViewModel
            {
                ConsultingTrackingId = consultingTracking.Id,
                TrackingType = consultingTracking.TrackingType,
                Description = consultingTracking.Description,
                Status = consultingTracking.Status,
                StartTime = consultingTracking.StartTime,
                EndTime = consultingTracking.EndTime
            };
            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditConsultingTracking(int id)
        {
            var consultingTracking = _fetcher.Get<Entities.ConsultingTracking>(id);
            var viewModel = new EditTrackingViewModel
            {
                ConsultingTrackingId = consultingTracking.Id,
                TrackingType = consultingTracking.TrackingType,
                Description = consultingTracking.Description,
                Status = consultingTracking.Status,
                StartTime = consultingTracking.StartTime,
                EndTime = consultingTracking.EndTime,
                ConsultingId = consultingTracking.Consulting.Id,
                Pid = consultingTracking.Project.Id,
            };
            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditConsultingTracking(EditConsultingTrackingCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Edit", new { id = command.ConsultingId });
        }

        [HttpPost]
        public ActionResult CreateConsultingTracking(CreateConsultingTrackingCommand command)
        {
            command.UserId = WebAppContext.Current.User.Id;
            _commandService.Execute(command);
            return RedirectToAction("Edit", new { id = command.ConsultingId });
        }

        [HttpGet]
        public JsonResult GetHouseTypes(int projectId)
        {
            return Json(_roomQueryService.GetRoomHouseTypes(projectId).Select(x => new {HouseType = x}).ToArray(),
                JsonRequestBehavior.AllowGet);
        }

        private CreateViewModel PrepareConsulting(int id)
        {
            var consulting = _fetcher.Get<Entities.Consulting>(id);
            var consultingFamily = _consultingQueryService.GetConsultingFamily(id).First();

            var viewModel = new CreateViewModel(Url)
            {
                ConsultingId = consulting.Id,
                ConsultingName = consulting.ConsultingName,
                Items = _consultingQueryService.GetConsultingTrackingAll(consulting.Id),
                ConsultingFamilyId = consultingFamily.Id,
                Consulting = consulting.MapToEntity<Commands.Consulting>(),
                ConsultingFamily = consultingFamily.MapToEntity<ConsultingFamily>(),
                ProjectList = _projectQueryService.QueryAll()
                    .Where(x => x.Status == ProjectStatus.有效)
                    .Select(x => new SelectListItem {Text = x.Name, Value = x.Id.ToString()}),
                FocusOnList = _settingQueryService.GetSettingsByType(SettingType.关注点)
                    .Select(x => new SelectListItem {Text = x.Name, Value = x.Name}),
                MoveInRequirementList = _settingQueryService.GetSettingsByType(SettingType.入住需求)
                    .Select(x => new SelectListItem { Text = x.Name, Value = x.Name }),
                ConsultingAreaText = GetConsultingAreaText(consulting.Area),
                ConsultingFamilyAreaText = GetConsultingFamilyAreaText(consultingFamily.Area)
            };

            viewModel.Consulting.Level = consulting.Level.ToString();
            viewModel.Consulting.ProjectIds = consulting.ConsultingProjects.Select(x => x.Project.Id).ToList();
            viewModel.Consulting.Projects = consulting.ConsultingProjects.Select(x => x.Project.Name).ToList();
            viewModel.Consulting.FocusOns = consulting.FocusOn.SplitToList<string>(',');
            viewModel.Consulting.HouseTypes = consulting.HouseType.SplitToList<string>(',');
            viewModel.Consulting.MoveInRequirements = consulting.MoveInRequirement.SplitToList<string>(',');
            viewModel.HouseTypeList = _roomQueryService.GetRoomHouseTypes(viewModel.Consulting.ProjectIds.First())
                .Select(x => new SelectListItem {Text = x, Value = x});

            return viewModel;
        }

        [HttpPost]
        public void UpdateWow(UpdateConsultingJoinWowCommand command)
        {
            _commandService.Execute(command);
            RedirectToAction("Index");
        }

        public string ConsultingAreaText { get; set; }
        public string ConsultingFamilyAreaText { get; set; }

        private string GetConsultingAreaText(Area area)
        {
            if (area == null)
            {
                return ConsultingAreaText;
            }
            if (ConsultingAreaText.IsNullOrWhiteSpace())
            {
                ConsultingAreaText += $"{area.Name}";
            }
            else
            {
                ConsultingAreaText = ConsultingAreaText.Insert(0, $"{area.Name}/");
            }
            
            return GetConsultingAreaText(area.ParentArea);
        }

        private string GetConsultingFamilyAreaText(Area area)
        {
            if (area == null)
            {
                return ConsultingFamilyAreaText;
            }
            if (ConsultingFamilyAreaText.IsNullOrWhiteSpace())
            {
                ConsultingFamilyAreaText += $"{area.Name}";
            }
            else
            {
                ConsultingFamilyAreaText = ConsultingFamilyAreaText.Insert(0, $"{area.Name}/");
            }

            return GetConsultingFamilyAreaText(area.ParentArea);
        }
    }
}