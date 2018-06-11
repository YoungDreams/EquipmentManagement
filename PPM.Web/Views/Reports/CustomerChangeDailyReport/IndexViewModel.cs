using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PensionInsurance.Entities;
using PensionInsurance.Query;

namespace PensionInsurance.Web.Views.Reports.CustomerChangeDailyReport
{
    public class IndexViewModel
    {
        public IList<Project> Projects { get; set; }
        public IList<Entities.CustomerChangeDailyReport> ReportData { get; set; }

        public CustomerChangeDailyReportQuery Query { get; set; }

        public IEnumerable<SelectListItem> ProjectSelectListItems { get; set; }

        public Entities.CustomerChangeDailyReport GetDaily(int projectId, DateTime date)
        {
            return ReportData.FirstOrDefault(x => x.Project.Id == projectId && x.ReportDate.Date == date.Date);
        }
    }
}