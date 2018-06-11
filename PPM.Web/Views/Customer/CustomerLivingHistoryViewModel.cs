using PensionInsurance.Query;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PensionInsurance.Web.Views.Customer
{
    public class CustomerLivingHistoryViewModel
    {
        private readonly UrlHelper _urlHelper;

        public CustomerLivingHistoryViewModel(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }
        public CustomerLivingHistoryQuery Query { get; set; }
        public IEnumerable<Entities.CustomerLivingHistory> CustomerLivingHistories { get; set; }
    }
}