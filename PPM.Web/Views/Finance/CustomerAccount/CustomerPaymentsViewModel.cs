using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foundation.Data;
using PensionInsurance.Commands;
using PensionInsurance.Web.Common;

namespace PensionInsurance.Web.Views.Finance.CustomerAccount
{
    public class CustomerPaymentsViewModel
    {
        private readonly UrlHelper _urlHelper;
        public CustomerPaymentsViewModel(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }
        public PagedData<Entities.CustomerPayment> CustomerPaymentList { get; set; }

        // 删除缴费记录
        public object DeletePayment(int id)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("DeletePayment", "CustomerAccount"),
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
    }
}