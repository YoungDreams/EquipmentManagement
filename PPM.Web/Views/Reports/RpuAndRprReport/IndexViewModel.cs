using System.Collections.Generic;
using System.Web.Mvc;
using PensionInsurance.Query;

namespace PensionInsurance.Web.Views.Reports.RpuAndRprReport
{
    public class IndexViewModel
    {
        public RpuAndRprReportQuery Query { get; set; }
        public IEnumerable<Entities.Reports.RpuAndRprReport> RpuAndRprReports { get; set; }
        public IEnumerable<SelectListItem> ProjectList { get; set; }
        public int QueryTimeMonths { get; set; }
    }
}