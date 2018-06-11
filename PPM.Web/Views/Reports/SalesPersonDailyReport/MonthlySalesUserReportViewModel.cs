using System;
using System.Collections.Generic;
using System.Web.Mvc;
using PensionInsurance.Entities;
using PensionInsurance.Query;

namespace PensionInsurance.Web.Views.Reports.SalesPersonDailyReport
{
    public class MonthlySalesUserReportViewModel
    {
        public IEnumerable<SelectListItem> ProjectList { get; set; }
        public SalesUserDailyReportQuery Query { get; set; }
        public IEnumerable<DateTime> Months { get; set; }
        public IEnumerable<Project> Projects { get; set; }
        public Dictionary<User, Dictionary<DateTime, IEnumerable<SalesUserDailyReport>>> MonthData { get; set; }
    }
}