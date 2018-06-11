using System;
using System.Linq;
using System.Web.Mvc;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Query;
using PensionInsurance.Shared;
using PensionInsurance.Web.Views.Account;
using PensionInsurance.Web.Views.Sales.Budget;

namespace PensionInsurance.Web.Views.Purchase
{
    public class PurchaseController : AuthorizedController
    {
        private readonly IProjectQueryService _projectQueryService;
        private readonly ICommandService _commandService;
        private readonly IBudgetQueryService _budgetQueryService;
        private readonly IProductCategoryQuery _productCategoryQuery;
        private readonly IProductQuery _productQuery;
        private readonly IPurchaseProductStockQuery _purchaseProductStockQuery;
        private readonly IPurchaseSupplierQuery _purchaseSupplierQuery;
        public PurchaseController(IProjectQueryService projectQueryService, ICommandService commandService, IBudgetQueryService budgetQueryService, IProductCategoryQuery productCategoryQuery, IProductQuery productQuery, IPurchaseProductStockQuery purchaseProductStockQuery, IPurchaseSupplierQuery purchaseSupplierQuery)
        {
            _projectQueryService = projectQueryService;
            _commandService = commandService;
            _budgetQueryService = budgetQueryService;
            _productCategoryQuery = productCategoryQuery;
            _productQuery = productQuery;
            _purchaseProductStockQuery = purchaseProductStockQuery;
            _purchaseSupplierQuery = purchaseSupplierQuery;
        }

        public ActionResult Index(BudgetDetailQuery query)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.采购管理, Permission.查看))
            {
                return RedirectToAction("NoPermission", "Home");
            }
            var viewModel = new IndexViewModel(Url)
            {
                Query = query,
                ProjectList = _projectQueryService.QueryAllValidByProjectFilter().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                MonthData = _budgetQueryService.GetBudget(query),
            };

            return View("~/Views/Purchase/Index.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult Confirm()
        {
            var viewModel = new ConfirmViewModel
            {
                ProjectList = _projectQueryService.QueryAllValidByProjectFilter().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            return PartialView("~/Views/purchase/_purchase.Confirm.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult Confirm(PurchaseConfirmCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("ConfirmOrder", "Order", new { orderType = command.OrderType, projectId = command.ProjectId, purchaseStartDate = command.PurchaseStartDate, purchaseEndDate = command.PurchaseEndDate });
        }
       
    }
}