using System.Collections.Generic;
using System.Web.Mvc;
using PensionInsurance.Entities;
using PensionInsurance.Query;

namespace PensionInsurance.Web.Views.Reports.CustomerMonthlyFeeDailyReport
{
    public class IndexViewModel
    {
        public CustomerMonthlyFeeDailyReportQuery Query { get; set; }
        public IEnumerable<CustomerMonthlyFeeDailyReportDetail> CustomerMonthlyFeeDailyReports { get; set; }
        public IEnumerable<SelectListItem> ProjectList { get; set; }
        public int QueryTimeDays { get; set; }
    }
}