using PensionInsurance.Commands;

namespace PensionInsurance.Web.Views.Finance.CustomerAccount
{
    public class EditCustomerPaymentViewModel : EditCustomerPaymentCommand
    {
        public string CustomerName { get; set; }
        public string PaymentTypeDesc { get; set; }
    }
}