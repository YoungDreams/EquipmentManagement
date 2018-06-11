using System.Collections.Generic;
using System.Web.Mvc;
using Foundation.Data;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Query;
using PensionInsurance.Web.Common;

namespace PensionInsurance.Web.Views.CustomerOrderFood
{
    public class IndexViewModel
    {
        private readonly UrlHelper _urlHelper;
        private readonly IFetcher _fetcher;

        public IndexViewModel(UrlHelper urlHelper, IFetcher fetcher)
        {
            _urlHelper = urlHelper;
            _fetcher = fetcher;
        }

        public ConsultingOrderFoodQuery Query { get; set; }
        public PagedData<Entities.ConsultingOrderFood> CashPaidOrderFoods { get; set; }
        public IEnumerable<SelectListItem> Projects { get; set; }

        public string GetSourceMsg(OrderFoodSourceType sourceType, int foreignkeyId)
        {
            if (sourceType == OrderFoodSourceType.咨询接待)
            {
                var source = _fetcher.Get<Entities.Consulting>(foreignkeyId);
                return $"{source.ConsultingName}-{source.ConsultingNo}";
            }
            else if (sourceType == OrderFoodSourceType.销售渠道)
            {
                var source = _fetcher.Get<Entities.SaleChannel>(foreignkeyId);
                return $"{source.Name}-{source.SaleChannelNo}";
            }
            else if (sourceType == OrderFoodSourceType.老人亲友)
            {
                var source = _fetcher.Get<Entities.CustomerAccount>(foreignkeyId);
                return $"{source.Customer.Name}-{source.Customer.Consulting.ConsultingNo}";
            }
            return "";
        }

        public object DeleteCommand(int id)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("Delete", "CustomerOrderFood"),
                Command = new DeleteEntityCommand { EntityId = id }
            };
        }

        public object CancelPayMentCommand(int id)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("CancelPayment", "CustomerOrderFood"),
                Command = new CancelCustomerOrderFoodPaymentCommand { CashPaidOrderFoodId = id }
            };
        }

        public object CompleteBillCommand(int id)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("CompleteBill", "CustomerOrderFood"),
                Command = new CancelCustomerOrderFoodPaymentCommand { CashPaidOrderFoodId = id }
            };
        }
    }
}