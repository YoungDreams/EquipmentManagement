using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Foundation.Data;
using PensionInsurance.Entities;
using PensionInsurance.Web.Views.Purchase;

namespace PensionInsurance.Web.Views.NewPurchase.ShoppingCart
{
    public class EditViewModel
    {
        private readonly UrlHelper _urlHelper;
        private readonly IFetcher _fetcher;

        public EditViewModel(UrlHelper urlHelper, IFetcher fetcher)
        {
            _urlHelper = urlHelper;
            _fetcher = fetcher;
        }

        public Entities.PurchaseShoppingCart Cart { get; set; }
        public int TotalQuantity { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CheckTime { get; set; }
        public DateTime ApplyDate { get; set; }
        public string CheckRemarks { get; set; }
        public FoodOrderType? FoodOrderType { get; set; }
        public ProductCategoryTreeView ProductCategoryTreeView { get; set; }
        public int GetStock(int pruductSupplierId, int projectId)
        {
            var productStock =
                _fetcher.Query<Entities.PurchaseProductStock>()
                    .FirstOrDefault(x => x.PurchaseProductSupplier.Id == pruductSupplierId && x.Project.Id == projectId);
            return productStock?.Stock ?? 0;
        }
    }

}