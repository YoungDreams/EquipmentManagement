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
using System.Collections.Generic;
using Foundation.Data.Implemention;
using Foundation.Utils;

namespace PensionInsurance.Web.Views.Purchase.Product
{
    public class ProductController : AuthorizedController
    {
        private readonly ICommandService _commandService;
        private readonly IFetcher _fetcher;
        private readonly IProductCategoryQuery _productCategoryQuery;
        private readonly IPurchaseSupplierQuery _purchaseSupplierQuery;
        private readonly IAreaQueryService _areaQueryService;
        private readonly IProductQuery _productQuery;
        private readonly IPurchaseProductSupplierQuery _purchaseProductSupplierQuery;

        public ProductController(IFetcher fetcher, ICommandService commandService, IProductCategoryQuery productCategoryQuery, IProductQuery productQuery, IPurchaseSupplierQuery purchaseSupplierQuery, IAreaQueryService areaQueryService, IPurchaseProductSupplierQuery purchaseProductSupplierQuery)
        {
            _fetcher = fetcher;
            _commandService = commandService;
            _productCategoryQuery = productCategoryQuery;
            _productQuery = productQuery;
            _areaQueryService = areaQueryService;
            _purchaseProductSupplierQuery = purchaseProductSupplierQuery;
            _purchaseSupplierQuery = purchaseSupplierQuery;
        }

        // GET: Product
        public ActionResult Index(int page = 1, int pageSize = Web.Common.PaginationSetttings.PageSize, ProductQuery query = null)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.商品管理, Permission.查看))
            {
                return RedirectToAction("NoPermission", "Home");
            }
            var categories = _productCategoryQuery.QueryAllValid().ToList();
            var viewModel = new IndexViewModel(Url)
            {
                Query = query,
                Categories = categories.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                Suppliers = _purchaseSupplierQuery.QueryAll().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                Areas = _areaQueryService.QueryCities().Select(x=> new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                Items = _productQuery.Query(page, pageSize, query),
                ProductCategoryTreeView = new ProductCategoryTreeView().GetProductCategoryTreeView(categories),
                CategoryText = query.ProductCategoryId.HasValue
                    ? categories.SingleOrDefault(x => x.Id == query.ProductCategoryId).Name
                    : string.Empty
            };
            viewModel.PriceItems = viewModel.GroupBySupplierPrices(viewModel.Items);

            return View("~/Views/Purchase/Product/Index.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.商品管理, Permission.新增))
            {
                return RedirectToAction("NoPermission", "Home");
            }
            var categories = _productCategoryQuery.QueryAllValid().ToList();
            var viewModel = new CreateViewModel()
            {
                Sort = 99,
                Categories = categories.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                Cities = _areaQueryService.QueryCities().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                Suppliers = _purchaseSupplierQuery.QueryAll().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                ProductCategoryTreeView = new ProductCategoryTreeView().GetProductCategoryTreeView(categories),
            };
            viewModel.ProductCategoryTreeView.LastLayerOnly = true;
            return View("~/Views/Purchase/Product/Create.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult Create(CreateProductCommand command)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.商品管理, Permission.新增))
            {
                return RedirectToAction("NoPermission", "Home");
            }
            if (Request.Files.Count > 0)
            {
                command.FileBytes = Request.Files[0].ReadBytes();
                command.FileName = Request.Files[0].FileName;
            }
            var result = _commandService.ExecuteFoResult(command);
            return RedirectToAction("Edit", new { id = result.Id });
        }

        [HttpPost]
        public void GenerateQueryCode(GenerateProductQueryCodeCommand command)
        {
            _commandService.Execute(command);
        }

        [HttpGet]
        public ActionResult Edit(int id, string backUrl)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.商品管理, Permission.编辑))
            {
                return RedirectToAction("NoPermission", "Home");
            }
            var product = _fetcher.Get<Entities.PurchaseProduct>(id);
            var categories = _productCategoryQuery.QueryAllValid().ToList();
            var viewModel = new EditViewModel(Url)
            {
                Id = product.Id,
                Name = product.Name,
                Code = product.Code,
                Brand = product.Brand,
                ProductCategoryId = product.ProductCategory.Id,
                Published = product.Published,
                Sort = product.Sort,
                Specification = product.Specification,
                Description = product.Description,
                IsAsset = product.IsAsset,
                StockAlert = product.StockAlert,
                Unit = product.Unit,
                IsFood = product.IsFood,
                AssetClass = product.AssetClass,
                QueryCode = product.QueryCode,
                IsMateriel = product.IsMateriel,
                Categories =
                    _productCategoryQuery.QueryAllValid()
                        .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                Cities = _areaQueryService.QueryCities().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                Suppliers = _purchaseSupplierQuery.QueryAll().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                PriceItems = _purchaseProductSupplierQuery.QueryByProductId(product.Id),
                ProductCategoryTreeView = new ProductCategoryTreeView().GetProductCategoryTreeView(categories),
                CategoryText = categories.SingleOrDefault(x => x.Id == product.ProductCategory.Id).Name,
                BackUrl = backUrl,
            };
            viewModel.ProductCategoryTreeView.LastLayerOnly = true;
            if (product.ProductPictures.Count > 0 && product.ProductPictures.FirstOrDefault() != null)
            {
                viewModel.Path = product.ProductPictures.FirstOrDefault().OriginalPath;
            }
            return View("~/Views/Purchase/Product/Edit.cshtml", viewModel);
        }

        public ActionResult Edit(EditProductCommand command)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.商品管理, Permission.编辑))
            {
                return RedirectToAction("NoPermission", "Home");
            }
            if (Request.Files.Count > 0)
            {
                command.FileBytes = Request.Files[0].ReadBytes();
                command.FileName = Request.Files[0].FileName;
            }
            _commandService.Execute(command);

            return command.BackUrl.IsNullOrWhiteSpace() ? (ActionResult)RedirectToAction("Index") : Redirect(command.BackUrl);
        }

        [HttpGet]
        public ActionResult GetAreasJson(int id, int priceId = -1)
        {
            var supplierId = id;
            var supplier = _fetcher.Get<PurchaseSupplier>(supplierId);
            var price = _fetcher.Get<PurchaseProductSupplier>(priceId);
            var cities = _areaQueryService.QueryCities().ToList();
            var jsonModel = new List<dynamic>();
            if (!string.IsNullOrEmpty(supplier.AreaIds))
            {
                var aredIds = supplier.AreaIds.SplitToList<int>(',');
                foreach (var areaId in aredIds)
                {
                    var cityname = cities.Single(x => x.Id == areaId).Name;
                    jsonModel.Add(new { Text = cityname, Value = areaId, Selected = (areaId == price?.Area.Id) });
                }
            }
            return Json(jsonModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(DeleteProductCommand command)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.商品管理, Permission.删除))
            {
                return RedirectToAction("NoPermission", "Home");
            }
            _commandService.Execute(command);
            return command.ReturnUrl.IsNullOrWhiteSpace() ? (ActionResult)RedirectToAction("Index") : Redirect(command.ReturnUrl);
        }

        public ActionResult ExportBatchUpdatePrice(ExportBatchUpdatePriceCommand command)
        {
            var result = _commandService.ExecuteFoResult(command);

            return Json(new
            {
                success = result.IsSucceed,
                redirect = $"{Url.Content("~/Attachments/Report/")}{result.FileName}"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DownLoad(DownLoadProductPriceTemplateCommand command)
        {
            var result = _commandService.ExecuteFoResult(command);

            return Json(new
            {
                success = result.IsSucceed,
                redirect = $"{Url.Content("~/Attachments/Report/")}{result.FileName}"
            }, JsonRequestBehavior.AllowGet);


        }

        public ActionResult ExportProduct(ProductQuery query)
        { 
            var command = new ExportPurchaseProductCommand ();
            command.Name = query.Name;
            command.ProductCategoryId = query.ProductCategoryId;
            command.ProductSupplierId = query.ProductSupplierId;
            command.Published = query.Published;
            if (!WebAppContext.Current.User.HasPermission(ModuleType.商品管理, Permission.导出))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var result = _commandService.ExecuteFoResult(command);
            return Redirect("../"+ result.UrlPath);
        }

        [HttpGet]
        public ActionResult DownLoad()
        {
            var categories = _productCategoryQuery.QueryAllValid().ToList();
            var cities = _areaQueryService.QueryCities().ToList();
            var model = new DownLoadViewModel
            {
                Suppliers = _purchaseSupplierQuery.QueryAll().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                Cities = cities.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                Categories = categories.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                ProductCategoryTreeView = new ProductCategoryTreeView().GetProductCategoryTreeView(categories),
            };

            return View("~/Views/Purchase/Product/_ProductPrice.download.cshtml", model);
        }

        [HttpGet]
        public PartialViewResult ImportPrice()
        {
            return PartialView("~/Views/Purchase/Product/_ProductPrice.Import.cshtml");
        }

        [HttpPost]
        public void ImportPrice(ImportProductPriceCommand command)
        {
            if (Request.Files.Count > 0)
            {
                command.FilePath = Request.Files[0].ReadBytes();
                command.FileName = Request.Files[0].FileName;
            }
            _commandService.Execute(command);
        }

        [HttpGet]
        public PartialViewResult Import()
        {
            return PartialView("~/Views/Purchase/Product/_Product.Import.cshtml");
        }

        [HttpPost]
        public void Import(ImportProductCommand command)
        {
            if (Request.Files.Count > 0)
            {
                command.FilePath = Request.Files[0].ReadBytes();
                command.FileName = Request.Files[0].FileName;
            }
            _commandService.Execute(command);
        }

        public ActionResult GetProducts(string keyword)
        {
            var products = _fetcher.Query<PurchaseProduct>()
                .Where(x => x.QueryCode.StartsWith(keyword.Trim().ToUpper()) && x.QueryCode != null)
                .Select(s => new { s.Name, s.QueryCode }).Distinct().Take(10).ToList();
            return Json(products.Select(x => new
            {
                Name = x.Name + "-" + x.QueryCode
            }), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ImportBatchUpdatePrice(ImportBatchUpdatePriceCommand command)
        {
            if (Request.Files.Count > 0)
            {
                command.FilePath = Request.Files[0].ReadBytes();
                command.FileName = Request.Files[0].FileName;
            }
            var result = _commandService.ExecuteFoResult(command);
            return Json(new
            {
                success = result.IsSucceed,
                errors = result.Errors.ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        private ProductCategoryTreeView GetProductCategoryTreeView(List<PurchaseProductCategory> purchaseProductCategories)
        {

            var minLayer = purchaseProductCategories.Min(x => x.Layer);
            var maxLayer = purchaseProductCategories.Min(x => x.Layer);
            var layerLength = purchaseProductCategories.Select(x => x.Layer).Distinct().Count();
            var rootLayerTrees = RetrieveRootTreeNodesByParentId(minLayer, purchaseProductCategories);
            var categoryTreeView = new ProductCategoryTreeView
            {
                Trees = new List<TreeNode>(),
            };
            foreach (var category in rootLayerTrees)
            {
                category.SubTreeNodes = new List<TreeNode>();
                category.SubTreeNodes = RetrieveSubTreeNodes(int.Parse(category.Value), purchaseProductCategories);
                categoryTreeView.Trees.Add(category);
            }


            return categoryTreeView;
        }

        public List<TreeNode> RetrieveRootTreeNodesByParentId(int layer, List<PurchaseProductCategory> purchaseProductCategories)
        {
            var subNodes = purchaseProductCategories.Where(x => x.Layer == layer).Select(x => new TreeNode
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                ParentId = x.ParentId.Value,
                Layer = x.Layer,
                HasSubTreeNodes = purchaseProductCategories.Any(z => z.ParentId == x.Id),
            }).ToList();
            return subNodes;
        }

        public List<TreeNode> RetrieveSubTreeNodes(int parentId, List<PurchaseProductCategory> purchaseProductCategories)
        {
            var subNodes = purchaseProductCategories.Where(x => x.ParentId == parentId).Select(x => new TreeNode
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Layer = x.Layer,
                HasSubTreeNodes = purchaseProductCategories.Any(z => z.ParentId == x.Id),
                SubTreeNodes = purchaseProductCategories.Any(z => z.ParentId == x.Id) ? RetrieveSubTreeNodes(x.Id, purchaseProductCategories) : new List<TreeNode>()
            }).ToList();
            return subNodes;
        }
    }
}