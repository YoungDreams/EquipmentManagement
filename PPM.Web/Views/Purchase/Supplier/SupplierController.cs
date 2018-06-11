using System.Linq;
using System.Web.Mvc;
using Foundation.Data;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Query;
using PensionInsurance.Web.Common;
using PensionInsurance.Web.Views.Account;
using Foundation.Core;
using PensionInsurance.Entities;
using PensionInsurance.Shared;
using Foundation.Data.Implemention;
using System.Collections.Generic;

namespace PensionInsurance.Web.Views.Purchase.Supplier
{
    public class SupplierController : AuthorizedController
    {
        private readonly ICommandService _commandService;
        private readonly IFetcher _fetcher;
        private readonly IAreaQueryService _areaQueryService;
        private readonly IPurchaseSupplierQuery _purchaseSupplierQuery;
        private readonly IProjectQueryService _projectQueryService;

        public SupplierController(IFetcher fetcher, ICommandService commandService, IAreaQueryService areaQueryService, IPurchaseSupplierQuery purchaseSupplierQuery, IProjectQueryService projectQueryService)
        {
            _fetcher = fetcher;
            _commandService = commandService;
            _areaQueryService = areaQueryService;
            _purchaseSupplierQuery = purchaseSupplierQuery;
            _projectQueryService = projectQueryService;
        }

        // GET: Supplier
        public ActionResult Index(int page = 1, int pageSize = PaginationSetttings.PageSize, PurchaseSupplierQuery query = null)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.供应商管理, Permission.查看))
            {
                return RedirectToAction("NoPermission", "Home");
            }
            var purchaseSupplier = _purchaseSupplierQuery.Query(page, pageSize, query).ToList();
            var cities = _areaQueryService.QueryCities().ToList();
            var projects = _projectQueryService.QueryAllValid().ToList();
            var purchaseSupplierViewData = purchaseSupplier.Convert();
            purchaseSupplierViewData.SetAreaNamesProjectNames(cities, projects);
            //projects = GetProject(query.AreaIds, projects);
            var viewModel = new IndexViewModel(Url)
            {
                Query = query,
                Cities = cities.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                Projects = projects.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                Items = purchaseSupplierViewData.ToPagedData(page, pageSize)
            };
            return View("~/Views/Purchase/Supplier/Index.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.供应商管理, Permission.新增))
            {
                return RedirectToAction("NoPermission", "Home");
            }
            var projects = _projectQueryService.QueryAllValid().ToList();
            var cities = _areaQueryService.QueryCities().ToList();
            var viewModel = new CreateViewModel()
            {
                Cities = cities.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                Projects = projects.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
            };
            return View("~/Views/Purchase/Supplier/Create.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult Create(CreatePurchaseSupplierCommand command)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.供应商管理, Permission.新增))
            {
                return RedirectToAction("NoPermission", "Home");
            }
            _commandService.Execute(command);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.供应商管理, Permission.编辑))
            {
                return RedirectToAction("NoPermission", "Home");
            }
            var productSupplier = _fetcher.Get<Entities.PurchaseSupplier>(id);
            var projects = _projectQueryService.QueryAllValid().ToList();
            var cities = _areaQueryService.QueryCities().ToList();
            var viewModel = new EditViewModel()
            {
                Id = productSupplier.Id,
                Name = productSupplier.Name,
                Projects = projects.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                Cities = cities.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                ProjectIds = productSupplier.ProjectIds?.Split(',').ToList(),
                AreaIds = productSupplier.AreaIds?.Split(',').ToList(),
                BankAccount = productSupplier.BankAccount,
                BankName = productSupplier.BankName,
                ContactPerson = productSupplier.ContactPerson,
                ContactPhone = productSupplier.ContactPhone,
                Email = productSupplier.Email,
                Note = productSupplier.Note,
                MinOrderMoney = productSupplier.MinOrderMoney,
                Status = productSupplier.Status
            };
            return View("~/Views/Purchase/Supplier/Edit.cshtml", viewModel);
        }

        public ActionResult Edit(EditPurchaseSupplierCommand command)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.供应商管理, Permission.编辑))
            {
                return RedirectToAction("NoPermission", "Home");
            }
            _commandService.Execute(command);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(DeletePurchaseSupplierCommand command)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.供应商管理, Permission.删除))
            {
                return RedirectToAction("NoPermission", "Home");
            }
            _commandService.Execute(command);
            return command.ReturnUrl.IsNullOrWhiteSpace() ? (ActionResult)RedirectToAction("Index") : Redirect(command.ReturnUrl);
        }

        [HttpGet]
        public ActionResult GetProjectsJson(string cityid, string projectid)
        {
            var projects = _projectQueryService.QueryAll().ToList();
            var jsonModel = new List<dynamic>();
            var ids = cityid == "null" ? new string[0] : cityid?.Split(',');
            var projectids = projectid == "null" ? new string[0] : projectid?.Split(',');
            if (ids.Length > 0)
            {
                foreach (var id in ids)
                {
                    foreach (var project in projects)
                    {
                        if (project.City != null && project.City.Id == int.Parse(id))
                        {
                            jsonModel.Add(new { Text = project.Name, Value = project.Id, Selected = projectids?.Contains(project.Id.ToString()) });
                        }
                    }
                }
            }
            else
            {
                foreach (var project in projects)
                {
                    jsonModel.Add(new { Text = project.Name, Value = project.Id, Selected = projectids?.Contains(project.Id.ToString()) });
                }
            }
            //foreach (var id in projectids)
            //{
            //    var project = jsonModel.FirstOrDefault(x => x.Value == id);
            //    if (project!=null)
            //    {
            //        project.Selected = true;
            //    }
            //}
            return Json(jsonModel.Distinct(), JsonRequestBehavior.AllowGet);
        }

        private List<Project> GetProject(List<string> cityIds, List<Project> projects)
        {
            var rlt = new List<Project>();
            if (cityIds!=null&&cityIds.Count > 0)
            {
                foreach (var cityid in cityIds)
                {
                    foreach (var project in projects)
                    {
                        if (project.City != null && project.City.Id == int.Parse(cityid))
                        {
                            rlt.Add(project);
                        }
                    }
                }
                return rlt;
            }
            else
            {
                return projects;
            }
            
        }
    }
}