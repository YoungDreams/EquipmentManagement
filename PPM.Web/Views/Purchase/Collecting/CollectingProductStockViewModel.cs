using System.Web.Mvc;
using Foundation.Data;
using PensionInsurance.Query;
using PensionInsurance.Web.Common;
using PensionInsurance.Commands;
using System.Collections.Generic;
using PensionInsurance.Entities;

namespace PensionInsurance.Web.Views.Purchase.Collecting
{
    public class CollectingProductStockViewModel
    {
        private readonly UrlHelper _urlHelper;
        public CollectingProductStockViewModel(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public string DepartmentName { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
        public string CollectingDate { get; set; }
        public List<SelectListItem> Suppliers { get; set; }
        public ProductCategoryTreeView ProductCategoryTreeView { get; set; }
        public string CategoryText { get; set; }
        public PurchaseProductStockQuery Query { get; set; }
        public PagedData<Entities.PurchaseProductStock> Items { get; set; }
        public Dictionary<PurchaseProductCategory, IEnumerable<PurchaseProductCategory>> CategoriesData { get; set; }
        public object DeleteCommand(int id, string strUrl)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("Delete", "Supplier"),
                Command = new DeletePurchaseSupplierCommand { Id = id, ReturnUrl = strUrl }
            };
        }
    }

    public class CollectingProductStockItem
    {
        public int ProductId { get; set; }
        public int PriceId { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        public string Stock { get; set; }
    }

    public class IndexViewModel
    {
        public int ProjectId { get; set; }
        public List<SelectListItem> Projects { get; set; }
    }

    public class CollectingProductViewModel
    {
        private readonly UrlHelper _urlHelper;
        public CollectingProductViewModel(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }
        public CollectingProductQuery Query { get; set; }
        public string DepartmentName { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
        public string CollectingDate { get; set; }
        public List<string> ConfirmPaperFilePathList { get; set; }
        public List<SelectListItem> Suppliers { get; set; }
        public PagedData<Entities.CollectingProduct> Items { get; set; }
        public object DeleteCommand(int id, string strUrl)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("DeleteCollectingProduct", "Collecting"),
                Command = new DeleteCollectingProductCommand { Id = id, ReturnUrl = strUrl }
            };
        }
    }
}