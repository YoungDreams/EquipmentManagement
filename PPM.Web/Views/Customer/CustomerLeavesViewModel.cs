using System.Web.Mvc;
using Foundation.Data;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Query;
using PensionInsurance.Web.Common;

namespace PensionInsurance.Web.Views.Customer
{
    public class CustomerLeavesViewModel
    {
        private readonly UrlHelper _urlHelper;
        public CustomerLeavesViewModel(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }
        // 客户账户
        public CustomerLeaveQuery Query { get; set; }
        public PagedData<CustomerLeave> CustomerWithoutList { get; set; }

        public object DeleteCommand(int id)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("DeleteCustomerLeave", "Customer"),
                Command = new DeleteEntityCommand { EntityId = id }
            };
        }
    }
}