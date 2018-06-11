using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PensionInsurance.Web.Views.Purchase.Order
{
    public class AddCartViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public IEnumerable<Entities.PurchaseProductSupplier> PurchaseProductSuppliers { get; set; }
        public decimal ProductPrice { get; set; }
    }
}