using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Foundation.Data;
using PensionInsurance.Entities;
using PensionInsurance.Payments.UnionPay.POS;
using PensionInsurance.Query;

namespace PensionInsurance.Web.Views.Payments.UnionPay.POS
{
    public class POSController : Controller
    {
        private readonly IFetcher _fetcher;
        private readonly ICustomerPaymentQueryService _customerPaymentQueryService;
        private readonly PensionInsurance.Payments.UnionPay.POS.POS _pos;
        public POSController(PensionInsurance.Payments.UnionPay.POS.POS pos, IFetcher fetcher, ICustomerPaymentQueryService customerPaymentQueryService)
        {
            _pos = pos;
            _fetcher = fetcher;
            _customerPaymentQueryService = customerPaymentQueryService;
        }

        [HttpPost]
        public ActionResult PaymentOrder(OrderInfoQueryRequest request)
        {
            return Content(_pos.PaymentOrder(request));
        }

        [HttpPost]
        public ActionResult ReceiveNotification(PayOrCancelConfirmRequest request)
        {
            return Content(_pos.ReceiveNotification(request));
        }

        [HttpGet]
        public ActionResult Detail(int id)
        {
            var customerPaymentOrder = _fetcher.Query<CustomerPaymentOrder>().FirstOrDefault(x=>x.CustomerPayment.Id == id);

            if (customerPaymentOrder == null)
            {
                throw new ApplicationException("支付订单错误！");
            }
            var viewModel = new DetailViewModel
            {
                CustomerPaymentOrderNo = customerPaymentOrder.PaymentOrderNo,
                CustomerNo = customerPaymentOrder.CustomerPayment.CustomerAccount.Customer.CustomerNo,
                CustomerName =  customerPaymentOrder.CustomerPayment.CustomerAccount.Customer.Name,
                Items = _customerPaymentQueryService.GetPaymentCaption(customerPaymentOrder.CustomerPayment.Id),
            };

            return View("~/Views/Payments/UnionPay/POS/Detail.cshtml", viewModel);
        }

        public ActionResult QRCode(string customerPaymentOrderNo)
        {
            return File(_pos.GenarateQRCode(customerPaymentOrderNo), "image/png");
        }
    }
}