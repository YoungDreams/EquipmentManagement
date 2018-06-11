using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PensionInsurance.Entities;
using PensionInsurance.Query;
using PensionInsurance.Web.Views.Reports.SalesReport;

namespace PensionInsurance.Web.Views.Reports.SalesPersonDailyReport
{
    public class WeeklySalesUserReportViewModel
    {
        public IEnumerable<SelectListItem> ProjectList { get; set; }
        public SalesUserDailyReportQuery Query { get; set; }
        public IEnumerable<Project> Projects { get; set; }
        public IEnumerable<SalesPersonDailyReportController.Week> Weeks { get; set; }
        public Dictionary<User, Dictionary<SalesPersonDailyReportController.Week, IEnumerable<SalesUserDailyReport>>> WeeklyData { get; set; }
    }
}