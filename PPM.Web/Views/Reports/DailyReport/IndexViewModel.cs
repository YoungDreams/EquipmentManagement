using System;
using System.Collections.Generic;
using System.Linq;
using Foundation.Data;
using PensionInsurance.Entities;
using PensionInsurance.Query;

namespace PensionInsurance.Web.Views.Reports.DailyReport
{
    public class IndexViewModel
    {
        private IFetcher _fetcher;
        public IndexViewModel(IFetcher fetcher)
        {
            _fetcher = fetcher;
        }


        public IList<Project> Projects { get; set; }
        public IList<Entities.DailyReport> ReportData { get; set; }

        public SalesDailyReportQuery Query { get; set; }

        public Entities.DailyReport GetDaily(int projectId, DateTime date)
        {
            return ReportData.FirstOrDefault(x => x.Project.Id == projectId && x.ReportDate.Date == date.Date);
        }
        public List<Entities.DailyReport> GetTotalDaily(DateTime date)
        {
            return ReportData.Where(x => x.ReportDate.Date == date.Date).ToList();
        }

        private IEnumerable<SalesBudget> GetSalesBudgets()
        {
            return _fetcher.Query<SalesBudget>().ToList();
        }

        public double GetMonthlyTotal(int projectId ,DateTime lastDay)
        {
            var monthBuget = GetSalesBudgets().FirstOrDefault(x =>
                    x.Project.Id == projectId && x.BudgetMonth == new DateTime(lastDay.Year, lastDay.Month, 1));
            return monthBuget?.BudgetMovedInRoomCount ?? 0;
        }

        public double GetYearTotal(int projectId, DateTime lastDay)
        {
            var yearBuget = GetSalesBudgets().FirstOrDefault(x =>
                    x.Project.Id == projectId && x.BudgetMonth == new DateTime(lastDay.Year, 12, 1));
            return yearBuget?.BudgetMovedInRoomCount ?? 0;
        }
    }
}