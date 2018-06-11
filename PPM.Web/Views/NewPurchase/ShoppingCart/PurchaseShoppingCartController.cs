using System;
using System.Linq;
using System.Web.Mvc;
using Foundation.Core;
using Foundation.Data;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Query;
using PensionInsurance.SendEmail;
using PensionInsurance.Shared;
using PensionInsurance.Web.Views.Account;
using PensionInsurance.Web.Views.Purchase;
using PensionInsurance.Web.Views.Sales.Budget;

namespace PensionInsurance.Web.Views.NewPurchase.ShoppingCart
{
    public class PurchaseShoppingCartController : AuthorizedController
    {
        private readonly IFetcher _fetcher;
        private readonly IProjectQueryService _projectQueryService;
        private readonly ICommandService _commandService;
        private readonly PurchaseOrderSendEmailService _emailService;
        private readonly IProductCategoryQuery _productCategoryQuery;

        public PurchaseShoppingCartController(IProjectQueryService projectQueryService, ICommandService commandService, IFetcher fetcher, PurchaseOrderSendEmailService emailService, IProductCategoryQuery productCategoryQuery)
        {
            _projectQueryService = projectQueryService;
            _commandService = commandService;
            _fetcher = fetcher;
            _emailService = emailService;
            _productCategoryQuery = productCategoryQuery;
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var cart = _fetcher.Get<Entities.PurchaseShoppingCart>(id);
            var viewModel = new EditViewModel(Url, _fetcher)
            {
                Cart = cart
            };
            var cates = _productCategoryQuery.GetCategoriesByDepartmentId(cart.PurchaseUser.Department.Id).ToList();

            viewModel.ProductCategoryTreeView = new ProductCategoryTreeView().GetProductCategoryTreeView(cates);

            viewModel.TotalPrice = cart.CartItems.Sum(s => s.PurchaseQuantity * s.PurchaseProductSupplier.Price);
            viewModel.TotalPrice += cart.OtherCartItems.Sum(s => s.PurchaseQuantity * s.PurchasePrice);
            viewModel.TotalQuantity = cart.CartItems.Sum(s => s.PurchaseQuantity);
            viewModel.TotalQuantity += cart.OtherCartItems.Sum(s => s.PurchaseQuantity);
            return View("~/Views/Newpurchase/ShoppingCart/Edit.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult Submit(PurchaseSubmitShoppingCartCommand command)
        {
            var result = _commandService.ExecuteFoResult(command);
            
            return RedirectToAction("Detail", "Order", new { id = result.OrderId });
        }

        [HttpGet]
        public PartialViewResult CreateOtherItem(int cartId)
        {
            var viewModel = new CreateOtherItemViewModel()
            {
                CartId = cartId
            };
            return PartialView("~/Views/Newpurchase/ShoppingCart/_CreateOtherItem.cshtml", viewModel);
        }
        public void CreateOtherItem(CreatePurchaseShoppingCartOtherItemCommand command)
        {
            _commandService.Execute(command);
        }
    }
}