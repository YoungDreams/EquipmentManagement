using System.Collections.Generic;
using System.Web.Mvc;
using PensionInsurance.Query;

namespace PensionInsurance.Web.Views.Reports.ProjectDealReport
{
    public class IndexViewModel
    {
        public ProjectDealReportQuery Query { get; set; }
        public IEnumerable<Entities.Reports.ProjectDealReport> ProjectDealReports { get; set; }
        public IEnumerable<SelectListItem> ProjectList { get; set; }
        public int QueryTimeMonths { get; set; }
    }
}