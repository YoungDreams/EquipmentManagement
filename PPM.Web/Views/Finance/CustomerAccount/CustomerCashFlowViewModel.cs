using System.Web.Mvc;
using Foundation.Data;
using PensionInsurance.Commands;
using PensionInsurance.Query;
using PensionInsurance.Web.Common;

namespace PensionInsurance.Web.Views.Finance.CustomerAccount
{
    public class CustomerCashFlowViewModel
    {
        private readonly UrlHelper _urlHelper;
        public CustomerCashFlowViewModel(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper; 
        }
        // 删除缴费记录
        public object DeletePayment(int id)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("DeleteCustomerPayment", "CustomerAccount"),
                Command = new DeleteEntityCommand { EntityId = id }
            };
        }

        // 缴费记录确认
        public object ConfirmPayment(int customerPaymentId)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("SubmitCustomerPayment", "CustomerAccount"),
                Command = new SubmitCustomerPaymentCommand { CustomerPaymentId = customerPaymentId }
            };
        }
        public CustomerCashFlowQuery Query { get; set; }
        public PagedData<Entities.CustomerPayment> Items { get; set; }
    }
}