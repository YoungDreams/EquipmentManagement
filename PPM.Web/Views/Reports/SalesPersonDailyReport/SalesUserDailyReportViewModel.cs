using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PensionInsurance.Entities;
using PensionInsurance.Query;

namespace PensionInsurance.Web.Views.Reports.SalesPersonDailyReport
{
    public class SalesUserDailyReportViewModel
    {
        public IEnumerable<SelectListItem> ProjectList { get; set; }
        public SalesUserDailyReportQuery Query { get; set; }
        public IEnumerable<DateTime> Days { get; set; }
        public IEnumerable<Project> Projects { get; set; }
        public Dictionary<User, Dictionary<DateTime, Entities.SalesUserDailyReport>> DayData { get; set; }
    }
}