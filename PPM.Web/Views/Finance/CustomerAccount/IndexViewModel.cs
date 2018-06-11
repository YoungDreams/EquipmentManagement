using Foundation.Data;
using PensionInsurance.Commands;
using PensionInsurance.Query;
using PensionInsurance.Web.Common;
using System.Collections.Generic;
using System.Web.Mvc;
using PensionInsurance.Entities;

namespace PensionInsurance.Web.Views.Finance.CustomerAccount
{
    public class IndexViewModel
    {
        private readonly UrlHelper _urlHelper;
        public IndexViewModel(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        // 客户账户
        public CustomerAccountQuery Query { get; set; }
        public PagedData<Entities.DetailViews.CustomerAccountDetail> CustomerAccountList { get; set; }
        // 客户金额
        public List<CustomerPayment> CustomerPaymentList { get; set; }
        public CustomerPaymentQuery CustomerPaymentQuery { get; set; }
        // 客户积分
        public List<CustomerPoint> CustomerPointList { get; set; }
        public CustomerPointQuery CustomerPointQuery { get; set; }

        public int CustomerAccountId { get; set; }

        // 删除缴费记录
        public object DeletePayment(int id)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("DeleteCustomerPayment", "CustomerAccount"),
                Command = new DeleteCustomerPaymentCommand { CustomerPaymentId = id}
            };
        }

        // 缴费记录确认
        public object ConfirmPayment(int customerPaymentId)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("SubmitCustomerPayment", "CustomerAccount"),
                Command = new SubmitCustomerPaymentCommand { CustomerPaymentId = customerPaymentId}
            };
        }

        // 删除缴费记录
        public object DeletePoint(int id)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("DeletePoint", "CustomerAccount"),
                Command = new DeleteEntityCommand { EntityId = id}
            };
        }

        // 缴费记录确认
        public object ConfirmPoint(int customerPointId)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("SubmitCustomerPoint", "CustomerAccount"),
                Command = new SubmitCustomerPointCommand { CustomerPointId = customerPointId }
            };
        }
    }
}