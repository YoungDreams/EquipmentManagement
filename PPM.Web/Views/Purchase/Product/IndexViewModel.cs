using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Foundation.Data;
using PensionInsurance.Query;
using PensionInsurance.Web.Common;
using PensionInsurance.Commands;

namespace PensionInsurance.Web.Views.Purchase.Product
{
    public class IndexViewModel
    {
        private readonly UrlHelper _urlHelper;

        public IndexViewModel(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public ProductQuery Query { get; set; }
        public PagedData<Entities.PurchaseProductSupplier> Items { get; set; }
        public IEnumerable<PriceItemViewModel> PriceItems { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<SelectListItem> Suppliers { get; set; }
        public IEnumerable<SelectListItem> Areas { get; set; }
        public ProductCategoryTreeView ProductCategoryTreeView { get; set; }
        public string CategoryText { get; set; }

        public object DeleteCommand(int id, string strUrl)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("Delete", "Product"),
                Command = new DeleteProductCommand { Id = id, ReturnUrl = strUrl }
            };
        }

        public IEnumerable<PriceItemViewModel> GroupBySupplierPrices(PagedData<Entities.PurchaseProductSupplier> items)
        {
            var distinctProducts = items.Data.Select(x => x.PurchaseProduct).Distinct();
            var priceItemViewModels = new List<PriceItemViewModel>();
            foreach (var product in distinctProducts)
            {
                var priceItems = items.Where(x => x.PurchaseProduct == product);
                var item = new PriceItemViewModel();
                item.Items = new List<MaterielItem>();
                item.Brand = priceItems.First().PurchaseProduct.Brand;
                item.Area = priceItems.First().Area.Name;
                item.Description = priceItems.First().PurchaseProduct.Description;
                item.PurchaseProductId = priceItems.First().PurchaseProduct.Id;
                item.Product = priceItems.First().PurchaseProduct.Name;
                item.Code = priceItems.First().PurchaseProduct.Code;
                item.ProductCategory = priceItems.First().PurchaseProduct.ProductCategory.Name;
                item.PurchaseSupplier = priceItems.First().PurchaseSupplier.Name;
                item.Published = priceItems.First().PurchaseProduct.Published;
                item.Specification = priceItems.First().PurchaseProduct.Specification;
                item.ThumbnailPath = priceItems.First().PurchaseProduct.ProductPictures.FirstOrDefault()?.ThumbnailPath;
                item.Unit = priceItems.First().PurchaseProduct.Unit;
                item.IsMaterial = priceItems.First().PurchaseProduct.IsMateriel;
                foreach (var purchaseProductSupplier in priceItems)
                {
                    item.Items.Add(new MaterielItem
                    {
                        Id = purchaseProductSupplier.Id,
                        Max = purchaseProductSupplier.Max,
                        Min = purchaseProductSupplier.Min,
                        Price = purchaseProductSupplier.Price,
                        Supplier = purchaseProductSupplier.PurchaseSupplier.Name,
                        Area = purchaseProductSupplier.Area.Name,
                        IsValid = purchaseProductSupplier.IsValid
                    });
                }
                priceItemViewModels.Add(item);
            }
            return priceItemViewModels;
        }
    }

    public class PriceItemViewModel
    {
        public int PurchaseProductId { get; set; }
        public string Product { get; set; }
        public string Code { get; set; }
        public string ThumbnailPath { get; set; }
        public string ProductCategory { get; set; }
        public string Brand { get; set; }
        public string Specification { get; set; }
        public string Unit { get; set; }
        public string PurchaseSupplier { get; set; }
        public string Description { get; set; }
        public bool Published { get; set; }
        public bool IsMaterial { get; set; }
        public string Area { get; set; }
        public List<MaterielItem> Items { get; set; }
    }
}