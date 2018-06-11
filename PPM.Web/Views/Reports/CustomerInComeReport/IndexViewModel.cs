using System.Collections.Generic;
using System.Web.Mvc;
using Foundation.Data;
using PensionInsurance.Entities;
using PensionInsurance.Query;

namespace PensionInsurance.Web.Views.Reports.CustomerInComeReport
{
    public class IndexViewModel
    {
        public CustomerInComeReportQuery Query { get; set; }
        public IEnumerable<CustomerInComeReportDetail> CustomerInComeReports { get; set; }
        public IEnumerable<SelectListItem> ProjectList { get; set; }
        public int QueryTimeMonths { get; set; }
    }
}