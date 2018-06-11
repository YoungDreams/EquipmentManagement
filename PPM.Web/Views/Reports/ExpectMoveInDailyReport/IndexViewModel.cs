using System;
using System.Collections.Generic;
using System.Linq;
using PensionInsurance.Entities;
using PensionInsurance.Query;

namespace PensionInsurance.Web.Views.Reports.ExpectMoveInDailyReport
{
    public class IndexViewModel
    {
        public IList<Project> Projects { get; set; }
        public IList<Entities.DailyReport> ReportData { get; set; }

        public SalesDailyReportQuery Query { get; set; }

        public Entities.DailyReport GetDaily(int projectId, DateTime date)
        {
            return ReportData.FirstOrDefault(x => x.Project.Id == projectId && x.ReportDate.Date == date.Date);
        }
    }
}