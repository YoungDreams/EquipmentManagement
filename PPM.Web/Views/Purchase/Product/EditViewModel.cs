using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PensionInsurance.Commands;
using PensionInsurance.Web.Common;

namespace PensionInsurance.Web.Views.Purchase.Product
{
    public class EditViewModel : EditProductCommand
    {
        private readonly UrlHelper _urlHelper;

        public EditViewModel(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }
        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<SelectListItem> Cities { get; set; }
        public IEnumerable<SelectListItem> Suppliers { get; set; }
        public IEnumerable<Entities.PurchaseProductSupplier> PriceItems { get; set; }
        public ProductCategoryTreeView ProductCategoryTreeView { get; set; }
        public string CategoryText { get; set; }
        public string BackUrl { get; set; }

        public object DeleteCommand(int id, string strUrl)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("Delete", "ProductSupplier"),
                Command = new DeletePurchaseSupplierCommand { Id = id, ReturnUrl = strUrl }
            };
        }
    }
}