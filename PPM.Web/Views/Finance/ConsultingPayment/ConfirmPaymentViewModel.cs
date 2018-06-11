namespace PensionInsurance.Web.Views.Finance.ConsultingPayment
{
    public class ConfirmPaymentViewModel
    {
        public string CustomerName { get; set; }
        public string PaymentType { get; set; }
        public decimal PayMoney { get; set; }
        public int CashPaidOrderFoodSerialId { get; set; }
    }
}