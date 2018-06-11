using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PensionInsurance.Web.Views.Purchase
{
    public class ConfirmViewModel
    {
        public int ProjectId { get; set; }
        public IEnumerable<SelectListItem> ProjectList { get; set; }
        public int OrderType { get; set; }
        public DateTime PurchaseStartDate { get; set; }
        public DateTime PurchaseEndDate { get; set; }
    }
}