using System.Collections.Generic;
using System.Web.Mvc;
using Foundation.Data;
using PensionInsurance.Commands;
using PensionInsurance.Query;
using PensionInsurance.Web.Common;

namespace PensionInsurance.Web.Views.CustomerMedicalAdvice
{
    public class IndexViewModel
    {
        private readonly UrlHelper _urlHelper;

        public IndexViewModel(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public CustomerMedicalAdviceQuery Query { get; set; }
        public PagedData<Entities.CustomerMedicalAdvice> CustomerMedicalAdvices { get; set; }
        public IEnumerable<SelectListItem> Projects { get; set; }
    }
}