using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foundation.Data;
using Foundation.Data.Implemention;
using PensionInsurance.Entities;
using PensionInsurance.Query;
using PensionInsurance.Shared;
using PensionInsurance.Web.Views.Purchase;

namespace PensionInsurance.Web.Views.NewPurchase
{
    public class ProductListViewModel
    {
        public int CartId { get; set; }
        public decimal TotalAmount { get; set; }
        public List<CartItem> CartItems { get; set; }
        public int ProjectId { get; set; }
        public bool IsFood { get; set; }
        public ProductQuery Query { get; set; }
        public PagedData<Entities.DetailViews.PurchaseProductListView> Items { get; set; }
        public IEnumerable<SelectListItem> Suppliers { get; set; }
        public string ReturnUrl { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        public ProductCategoryTreeView ProductCategoryTreeView { get; set; }
        public string CategoryText { get; set; }
    }

    public class CartItem
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
    }

    public static class ProductListViewModelExtension
    {
        public static PagedData<Entities.DetailViews.PurchaseProductListView> GetListViewWithStock(this PagedData<Entities.DetailViews.PurchaseProductListView> listView, IPurchaseProductStockQuery purchaseProductStockQuery, int? projectId)
        {
            var stocks = purchaseProductStockQuery.QueryAll();
            foreach (var purchaseProductStock in stocks)
            {
                var price = purchaseProductStock.PurchaseProductSupplier;
                var stock = purchaseProductStock.Stock;
                var project = purchaseProductStock.Project;
                var products =
                    listView.Where(x => x.PruductSupplierPriceId == price.Id && x.Id == price.PurchaseProduct.Id);
                if (projectId.HasValue &&
                    projectId == purchaseProductStock.Project.Id)
                {
                    foreach (var product in products)
                    {
                        product.StockSum += stock;
                        product.Stocks.Add(project.Name + "(" + stock + ")");
                    }
                }
            }
            return listView;
        }
    }
}