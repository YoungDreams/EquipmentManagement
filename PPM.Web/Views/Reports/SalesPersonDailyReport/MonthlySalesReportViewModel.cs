using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PensionInsurance.Entities;
using PensionInsurance.Query;

namespace PensionInsurance.Web.Views.Reports.SalesPersonDailyReport
{
    public class MonthlySalesReportViewModel
    {
        public IEnumerable<SelectListItem> ProjectList { get; set; }
        public SalesProjectDailyReportQuery Query { get; set; }
        public IEnumerable<Project> Projects { get; set; }
        public IDictionary<int, List<int>> YearAndMonth { get; set; }
        public IList<Entities.SalesProjectDailyReport> ReportData { get; set; }
        public MonthlySalesProjectDailyReport GetMonthlySalesPersonDailyReport(int projectId, DateTime date)
        {
            var results = ReportData.Where(x => x.ReportDate.Year == date.Year && x.ReportDate.Month == date.Month && x.Project.Id == projectId).ToList();

            var monthlySalesPersonDailyReport = new MonthlySalesProjectDailyReport
            {
                TotalIncomingCallCount = results.Sum(s=>s.IncomingCallCount),
                TotalNewAddIncomingCallCount = results.Sum(s => s.NewAddIncomingCallCount),
                TotalValidIncomingCallCount = results.Sum(s => s.ValidIncomingCallCount),
                TotalVisitCount = results.Sum(s => s.VisitCount),
                TotalIncomingPhoneToVisitCount = results.Sum(s => s.IncomingPhoneToVisitCount),
                TotalValidVisitCount = results.Sum(s => s.ValidVisitCount),
                TotalRoomCount = results.Sum(s => s.RoomCount)
            };
            return monthlySalesPersonDailyReport;
        }
    }

    public class MonthlySalesProjectDailyReport
    {
        /// <summary>
        /// 总来电
        /// </summary>
        public virtual int TotalIncomingCallCount { get; set; }
        /// <summary>
        /// 新增来电
        /// </summary>
        public virtual int TotalNewAddIncomingCallCount { get; set; }
        /// <summary>
        /// 有效来电
        /// </summary>
        public virtual int TotalValidIncomingCallCount { get; set; }
        public int TotalVisitCount { get; set; }
        public int TotalValidVisitCount { get; set; }
        public int TotalIncomingPhoneToVisitCount { get; set; }
        public double TotalRoomCount { get; set; }
    }
}