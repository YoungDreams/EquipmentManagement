using PensionInsurance.Entities;

namespace PensionInsurance.Web.Views.CustomerOrderFood
{
    public class PayOrderFoodViewModel
    {
        public int ConsultingOrderFoodId { get; set; }
        public string ProjectName { get; set; }
        public string CustomerName { get; set;}
        public int UserId { get; set; }
        public int ServicePackCatalogId { get; set; }
        public string ServicePackCatalogName { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Amount { get; set; }
        public decimal Total { get; set; }
        public decimal PaidMoney { get; set; }
        public decimal UnPaidMoney { get; set; }
        public decimal WaitConfirmMoney { get; set; }
        public decimal PayMoney { get; set; }
        public string Remark { get; set; }
        public PaymentType PaymentType { get; set; }

    }
}