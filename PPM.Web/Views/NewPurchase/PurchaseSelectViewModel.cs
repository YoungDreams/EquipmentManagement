using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PensionInsurance.Web.Views.NewPurchase
{
    public class PurchaseSelectViewModel
    {
        public int ProjectId { get; set; }
        public int OrderType { get; set; }
        public IEnumerable<SelectListItem> ProjectList { get; set; }
    }
}