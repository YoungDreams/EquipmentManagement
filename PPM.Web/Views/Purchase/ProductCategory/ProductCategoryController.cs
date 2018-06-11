using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Foundation.Data;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Query;
using PensionInsurance.Web.Views.Account;
using Foundation.Core;
using PensionInsurance.Entities;
using PensionInsurance.Shared;
using PensionInsurance.Web.Common;

namespace PensionInsurance.Web.Views.Purchase.ProductCategory
{
    public class ProductCategoryController : AuthorizedController
    {
        private readonly ICommandService _commandService;
        private readonly IFetcher _fetcher;
        private readonly IProductCategoryQuery _productCategoryQuery;
        private readonly IProjectQueryService _projectQueryService;
        private readonly IDepartmentQueryService _departmentQueryService;

        public ProductCategoryController(ICommandService commandService, IFetcher fetcher, IProductCategoryQuery productCategoryQuery, IProjectQueryService projectQueryService, IDepartmentQueryService departmentQueryService)
        {
            _commandService = commandService;
            _fetcher = fetcher;
            _productCategoryQuery = productCategoryQuery;
            _projectQueryService = projectQueryService;
            _departmentQueryService = departmentQueryService;
        }

        public ActionResult Index(int page = 1, int pageSize = Web.Common.PaginationSetttings.PageSize, ProductCategoryQuery query = null)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.商品分类管理, Permission.查看))
            {
                return RedirectToAction("NoPermission", "Home");
            }
            var categories = _productCategoryQuery.QueryAllValid().ToList();
            var viewModel = new IndexViewModel(Url)
            {
                Query = query,
                FancyTreeNodeView = new FancyTreeNodeView(categories),
                Items = _productCategoryQuery.Query(page, pageSize, query)
            };

            return View("~/Views/Purchase/ProductCategory/Index.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult GetFancyTreeViewData()
        {
            var categories = _productCategoryQuery.QueryAllValid().ToList();
            var treeView = new FancyTreeNodeView(categories);
            return Json(treeView.Trees, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.商品分类管理, Permission.新增))
            {
                return RedirectToAction("NoPermission", "Home");
            }
            var categories = _productCategoryQuery.QueryAllValid().ToList();
            var viewModel = new CreateViewModel
            {
                Sort = 99,
                Categories = categories
                    .Where(x => x.ParentId == 0)
                    .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                Projects = _projectQueryService.QueryAllValid()
                    .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                Departments = _departmentQueryService.GetDepartments().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                ProductCategoryTreeView = new ProductCategoryTreeView().GetProductCategoryTreeView(categories),
            };

            return View("~/Views/Purchase/ProductCategory/Create.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult Create(CreatePurchaseProductCategoryCommand command)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.商品分类管理, Permission.查看))
            {
                return RedirectToAction("NoPermission", "Home");
            }
            _commandService.Execute(command);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.商品分类管理, Permission.编辑))
            {
                return RedirectToAction("NoPermission", "Home");
            }
            var productCategory = _productCategoryQuery.Get(id);
            var categories = _productCategoryQuery.QueryAllValid().ToList();
            var treeView = new ProductCategoryTreeView(categories);
            
            var viewModel = new EditViewModel
            {
                Id = productCategory.Id,
                Name = productCategory.Name,
                Description = productCategory.Description,
                Sort = productCategory.Sort,
                ParentId = productCategory.ParentId,
                Published = productCategory.Published,
                IsAssetOrLongConsumption = productCategory.IsAssetOrLongConsumption,
                ProductCategoryType = productCategory.ProductCategoryType,
                //Department = productCategory.Departments?.Id,
                DepartmentIds = productCategory.Departments?.Split(',').ToList(),
                Categories = categories.Where(x => x.Id != productCategory.Id && x.ParentId == 0).Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                Projects = _projectQueryService.QueryAllValid()
                    .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                Departments = _departmentQueryService.GetDepartments().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                ProductCategoryTreeView = treeView.GetProductCategoryTreeView(),
                Code = productCategory.Code
            };
            viewModel.ProductCategoryTreeView.CategoryText = categories.SingleOrDefault(x => x.Id == productCategory.ParentId)?.Name;

            return View("~/Views/Purchase/ProductCategory/Edit.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult Edit(EditPurchaseProductCategoryCommand command)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.商品分类管理, Permission.编辑))
            {
                return RedirectToAction("NoPermission", "Home");
            }
            _commandService.Execute(command);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(DeleteProductCategoryCommand command)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.商品分类管理, Permission.删除))
            {
                return RedirectToAction("NoPermission", "Home");
            }
            _commandService.Execute(command);
            return command.ReturnUrl.IsNullOrWhiteSpace() ? (ActionResult)RedirectToAction("Index") : Redirect(command.ReturnUrl);
        }

        [HttpGet]
        public PartialViewResult Import()
        {
            return PartialView("~/Views/Purchase/ProductCategory/_ProductCategory.Import.cshtml");
        }

        [HttpPost]
        public void Import(ImportProductCategoryCommand command)
        {
            if (Request.Files.Count > 0)
            {
                command.FilePath = Request.Files[0].ReadBytes();
                command.FileName = Request.Files[0].FileName;
            }
            _commandService.Execute(command);
        }
    }
}