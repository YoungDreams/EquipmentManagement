using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PensionInsurance.Entities;

namespace PensionInsurance.Web.Views.Finance.ReliefBill
{
    public class CreateViewModel
    {
        public int CustomerAccountId { get; set; }
        public Entities.CustomerAccount CustomerAccount { get; set; }
        public IList<CustomerBill> CustomerBills { get; set; }
        public bool IsIncluded { get; set; }
        public int BillId { get; set; }
        public decimal RoomCost { get; set; }
        public decimal MealsCost { get; set; }
        public decimal PackageServiceCost { get; set; }
        public decimal IncrementCost { get; set; }
        public decimal ActualPaymentCost { get; set; }
        public decimal CustomerCurrentYearDiscount { get; set; }
        public decimal ProjectYearDiscount { get; set; }
        public string Description { get; set; }
    }
}