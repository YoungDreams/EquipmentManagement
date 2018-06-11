using System.Collections.Generic;
using System.Web.Mvc;
using Foundation.Data;
using PensionInsurance.Query;

namespace PensionInsurance.Web.Views.Purchase.OrderAcceptance
{
    public class IndexViewModel
    {
        public PurchaseOrderAcceptanceQuery Query { get; set; }
        public PagedData<Entities.PurchaseOrderAcceptance> Items { get; set; }
        public IEnumerable<SelectListItem> ProjectList { get; set; }
    }
}