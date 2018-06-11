using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PensionInsurance.Entities;
using PensionInsurance.Entities.Reports;
using PensionInsurance.Query;

namespace PensionInsurance.Web.Views.Reports.CustomerSalesChannelReport
{
    public class IndexViewModel
    {
        public CustomerSalesChannelReportQuery Query { get; set; }
        public IEnumerable<Entities.Reports.CustomerSalesChannelReport> CustomerSalesChannelReport { get; set; }
        public IEnumerable<SelectListItem> ProjectList { get; set; }
        public IEnumerable<Project> Projects { get; set; }
        public IEnumerable<Setting> Channels { get; set; }
        
        public IEnumerable<Entities.Reports.CustomerSalesChannelReport> GetReports(List<int> projectIds,string channel, DateTime date)
        {
            return CustomerSalesChannelReport.Where(
                    x => projectIds.Contains(x.Project.Id) && x.Month.Date == date.Date && x.Channel == channel)
                .ToList();
        }

        public Entities.Reports.CustomerSalesChannelReport GetReport(int projectId, string channel, DateTime date)
        {
            return CustomerSalesChannelReport.FirstOrDefault(
                x => x.Project.Id == projectId && x.Month.Date == date.Date && x.Channel == channel);
        }

        public IEnumerable<Entities.Reports.CustomerSalesChannelReport> GetReports(List<int> projectIds, string channel, DateTime startDate, DateTime endDate)
        {
            return CustomerSalesChannelReport.Where(
                    x => projectIds.Contains(x.Project.Id) && x.Month.Date >= startDate && x.Month.Date <= endDate &&
                         x.Channel == channel)
                .ToList();
        }

        public IEnumerable<Entities.Reports.CustomerSalesChannelReport> GetReports(int projectId, string channel, DateTime startDate, DateTime endDate)
        {
            return CustomerSalesChannelReport.Where(
                    x => x.Project.Id == projectId && x.Month.Date >= startDate && x.Month.Date <= endDate &&
                         x.Channel == channel)
                .ToList();
        }

        public int GetRowSpanCount(IList<int> projectIds, DateTime startDate, DateTime endDate)
        {
            var count = 0;
            foreach (var channel in this.Channels)
            {
                var lstReports= CustomerSalesChannelReport.Where(
                        x => projectIds.Contains(x.Project.Id) && x.Month.Date >= startDate && x.Month.Date <= endDate &&
                             x.Channel == channel.Name)
                    .ToList();
                if (lstReports.Sum(x => x.Total) != 0)
                {
                    count++;
                }
            }
            return count;
        }

        public int GetTotal(IList<int> projectIds, string channel, DateTime startDate, DateTime endDate)
        {
            var lstReports = CustomerSalesChannelReport.Where(
                    x => projectIds.Contains(x.Project.Id) && x.Month.Date >= startDate && x.Month.Date <= endDate &&
                         x.Channel == channel)
                .ToList();
            return lstReports.Sum(x => x.Total);
        }

        public int GetRowSpanCount(int projectId,DateTime startDate, DateTime endDate)
        {
            var count = 0;
            foreach (var channel in this.Channels)
            {
                var lstReports = CustomerSalesChannelReport.Where(
                        x => x.Project.Id == projectId && x.Month.Date >= startDate && x.Month.Date <= endDate &&
                             x.Channel == channel.Name)
                    .ToList();
                if (lstReports.Sum(x => x.Total) != 0)
                {
                    count++;
                }
            }
            return count;
        }

        public int GetTotal(int projectId, string channel, DateTime startDate, DateTime endDate)
        {
            var lstReports = CustomerSalesChannelReport.Where(
                    x => x.Project.Id == projectId && x.Month.Date >= startDate && x.Month.Date <= endDate &&
                         x.Channel == channel)
                .ToList();
            return lstReports.Sum(x => x.Total);
        }
    }
}