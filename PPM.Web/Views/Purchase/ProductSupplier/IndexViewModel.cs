using System.Collections.Generic;
using System.Web.Mvc;
using Foundation.Data;
using PensionInsurance.Query;
using PensionInsurance.Web.Common;
using PensionInsurance.Commands;

namespace PensionInsurance.Web.Views.Purchase.ProductSupplier
{
    public class IndexViewModel
    {
        private readonly UrlHelper _urlHelper;
        public IndexViewModel(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }
        public PurchaseProductSupplierQuery Query { get; set; }
        public PagedData<Entities.PurchaseProductSupplier> Items { get; set; }
        public IEnumerable<SelectListItem> PurchaseProducts { get; set; }
        public IEnumerable<SelectListItem> PurchaseSuppliers { get; set; }
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