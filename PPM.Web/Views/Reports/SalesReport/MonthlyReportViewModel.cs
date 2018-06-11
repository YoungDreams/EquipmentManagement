using System;
using System.Collections.Generic;
using PensionInsurance.Entities;
using PensionInsurance.Query;

namespace PensionInsurance.Web.Views.Reports.SalesReport
{
    public class MonthlyReportViewModel
    {
        public IDictionary<int, List<int>> YearAndMonth { get; set; }
        public MonthlyReportQuery Query { get; set; }
        public IEnumerable<Project> Projects { get; set; }
        public Dictionary<Project, Dictionary<DateTime, MonthlyReport>> Reports { get; set; }
    }
}