using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PensionInsurance.Web.Views.Purchase.Order
{
    public class EditOrderOtherItemViewModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal PurchasePrice { get; set; }
        public string ProductUrl { get; set; }
    }
}
