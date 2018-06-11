using System.Web.Mvc;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Web.Common;
using PensionInsurance.Web.Views.TrackingResult;

namespace PensionInsurance.Web.Views.CustomerAccountCheckOutRefund
{
    public class EditViewModel
    {
        private readonly UrlHelper _urlHelper;
        public EditViewModel(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public int ContractRefundId { get; set; }
        public Entities.CustomerAccountCheckOutRefund RefundInfo { get; set; }
        public TrackingResultViewModel TrackingResult { get; set; }
        public WorkflowStep CurrentWorkFlowStep { get; set; }
        public int CurrentWorkflowStepId { get; set; }
        public string RefundReceiptFile { get; set; }
        public string RefundReceiptFileName { get; set; }

        public object PrintRefund(int checkOutRefundId)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("PrintRefund", "CustomerAccountCheckOutRefund"),
                Command = new PrintCheckOutRefundWordToPdfCommand { CheckOutRefundId = checkOutRefundId }
            };
        }
        public object PrintAgreement(int checkOutRefundId)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("PrintAgreement", "CustomerAccountCheckOutRefund"),
                Command = new PrintCheckOutAgreementWordToPdfCommand { CheckOutRefundId = checkOutRefundId }
            };
        }
    }
}