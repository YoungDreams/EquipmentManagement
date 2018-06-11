using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PensionInsurance.Commands;

namespace PensionInsurance.Web.Views.Purchase.ProductSupplier
{
    public class CreateViewModel: CreatePurchaseProductSupplierCommand
    {
        public IEnumerable<SelectListItem> PurchaseProducts { get; set; }
        public IEnumerable<SelectListItem> PurchaseSuppliers { get; set; }
    }
}