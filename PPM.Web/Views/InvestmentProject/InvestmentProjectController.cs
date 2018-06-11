using System;
using System.Linq;
using System.Web.Mvc;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Query;
using PensionInsurance.Shared;
using PensionInsurance.Web.Common;
using PensionInsurance.Web.Views.Account;
using WebGrease.Css.Extensions;

namespace PensionInsurance.Web.Views.InvestmentProject
{
    public class InvestmentProjectController : AuthorizedController
    {
        private readonly ICommandService _commandService;
        private readonly IInvestmentProjectQueryService _investmentProjectQueryService;
        private readonly IAreaQueryService _areaQueryService;
        private readonly IInvestmentProjectLandService _investmentProjectLandService;
        private readonly IProjectQueryService _projectQueryService;
        public InvestmentProjectController(ICommandService commandService, IInvestmentProjectQueryService investmentProjectQueryService, IAreaQueryService areaQueryService, IInvestmentProjectLandService investmentProjectLandService, IProjectQueryService projectQueryService)
        {
            _commandService = commandService;
            _investmentProjectQueryService = investmentProjectQueryService;
            _areaQueryService = areaQueryService;
            _investmentProjectLandService = investmentProjectLandService;
            _projectQueryService = projectQueryService;
        }

        public ActionResult Index(int page = 1, int pageSize = Web.Common.PaginationSetttings.PageSize, InvestmentProjectQuery query = null)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.项目信息管理, Permission.查看))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var viewModel = new IndexViewModel(Url)
            {
                Query = query,
                Items = _investmentProjectQueryService.Query(page, pageSize, query),
                Types = Enum.GetNames(typeof(ProjectType))
                    .Select(x => new SelectListItem {Text = x, Value = x.ToString()})
                    .ToList(),
                Status = Enum.GetNames(typeof(InvestmentProjectStatus))
                    .Select(x => new SelectListItem { Text = x, Value = x.ToString() })
                    .ToList()
            };
            viewModel.Items.ForEach(m=>m.BuildingAcreageGround = m.Lands.Sum(n=>n.BuildingAcreageGround));
            viewModel.Items.ForEach(m => m.BuildingAcreageUnderGround = m.Lands.Sum(n => n.BuildingAcreageUnderGround));
            viewModel.Items.ForEach(m => m.BuildingAcreage = m.BuildingAcreageGround + m.BuildingAcreageUnderGround);

            return View("~/Views/InvestmentProject/Index.cshtml", viewModel);
        }
        
        [HttpGet]
        public ActionResult Create()
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.项目信息管理, Permission.新增))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var viewModel = new CreateViewModel
            {
                Cities = _areaQueryService.QueryCities()
                    .Select(x => new SelectListItem {Text = x.Name, Value = x.Id.ToString()}),
                Projects = _projectQueryService.QueryAll()
                    .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
            };

            return View("~/Views/InvestmentProject/Create.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult Create(CreateInvestmentProjectCommand command)
        {
            if (Request.Files.Count > 0)
            {
                command.FileBytes = Request.Files[0].ReadBytes();
                command.FileName = Request.Files[0].FileName;
            }

            _commandService.Execute(command);

            return RedirectToAction("Index");
        }
        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.项目信息管理, Permission.编辑))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            Entities.InvestmentProject investmentProject = _investmentProjectQueryService.Get(id);
            investmentProject.Lands.ForEach(m => m.BuildingAcreage =m.BuildingAcreageGround + m.BuildingAcreageUnderGround);
            var viewModel = new EditViewModel(Url)
            {
                InvestmentProjectId = investmentProject.Id,
                Name = investmentProject.Name,
                Address = investmentProject.Address,
                AreaId = investmentProject.Area.Id,
                InvestmentProjectType = investmentProject.InvestmentProjectType,
                Longitude = investmentProject.Longitude,
                Latitude = investmentProject.Latitude,
                Cities = _areaQueryService.QueryCities()
                    .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                Projects = _projectQueryService.QueryAll()
                    .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                FileName = investmentProject.GoogleEarthFileName,
                Status = investmentProject.Status,
                IncorporatedCompany = investmentProject.IncorporatedCompany,
                FilePath = investmentProject.GoogleEarthFileName,
                BuildingLandAcreage = investmentProject.Lands.Sum(m=>m.BuildingLandAcreage),
                BuildingAcreage = investmentProject.Lands.Sum(m=>m.BuildingAcreage),
                BuildingAcreageGround = investmentProject.Lands.Sum(m => m.BuildingAcreageGround),
                BuildingAcreageUnderGround = investmentProject.Lands.Sum(m => m.BuildingAcreageUnderGround),
                InvestmentProjectLands = _investmentProjectLandService.QueryAll().Where(m=>m.InvestmentProject.Id == id),
                BedAmount = investmentProject.BedAmount,
                RoomAmount = investmentProject.RoomAmount,
            };
            

            return View("~/Views/InvestmentProject/Edit.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult Edit(EditInvestmentProjectCommand command)
        {
            if (Request.Files.Count > 0)
            {
                command.FileBytes = Request.Files[0].ReadBytes();
                command.FileName = Request.Files[0].FileName;
            }

            _commandService.Execute(command);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Detail(int id)
        {
            Entities.InvestmentProject investmentProject = _investmentProjectQueryService.Get(id);

            var viewModel = new EditViewModel(Url)
            {
                InvestmentProjectId = investmentProject.Id,
                Name = investmentProject.Name,
                Address = investmentProject.Address,
                AreaId = investmentProject.Area.Id,
                InvestmentProjectType = investmentProject.InvestmentProjectType,
                Longitude = investmentProject.Longitude,
                Latitude = investmentProject.Latitude,
                AreaName = investmentProject.Area.Name,
                FileName = investmentProject.GoogleEarthFileName,
                FilePath = investmentProject.GoogleEarthFilePath,
                Status = investmentProject.Status,
                IncorporatedCompany = investmentProject.IncorporatedCompany,
                BuildingLandAcreage = investmentProject.Lands.Sum(m => m.BuildingLandAcreage),
                BuildingAcreage = investmentProject.Lands.Sum(m => m.BuildingAcreage),
                BuildingAcreageGround = investmentProject.Lands.Sum(m => m.BuildingAcreageGround),
                BuildingAcreageUnderGround = investmentProject.Lands.Sum(m => m.BuildingAcreageUnderGround),
                BusinessPattern = investmentProject.BusinessPattern,
                ObtainMethod = investmentProject.ObtainMethod,
                InvestmentProjectLands = _investmentProjectLandService.QueryAll().Where(m => m.InvestmentProject.Id == id),
            };
            return View("~/Views/InvestmentProject/Detail.cshtml",viewModel);
        }

        public JsonResult GetProjectBedAndRoomAmount(int id)
        {
            ProjectRoomBedAmount projectRoomBedAmount = new ProjectRoomBedAmount
            {
                BedAmount = _projectQueryService.GetProjectBedAmount(id),
                RoomAmount = _projectQueryService.GetProjectRoomAmount(id)
            };
            return Json(projectRoomBedAmount, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(DeleteInvestmentProjectCommand command)
        {
            _commandService.Execute(command);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Map(InvestmentProjectQuery query = null)
        {
            var viewModel = new MapViewModel
            {
                Query = query,
                Items = _investmentProjectQueryService.QueryAll(query),
                Types = Enum.GetNames(typeof(ProjectType))
                    .Select(x => new SelectListItem { Text = x, Value = x.ToString() })
                    .ToList(),
                Status = Enum.GetNames(typeof(InvestmentProjectStatus))
                    .Select(x => new SelectListItem { Text = x, Value = x.ToString() })
                    .ToList()
            };
            return View("~/Views/InvestmentProject/Map.cshtml", viewModel);
        }
    }
}
