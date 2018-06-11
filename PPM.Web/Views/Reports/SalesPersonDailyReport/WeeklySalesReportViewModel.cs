using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PensionInsurance.Entities;
using PensionInsurance.Query;

namespace PensionInsurance.Web.Views.Reports.SalesPersonDailyReport
{
    public class WeeklySalesReportViewModel
    {
        public IEnumerable<SelectListItem> ProjectList { get; set; }
        public SalesProjectDailyReportQuery Query { get; set; }
        public IEnumerable<Project> Projects { get; set; }
        public IEnumerable<SalesPersonDailyReportController.Week> Weeks { get; set; }
        public Dictionary<Project, Dictionary<SalesPersonDailyReportController.Week, Entities.SalesProjectDailyReport>> WeeklyData { get; set; }


        //public Entities.SalesPersonDailyReport GetWeekDaily(int projectId, DateTime date)
        //{
        //   // return ReportData.FirstOrDefault(x => x.Project.Id== projectId && x.ReportDate.Date == date.Date);
        //}
    }
}