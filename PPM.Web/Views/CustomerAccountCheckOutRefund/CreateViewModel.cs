using System.Collections.Generic;
using PensionInsurance.Entities;

namespace PensionInsurance.Web.Views.CustomerAccountCheckOutRefund
{
    public class CreateViewModel
    {
        public int? CheckOutRefundId { get; set; }
        public Entities.Project Project { get; set; }
        public IEnumerable<Project> RefundProjects { get; set; }
        public RefundType? RefundType { get; set; }
        public RefundStatus RefundStatus { get; set; }
        public string RefundName { get; set; }
        public string RefundBank { get; set; }
        public decimal RefundMoney { get; set; }
        public string RefundAccountNo { get; set; }

        public string RefundName2 { get; set; }
        public string RefundBank2 { get; set; }
        public decimal RefundMoney2 { get; set; }
        public string RefundAccountNo2 { get; set; }

        public string RefundName3 { get; set; }
        public string RefundBank3 { get; set; }
        public string RefundAccountNo3 { get; set; }
        public decimal RefundMoney3 { get; set; }

        public string RefundReceiptFile { get; set; }
        public string RefundReceiptFileName { get; set; }
        public CustomerAccount CustomerAccount { get; set; }
        public TrackingResult.TrackingResultViewModel TrackingResult { get; set; }
    }
}