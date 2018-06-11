using System.Collections.Generic;
using Foundation.Data;
using System.Web.Mvc;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Entities.DetailViews;
using PensionInsurance.Query;
using PensionInsurance.Web.Common;

namespace PensionInsurance.Web.Views.Finance.Bill
{
    public class IndexViewModel
    {
        private readonly UrlHelper _url;

        public IndexViewModel(UrlHelper url)
        {

            _url = url;
        }
        public decimal AllowedRelief { get; set; }
        public PagedData<CustomerBillDetail> Items { get; set; }
        public CustomerBillQuery Query { get; set; }
        public IEnumerable<SelectListItem> ProjectList { get; set; }

    }
}