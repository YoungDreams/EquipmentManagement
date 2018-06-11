using System.Collections.Generic;
using System.Web.Mvc;
using Foundation.Data;
using PensionInsurance.Query;

namespace PensionInsurance.Web.Views.Reports.CustomerDistributionReport
{
    public class IndexViewModel
    {
        public CustomerDistributionReportQuery Query { get; set; }
        public IEnumerable<SelectListItem> ProjectList { get; set; }
        public IEnumerable<string> Addresses { get; set; }
    }
}