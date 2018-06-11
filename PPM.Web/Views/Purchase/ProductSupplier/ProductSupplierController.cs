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

namespace PensionInsurance.Web.Views.Purchase.ProductSupplier
{
    public class ProductSupplierController : AuthorizedController
    {
        private readonly ICommandService _commandService;
        private readonly IFetcher _fetcher;
        private readonly IPurchaseProductSupplierQuery _purchaseProductSupplierQuery;
        private readonly IPurchaseProductQuery _purchaseProductQuery;
        private readonly IPurchaseSupplierQuery _purchaseSupplierQuery;        

        public ProductSupplierController(IFetcher fetcher, ICommandService commandService, IPurchaseProductSupplierQuery purchaseProductSupplierQuery, IPurchaseProductQuery purchaseProductQuery, IPurchaseSupplierQuery purchaseSupplierQuery)
        {
            _fetcher = fetcher;
            _commandService = commandService;
            _purchaseProductSupplierQuery = purchaseProductSupplierQuery;
            _purchaseProductQuery = purchaseProductQuery;
            _purchaseSupplierQuery = purchaseSupplierQuery;
        }

        // GET: ProductSupplier
        public ActionResult Index(int page = 1, int pageSize = PaginationSetttings.PageSize, PurchaseProductSupplierQuery query = null)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.供应商价格管理, Permission.查看))
            {
                return RedirectToAction("NoPermission", "Home");
            }
            var viewModel = new IndexViewModel(Url)
            {
                Query = query,
                PurchaseProducts = _purchaseProductQuery.QueryAll().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                PurchaseSuppliers = _purchaseSupplierQuery.QueryAll().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                Items = _purchaseProductSupplierQuery.Query(page, pageSize, query)
            };
            return View("~/Views/Purchase/ProductSupplier/Index.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.供应商价格管理, Permission.新增))
            {
                return RedirectToAction("NoPermission", "Home");
            }
            var viewModel = new CreateViewModel()
            {
                PurchaseProducts = _purchaseProductQuery.QueryAll().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                PurchaseSuppliers = _purchaseSupplierQuery.QueryAll().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
            };
            return View("~/Views/Purchase/ProductSupplier/Create.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult Create(CreatePurchaseProductSupplierCommand command)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.供应商价格管理, Permission.新增))
            {
                return RedirectToAction("NoPermission", "Home");
            }
            _commandService.Execute(command);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult CreateMaterielPrice(CreatePurchaseProductSupplierMaterielCommand command)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.供应商价格管理, Permission.新增))
            {
                return RedirectToAction("NoPermission", "Home");
            }
            _commandService.Execute(command);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.供应商价格管理, Permission.编辑))
            {
                return RedirectToAction("NoPermission", "Home");
            }
            var productSupplier = _fetcher.Get<Entities.PurchaseProductSupplier>(id);
            var viewModel = new EditViewModel()
            {
                Id = productSupplier.Id,
                Price = productSupplier.Price,
                Max = productSupplier.Max,
                Min = productSupplier.Min,
                AreaId = productSupplier.Area.Id,
                PurchaseProductId = productSupplier.PurchaseProduct.Id,
                PurchaseSupplierId = productSupplier.PurchaseSupplier.Id,
                PurchaseSuppliers = _purchaseSupplierQuery.QueryAll().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                IsValid = productSupplier.IsValid?"1":"0"
            };
            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Edit(EditPurchaseProductSupplierCommand command)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.供应商价格管理, Permission.编辑))
            {
                return RedirectToAction("NoPermission", "Home");
            }
            _commandService.Execute(command);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult EditMaterielPrice(EditPurchaseProductSupplierMaterielPriceCommand command)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.供应商价格管理, Permission.编辑))
            {
                return RedirectToAction("NoPermission", "Home");
            }
            _commandService.Execute(command);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(DeletePurchaseProductSupplierCommand command)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.供应商价格管理, Permission.删除))
            {
                return RedirectToAction("NoPermission", "Home");
            }
            _commandService.Execute(command);
            return command.ReturnUrl.IsNullOrWhiteSpace() ? (ActionResult)RedirectToAction("Index") : Redirect(command.ReturnUrl);
        }
    }
}