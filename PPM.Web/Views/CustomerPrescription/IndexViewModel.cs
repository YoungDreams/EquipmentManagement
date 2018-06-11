using System.Collections.Generic;
using System.Web.Mvc;
using Foundation.Data;
using PensionInsurance.Commands;
using PensionInsurance.Query;
using PensionInsurance.Web.Common;

namespace PensionInsurance.Web.Views.CustomerPrescription
{
    public class IndexViewModel
    {
        private readonly UrlHelper _urlHelper;

        public IndexViewModel(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public CustomerPrescriptionQuery Query { get; set; }
        public PagedData<Entities.CustomerPrescription> CustomerPrescriptions { get; set; }
        public IEnumerable<SelectListItem> Projects { get; set; }
    }
}