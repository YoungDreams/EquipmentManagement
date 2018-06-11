using System;
using System.Linq;
using System.Web.Mvc;
using Foundation.Data;
using Foundation.Messaging;
using PPM.Commands;
using PPM.Entities;
using PPM.MVC.Views.Account;
using PPM.Query;

namespace PPM.MVC.Views.Equipment.Category
{
    public class EquipmentCategoryController : AuthorizedController
    {
        private readonly ICommandService _commandService;
        private readonly IEquipmentCategoryQueryService _categoryQuery;
        private readonly IFetcher _fetcher;

        public EquipmentCategoryController(ICommandService commandService, IEquipmentCategoryQueryService userQuery, IFetcher fetcher)
        {
            _commandService = commandService;
            _categoryQuery = userQuery;
            _fetcher = fetcher;
        }

        public ActionResult Index(EquipmentCategoryQuery query = null)
        {
            var viewModel = new IndexViewModel(Url)
            {
                Query = query,
                Items = _categoryQuery.Query(query)
            };
            return View("~/Views/Equipment/Category/Index.cshtml", viewModel);
        }

        public ActionResult Create()
        {
            var categories = _categoryQuery.QueryAllValid().ToList();
            var viewModel = new CreateViewModel
            {
                Categories = categories
                    .Where(x => x.ParentId == 0)
                    .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                ProductCategoryTreeView = new EquipmentCategoryTreeView().GetProductCategoryTreeView(categories),
            };
            return View("~/Views/Equipment/Category/Create.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult GetFancyTreeViewData()
        {
            var categories = _categoryQuery.QueryAllValid().ToList();
            var treeView = new FancyTreeNodeView(categories);
            return Json(treeView.Trees, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(CreateEquipmentCategoryCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var productCategory = _categoryQuery.Get(id);
            var categories = _categoryQuery.QueryAllValid().ToList();
            var treeView = new EquipmentCategoryTreeView(categories);

            var viewModel = new EditViewModel(Url)
            {
                Id = productCategory.Id,
                Name = productCategory.Name,
                Description = productCategory.Description,
                ParentId = productCategory.ParentId,
                Published = productCategory.Published,
                Categories = categories.Where(x => x.Id != productCategory.Id && x.ParentId == 0).Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                ColumnTypes = Enum.GetNames(typeof(EquipmentCategoryColumnType))
                    .Select(x => new SelectListItem { Text = x, Value = x.ToString() })
                    .ToList(),
                ProductCategoryTreeView = treeView.GetProductCategoryTreeView(),
                Columns = productCategory.Columns
            };
            viewModel.ProductCategoryTreeView.CategoryText = categories.SingleOrDefault(x => x.Id == productCategory.ParentId)?.Name;

            return View("~/Views/Equipment/Category/Edit.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult EditColumn(int id)
        {
            var column = _fetcher.Get<EquipmentCategoryColumn>(id);
            var viewModel = new EditEquipmentCategoryColumnViewModel
            {
                Id = column.Id,
                ColumnName = column.ColumnName,
                ColumnType = column.ColumnType
            };
            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditColumn(EditEquipmentCategoryColumnCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Edit");
        }

        [HttpPost]
        public ActionResult CreateColumns(SetEquipmentCategoryColumnsCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Edit");
        }

        [HttpPost]
        public ActionResult DeleteColumn(DeleteEquipmentCategoryColumnCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Edit");
        }

        [HttpPost]
        public ActionResult Edit(EditEquipmentCategoryCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(DeleteUserCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Export(ExportEquipmentCategoryColumnTemplateCommand command)
        {
            _commandService.ExecuteFoResult(command);
            var result = _commandService.ExecuteFoResult(command);
            return Redirect("../" + result.UrlPath);
        }
    }
}
