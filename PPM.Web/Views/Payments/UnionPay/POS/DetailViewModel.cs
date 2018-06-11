using System.Collections.Generic;
using PensionInsurance.Entities;
using PensionInsurance.Query;
using OrderItem = PensionInsurance.Query.OrderItem;

namespace PensionInsurance.Web.Views.Payments.UnionPay.POS
{
    public class DetailViewModel
    {
        public string CustomerPaymentOrderNo { get; set; }
        public string CustomerName { get; set; }
        public string CustomerNo { get; set; }
        public IList<OrderItem> Items { get; set; }
    }
}