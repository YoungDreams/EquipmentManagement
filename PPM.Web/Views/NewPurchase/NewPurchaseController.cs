using System;
using System.Linq;
using System.Web.Mvc;
using Foundation.Core;
using Foundation.Data;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Query;
using PensionInsurance.Shared;
using PensionInsurance.Web.Common;
using PensionInsurance.Web.Views.Account;
using PensionInsurance.Web.Views.Purchase;

namespace PensionInsurance.Web.Views.NewPurchase
{
    public class NewPurchaseController : AuthorizedController
    {
        private readonly IFetcher _fetcher;
        private readonly IProjectQueryService _projectQueryService;
        private readonly ICommandService _commandService;
        private readonly IProductCategoryQuery _productCategoryQuery;
        private readonly IProductQuery _productQuery;
        private readonly IPurchaseProductStockQuery _purchaseProductStockQuery;
        private readonly IPurchaseSupplierQuery _purchaseSupplierQuery;
        private readonly IPurchaseStatisticsQueryService _purchaseStatisticsQueryService;
        private readonly IDepartmentQueryService _departmentQueryService;

        public NewPurchaseController(IProjectQueryService projectQueryService, ICommandService commandService, IProductCategoryQuery productCategoryQuery, IProductQuery productQuery, IPurchaseProductStockQuery purchaseProductStockQuery, IPurchaseSupplierQuery purchaseSupplierQuery, IFetcher fetcher, IPurchaseStatisticsQueryService purchaseStatisticsQueryService, IDepartmentQueryService departmentQueryService)
        {
            _projectQueryService = projectQueryService;
            _commandService = commandService;
            _productCategoryQuery = productCategoryQuery;
            _productQuery = productQuery;
            _purchaseProductStockQuery = purchaseProductStockQuery;
            _purchaseSupplierQuery = purchaseSupplierQuery;
            _fetcher = fetcher;
            _purchaseStatisticsQueryService = purchaseStatisticsQueryService;
            _departmentQueryService = departmentQueryService;
        }

        [HttpGet]
        public ActionResult Select()
        {
            var viewModel = new PurchaseSelectViewModel
            {
                ProjectList = _projectQueryService.QueryAllValidByProjectFilter().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            return PartialView("~/Views/Newpurchase/_NewPurchase.Select.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult Select(PurchaseSelectProjectCommand command)
        {
            var result = _commandService.ExecuteFoResult(command);
            return RedirectToAction("Edit", "Purchaseshoppingcart", new { id = result.CartId });
        }

        [HttpPost]
        public ActionResult CreateCartItem(CreatePurchaseShoppingCartItemCommand command)
        {
            var result = _commandService.ExecuteFoResult(command);
            if (!string.IsNullOrWhiteSpace(command.ReturnUrl))
            {
                return Redirect(command.ReturnUrl);
            }
            return RedirectToAction("Edit", "Purchaseshoppingcart", new { id = result.CartId });
        }

        /// <summary>
        /// 提交采购产品
        /// </summary>
        /// <param name="pruductSupplierPriceId"></param>
        /// <param name="cartId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public JsonResult AddCart(int pruductSupplierPriceId, int cartId, int quantity)
        {
            var purchaseProductSupplier = _fetcher.Get<PurchaseProductSupplier>(pruductSupplierPriceId);
            var queyitem = _fetcher.Query<PurchaseShoppingCartItem>(
            ).FirstOrDefault(x => x.PurchaseShoppingCart.Id == cartId &&
                                  x.PurchaseProductSupplier.Id == purchaseProductSupplier.Id);

            if (queyitem != null)
            {
                if (purchaseProductSupplier.Max != 0 && queyitem.PurchaseQuantity + quantity > purchaseProductSupplier.Max)
                {
                    return Json(new { success = false, msg = "加入购物车数量超过价格区间上限，不允许增加" }, JsonRequestBehavior.AllowGet);
                }
            }

            var command = new AddCartCommand
            {
                PruductSupplierPriceId = pruductSupplierPriceId,
                CartId = cartId,
                Quantity = quantity
            };
            _commandService.Execute(command);
            var cart = _fetcher.Get<PurchaseShoppingCart>(command.CartId);
            var totalAmount = cart.CartItems.Sum(s => s.PurchaseQuantity * s.PurchaseProductSupplier.Price);

            var cartItems = cart.CartItems.Select(item => new CartItem
            {
                Id = item.PurchaseProductSupplier.Id,
                Name = item.PurchaseProductSupplier.PurchaseProduct.Name,
                Quantity = item.PurchaseQuantity,
                Price = item.PurchaseProductSupplier.Price
            }).ToList();

            return Json(new { success = true, TotalAmount = totalAmount, Items = cartItems, msg = "添加成功" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProducts(string keyword)
        {
            var products = _productQuery.GetProductsByKeyWords(keyword).ToList();
            if (products.Any())
            {
                return Json(products.Select(x => new
                {
                    Name = x.Name + "-" + x.QueryCode
                }), JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Delete(int itemId, int cartId)
        {
            var command = new DeletePurchaseShoppingCartItemCommand
            {
                ItemId = itemId,
                CartId = cartId,
            };
            _commandService.Execute(command);

            var cart = _fetcher.Get<PurchaseShoppingCart>(command.CartId);
            var totalQuantity = cart.CartItems.Sum(s => s.PurchaseQuantity) + cart.OtherCartItems.Sum(s => s.PurchaseQuantity);
            var totalAmount = cart.CartItems.Sum(s => s.PurchaseQuantity * s.PurchaseProductSupplier.Price) + cart.OtherCartItems.Sum(s => s.PurchaseQuantity * s.PurchasePrice);
            return Json(new { success = true, TotalQuantity = totalQuantity, TotalAmount = totalAmount, msg = "删除成功" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteOther(int itemId, int cartId)
        {
            var command = new DeletePurchaseShoppingCartOtherItemCommand
            {
                ItemId = itemId,
                CartId = cartId,
            };
            _commandService.Execute(command);
            var cart = _fetcher.Get<PurchaseShoppingCart>(command.CartId);
            var totalQuantity = cart.CartItems.Sum(s => s.PurchaseQuantity) + cart.OtherCartItems.Sum(s => s.PurchaseQuantity);
            var totalAmount = cart.CartItems.Sum(s => s.PurchaseQuantity * s.PurchaseProductSupplier.Price) + cart.OtherCartItems.Sum(s => s.PurchaseQuantity * s.PurchasePrice);
            return Json(new { success = true, TotalQuantity = totalQuantity, TotalAmount = totalAmount, msg = "删除成功" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateShoppingCartItemRemarks(int itemId, string remarks)
        {
            var command = new UpdatePurchaseShoppingCartItemRemarkCommand
            {
                ItemId = itemId,
                Remark = remarks
            };
            _commandService.Execute(command);

            return Json(new { success = true, msg = "修改成功" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateShoppingCartItemQuantity(int itemId, int cartId, int quantity)
        {
            var command = new UpdatePurchaseShoppingCartItemQuantityCommand
            {
                ItemId = itemId,
                CartId = cartId,
                Quantity = quantity
            };
            _commandService.Execute(command);
            var cart = _fetcher.Get<PurchaseShoppingCart>(command.CartId);
            var cartItem = _fetcher.Get<PurchaseShoppingCartItem>(command.ItemId);
            var totalQuantity = cart.CartItems.Sum(s => s.PurchaseQuantity) + cart.OtherCartItems.Sum(s => s.PurchaseQuantity);
            var totalAmount = cart.CartItems.Sum(s => s.PurchaseQuantity * s.PurchaseProductSupplier.Price) + cart.OtherCartItems.Sum(s => s.PurchaseQuantity * s.PurchasePrice);
            var itemTotalAmount = cartItem.PurchaseQuantity * cartItem.PurchaseProductSupplier.Price;

            return Json(new { success = true, TotalQuantity = totalQuantity, TotalAmount = totalAmount, ItemTotalAmount = itemTotalAmount, msg = "修改成功" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Statistics(PurchaseStatisticsQuery query = null)
        {
            var projects = _projectQueryService.QueryAllValidByProjectFilter().ToList();
            var categories = _productCategoryQuery.QueryAllValidByFilter().ToList();
            var departments = _departmentQueryService.QueryAllValidByDepartmentFilter().ToList();
            var products = _productQuery.GetProductsByProductCategorys(categories.Select(s => s.Id).ToList());
            var viewModel = new StatisticsViewModel
            {
                Query = query,
                ProjectList = projects.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),

                DepartmentList = departments.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                ProductCategoryTreeView = new ProductCategoryTreeView().GetProductCategoryTreeView(categories),
            };
            viewModel.ProductList = products.Select(s => new SelectListItem { Text = $"{s.Name}({s.Specification})", Value = s.Id.ToString() });

            if (!string.IsNullOrWhiteSpace(query?.ProductCategoryIds))
            {
                var productCategoryIdList = query.ProductCategoryIds.SplitToList<int>(',').ToList();
                viewModel.CategoryText =
                    string.Join(",", categories.Where(x => productCategoryIdList.Contains(x.Id)).Select(s => s.Name));
                var productlist = products.Where(x => productCategoryIdList.Contains(x.ProductCategory.Id))
                    .ToList();
                viewModel.ProductList =
                    productlist.Select(s => new SelectListItem { Text = $"{s.Name}({s.Specification})", Value = s.Id.ToString() });
            }
            viewModel.Result = _purchaseStatisticsQueryService.GetOrderItems(query);
            return View("~/Views/NewPurchase/Statistics.cshtml", viewModel);
        }

        public ActionResult ProductList(int cartId, string returnUrl, int page = 1, int pageSize = Web.Common.PaginationSetttings.PageSize, ProductQuery query = null)
        {
            var cart = _fetcher.Get<Entities.PurchaseShoppingCart>(cartId);
            if (cart != null)
            {
                query.ProjectId = cart.Project.Id;
            }
            var categories = _productCategoryQuery.QueryAllValid().ToList();

            var productList = _productQuery.QueryListViewForPurchaseOrder(query,page,pageSize);
            var viewModel = new ProductListViewModel
            {
                Categories = categories.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                ReturnUrl = returnUrl,
                TotalAmount = cart.CartItems.Sum(x => x.PurchaseQuantity * x.PurchaseProductSupplier.Price),
                CartId = cart.Id,
                Query = query,
                CategoryText = query.ProductCategoryId.HasValue
                    ? categories.SingleOrDefault(x => x.Id == query.ProductCategoryId).Name
                    : string.Empty,
                ProductCategoryTreeView = new ProductCategoryTreeView().GetProductCategoryTreeView(categories),
                Items = productList.GetListViewWithStock(_purchaseProductStockQuery, query.ProjectId)
            };
            viewModel.CartItems = cart.CartItems.Select(item => new CartItem
            {
                Id = item.PurchaseProductSupplier.Id,
                Name = item.PurchaseProductSupplier.PurchaseProduct.Name,
                Quantity = item.PurchaseQuantity,
                Price = item.PurchaseProductSupplier.Price
            }).ToList();

            viewModel.Suppliers = _purchaseSupplierQuery.QueryAll().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            return View("~/Views/NewPurchase/ProductList.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult DownLoad(DownloadFoodPurchaseExcelCommand command)
        {
            var result = _commandService.ExecuteFoResult(command);
            return Json(new
            {
                success = result.IsSucceed,
                redirect = $"{Url.Content("~/Attachments/FoodDownload/")}{result.FileName}"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult ImportFood(int cartId)
        {
            ImportFoodViewModel viewModel = new ImportFoodViewModel
            {
                CartId = cartId
            };
            return PartialView("~/Views/NewPurchase/_ProductSupplier.Import.cshtml", viewModel);
        }
        [HttpPost]
        public ActionResult ImportFood(ImportFoodPurchaseExcelCommand command)
        {
            if (Request.Files.Count > 0)
            {
                command.FilePath = Request.Files[0].ReadBytes();
                command.FileName = Request.Files[0].FileName;
            }
            _commandService.Execute(command);

            return RedirectToAction("Edit", "Purchaseshoppingcart", new { id = command.CartId });
        }
    }
}