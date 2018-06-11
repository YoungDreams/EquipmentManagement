using System.Collections.Generic;
using System.Web.Mvc;
using Foundation.Data;
using PensionInsurance.Commands;
using PensionInsurance.Query;
using PensionInsurance.Web.Common;

namespace PensionInsurance.Web.Views.Finance.ConsultingPayment
{
    public class IndexViewModel
    {
        private readonly UrlHelper _urlHelper;

        public IndexViewModel(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public ConsutlingPaymentQuery Query { get; set; }
        public PagedData<Entities.ConsutlingPayment> CashPaidOrderFoodSerials { get; set; }
        public IEnumerable<SelectListItem> Projects { get; set; }

        public object DeleteCommand(int id)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("Delete", "CustomerOrderFoodSerial"),
                Command = new DeleteEntityCommand { EntityId = id }
            };
        }

        public object ConfirmPayMentCommand(int id)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("ConfirmPayment", "CustomerOrderFoodSerial"),
                Command = new ConfirmCustomerOrderFoodSerialPaymentCommand { CashPaidOrderFoodSerialId = id }
            };
        }
    }
}