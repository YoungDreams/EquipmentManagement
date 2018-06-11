using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PensionInsurance.Entities;
using PensionInsurance.Web.Views.TrackingResult;

namespace PensionInsurance.Web.Views.Purchase.Order
{
    public class ConfirmOrderViewModel
    {
        public Entities.Project Project { get; set; }
        public Entities.OrderType OrderType { get; set; }
        public IEnumerable<Entities.PurchaseOrderAcceptance> PurchaseOrderAcceptances { get; set; }
        public DateTime PurchaseStartDate { get; set; }
        public DateTime PurchaseEndDate { get; set; }
        public decimal TotalAmount { get; set; }
    }
}