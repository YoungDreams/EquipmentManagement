using System.Linq;
using System.Web.Mvc;
using Foundation.Data;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Query;
using PensionInsurance.Web.Common;
using PensionInsurance.Web.Views.Account;
using PensionInsurance.Entities;
using PensionInsurance.Shared;

namespace PensionInsurance.Web.Views.Purchase.Collecting
{
    public class CollectingController : AuthorizedController
    {
        private readonly ICommandService _commandService;
        private readonly IPurchaseProductStockQuery _purchaseProductStockQuery;
        private readonly ICollectingProductQuery _collectingProductQuery;
        private readonly IProjectQueryService _projectQueryService;
        private readonly IProductCategoryQuery _productCategoryQuery;
        private readonly IPurchaseSupplierQuery _purchaseSupplierQuery;

        public CollectingController(IFetcher fetcher, ICommandService commandService, IProjectQueryService projectQueryService, IPurchaseProductStockQuery purchaseProductStockQuery, ICollectingProductQuery collectingProductQuery, IProductCategoryQuery productCategoryQuery, IPurchaseSupplierQuery purchaseSupplierQuery)
        {
            _commandService = commandService;
            _projectQueryService = projectQueryService;
            _purchaseProductStockQuery = purchaseProductStockQuery;
            _collectingProductQuery = collectingProductQuery;
            _productCategoryQuery = productCategoryQuery;
            _purchaseSupplierQuery = purchaseSupplierQuery;
        }

        public ActionResult CollectingProductStock(CollectingProductQuery query)
        {
            var user = WebAppContext.Current.User;
            if (IsManagement() && WebAppContext.Current.User.HasPermission(ModuleType.出库, Permission.查看))
            {
                if (!user.HasPermission(ModuleType.出库, Permission.查看))
                {
                    return RedirectToAction("NoPermission", "Home");
                }
                query.IsFinish = false;
                var project = _projectQueryService.Get(query.ProjectId);
                query.ProjectName = project.Name;
                var viewModel = new CollectingProductViewModel(Url)
                {
                    Query = query,
                    Items = _collectingProductQuery.Query(query, 1, 100),
                    DepartmentName = user.Department?.Name,
                    UserId = user.Id,
                    UserName = user.RealName
                };
                return View("~/Views/Purchase/Collecting/CollectingProductStock.cshtml", viewModel);
            }
            return RedirectToAction("NoPermission", "Home");
        }

        public ActionResult CollectingProduct(CollectingProductQuery query, int page = 1, int pageSize = PaginationSetttings.PageSize)
        {
            if (!IsManagement()&&!WebAppContext.Current.User.HasPermission(ModuleType.出库, Permission.查看))
            {
                return RedirectToAction("NoPermission", "Home");
            }
            query.IsFinish = true;
            var project = _projectQueryService.Get(query.ProjectId);
            query.ProjectName = project.Name;
            var viewModel = new CollectingProductViewModel(Url)
            {
                Query = query,
                Items = _collectingProductQuery.Query(query, page, pageSize)
            };
            return View("~/Views/Purchase/Collecting/CollectingProduct.cshtml", viewModel);
        }

        public ActionResult Index()
        {
            //if (!IsManagementInDepartment() && !WebAppContext.Current.User.HasPermission(ModuleType.出库, Permission.查看))
            //{
            //    return RedirectToAction("NoPermission", "Home");
            //}
            var currentUserProjectCount = WebAppContext.Current.User.Projects.Count;
            if (currentUserProjectCount == 1)
            {
                var projectId = WebAppContext.Current.User.Projects.First().Id;
                return RedirectToAction("CollectingProductStock", new PurchaseProductStockQuery { ProjectId = projectId });
            }
            var projects = _projectQueryService.QueryAllValid();
            var currentUserProejctId = WebAppContext.Current.User.Projects?.FirstOrDefault()?.Id;
            var viewModel = new SelectViewModel
            {
                ProjectId = currentUserProejctId??0,
                ProjectList = projects.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).ToList()
            };
            return PartialView("~/Views/purchase/Collecting/_Collecting.SelectProject.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult SelectProductToCollect(PurchaseProductStockQuery query)
        {
            var categories = _productCategoryQuery.QueryAllValid().ToList();
            var parentCategories = _productCategoryQuery.QueryAllValid().Where(x => x.ParentId == 0 && x.Layer == 1).ToList();
            var project = _projectQueryService.Get(query.ProjectId);
            query.ProjectName = project.Name;
            var viewModel = new CollectingProductStockViewModel(Url)
            {
                Query = query,
                Suppliers = _purchaseSupplierQuery.QueryAll().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).ToList(),
                Items = _purchaseProductStockQuery.Query(1, 100, query, false),
                ProductCategoryTreeView = new ProductCategoryTreeView().GetProductCategoryTreeView(categories),
                CategoryText = query.ProductCategoryId.HasValue
                    ? categories.SingleOrDefault(x => x.Id == query.ProductCategoryId).Name
                    : string.Empty
            };
            return View("~/Views/Purchase/Collecting/SelectCollecting.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult SelectProductToCollect(CreateSelectMoveOutStockCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("CollectingProductStock", new PurchaseProductStockQuery { ProjectId = command.ProjectId });
        }

        [HttpPost]
        public ActionResult Select(PurchaseSelectProjectCommand command)
        {
            //_commandService.ExecuteFoResult(command);
            return RedirectToAction("CollectingProductStock", new PurchaseProductStockQuery { ProjectId = command.ProjectId.Value });
        }

        [HttpPost]
        public ActionResult SubmitCollectingProducts(SubmitCollectingProjectProductCommand command)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.出库, Permission.新增))
            {
                RedirectToAction("NoPermission", "Home");
            }
            _commandService.Execute(command);
            return RedirectToAction("CollectingProduct", new CollectingProductQuery { ProjectId = command.ProjectId });
        }

        [HttpPost]
        public ActionResult DeleteCollectingProduct(DeleteCollectingProductCommand command)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.出库, Permission.删除))
            {
                return RedirectToAction("NoPermission", "Home");
            }
            _commandService.Execute(command);
            return Redirect(command.ReturnUrl);
        }

        [HttpPost]
        public ActionResult UploadConfirmPaper(UploadCollectingConfirmPaperCommand command)
        {
            if (Request.Files.Count > 0)
            {
                command.FileBytes = Request.Files[0].ReadBytes();
                command.FileName = Request.Files[0].FileName;
            }
            var result = _commandService.ExecuteFoResult(command);
            return Json(result);
        }

        private static bool IsManagement()
        {
            var user = WebAppContext.Current.User;
            if (user.Roles != null && user.Roles.Any(x => x.RoleType == RoleType.采购管理员))
            {
                return true;
            }
            return false;
        }
    }
}