using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PensionInsurance.Entities;
using PensionInsurance.Query;

namespace PensionInsurance.Web.Views.Reports.SalesPersonDailyReport
{
    public class IndexViewModel
    {
        public IEnumerable<SelectListItem> ProjectList { get; set; }
        public IList<Entities.SalesProjectDailyReport> ReportData { get; set; }
        public SalesProjectDailyReportQuery Query { get; set; }
        public IEnumerable<Project> Projects { get; set; }

        public Entities.SalesProjectDailyReport GetDaily(int projectId, DateTime date)
        {
            return ReportData.FirstOrDefault(x => x.Project.Id== projectId && x.ReportDate.Date == date.Date);
        }
    }
}