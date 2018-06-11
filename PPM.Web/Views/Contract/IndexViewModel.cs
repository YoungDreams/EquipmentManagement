using System.Collections.Generic;
using Foundation.Data;
using PensionInsurance.Entities.DetailViews;
using PensionInsurance.Query;
using System.Web.Mvc;

namespace PensionInsurance.Web.Views.Contract
{
    public class IndexViewModel
    {
        private readonly UrlHelper _urlHelper;
        public IndexViewModel(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public ContractQuery Query { get; set; }
        public PagedData<ContractDetail> Contractlist { get; set; }
        public IEnumerable<SelectListItem> ProjectList { get; set; }
        public IEnumerable<SelectListItem> SaleUserList { get; set; }
    }
}