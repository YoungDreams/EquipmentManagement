using System.Collections.Generic;
using System.Web.Mvc;
using Foundation.Data;
using PensionInsurance.Query;

namespace PensionInsurance.Web.Views.Reports.CustomerAcountCheckInReport
{
    public class IndexViewModel
    {
        public CustomerAcountCheckInReportQuery Query { get; set; }
        public IEnumerable<Entities.Reports.CustomerAcountCheckInReport> Report { get; set; }
        public IEnumerable<SelectListItem> ProjectList { get; set; }
    }
}