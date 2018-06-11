using System.Collections.Generic;
using System.Web.Mvc;
using PensionInsurance.Commands;

namespace PensionInsurance.Web.Views.Purchase.ProductSupplier
{
    public class EditViewModel : EditPurchaseProductSupplierCommand
    {
        public IEnumerable<SelectListItem> PurchaseProducts { get; set; }
        public IEnumerable<SelectListItem> PurchaseSuppliers { get; set; }
        public IEnumerable<SelectListItem> Cities { get; set; }
    }
}