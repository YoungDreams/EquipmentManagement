using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PensionInsurance.Entities;
using PensionInsurance.Query;

namespace PensionInsurance.Web.Views.Reports.CustomerCheckInSummaryReport
{
    public class IndexViewModel
    {
        public CustomerCheckInSummaryReportQuery Query { get; set; }
        public IEnumerable<Entities.Reports.CustomerCheckInSummaryReport> CustomerCheckInSummaryReport { get; set; }
        public IEnumerable<SelectListItem> ProjectList { get; set; }
        public IEnumerable<Project> Projects { get; set; }
        public IEnumerable<Area> Cities => CustomerCheckInSummaryReport.GroupBy(x => x.City).Select(x => x.Key);
        public IEnumerable<ProjectType> ProjectTypes => Enum.GetValues(typeof(ProjectType)).Cast<ProjectType>();
        public IEnumerable<ManagementRegion> ManagementRegions => CustomerCheckInSummaryReport.GroupBy(x => x.ManagementRegion).Select(x => x.Key);

        public Entities.Reports.CustomerCheckInSummaryReport GetReport(int projectId, DateTime date)
        {
            date = date >= DateTime.Now.Date ? DateTime.Now.Date : date;
            return CustomerCheckInSummaryReport.FirstOrDefault(x => x.ProjectId == projectId && x.Month.Date == date.Date);
        }

        public Entities.Reports.CustomerCheckInSummaryReport GetReport(Area city, ManagementRegion managementRegion, int projectId, DateTime date)
        {
            date = date >= DateTime.Now.Date ? DateTime.Now.Date : date;
            return CustomerCheckInSummaryReport.FirstOrDefault(
                x => x.City == city && x.ManagementRegion == managementRegion && x.ProjectId == projectId &&
                     x.Month.Date == date.Date);
        }

        public List<Entities.Reports.CustomerCheckInSummaryReport> GetReport(List<int> projectIds, DateTime date)
        {
            date = date >= DateTime.Now.Date ? DateTime.Now.Date : date;
            if (projectIds == null || !projectIds.Any())
            {
                return CustomerCheckInSummaryReport.Where(x => x.Month.Date == date.Date).ToList();
            }
            return CustomerCheckInSummaryReport.Where(x => projectIds.Contains(x.ProjectId) && x.Month.Date == date.Date).ToList();
        }

        public List<Entities.Reports.CustomerCheckInSummaryReport> GetReport(ManagementRegion managementRegion, DateTime date)
        {
            date = date >= DateTime.Now.Date ? DateTime.Now.Date : date;
            return CustomerCheckInSummaryReport.Where(x => x.ManagementRegion == managementRegion && x.Month.Date == date.Date).ToList();
        }

        public List<Entities.Reports.CustomerCheckInSummaryReport> GetReport(Area city, ManagementRegion managementRegion, DateTime date)
        {
            date = date >= DateTime.Now.Date ? DateTime.Now.Date : date;
            return CustomerCheckInSummaryReport.Where(x => x.City == city && x.ManagementRegion == managementRegion &&
                                                           x.Month.Date == date.Date)
                .ToList();
        }

        public List<Entities.Reports.CustomerCheckInSummaryReport> GetReport(ProjectType type, DateTime date)
        {
            date = date >= DateTime.Now.Date ? DateTime.Now.Date : date;
            return CustomerCheckInSummaryReport.Where(x => x.ProjectType == type && x.Month.Date == date.Date).ToList();
        }

        public List<Entities.Reports.CustomerCheckInSummaryReport> GetReport(Area city, DateTime date)
        {
            date = date >= DateTime.Now.Date ? DateTime.Now.Date : date;
            return CustomerCheckInSummaryReport.Where(x => x.City == city && x.Month.Date == date.Date).ToList();
        }

        public string GetColor<T1, T2>(T1 number1, T2 number2) where T1 : struct where T2 : struct
        {
            var num1 = Convert.ToDouble(number1);
            var num2 = Convert.ToDouble(number2);
            var percent = num2 > 0 ? num1 / num2 : num1;
            return GetColor(percent * 100);
        }

        private string GetColor(double percent)
        {
            if (percent >= 100 && percent <= 105)
            {
                return "#8BB917";
            }
            
            if (percent >= 105)
            {
                return "#00B3D5";
            }

            if (percent >= 90 && percent < 100)
            {
                return "#F39700";
            }
            if (percent < 90)
            {
                return "#D7092F";
            }

            return string.Empty;
        }
    }
}