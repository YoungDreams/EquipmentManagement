using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Foundation.Core;
using Foundation.Data;
using PensionInsurance.Entities;
using PensionInsurance.Query;
using PensionInsurance.Shared;
using PensionInsurance.Web.Views.Account;

namespace PensionInsurance.Web.Views.Reports.SalesReport
{
    public class SalesReportController : AuthorizedController
    {
        private readonly IReportQueryService _reportQueryService;
        private readonly IFetcher _fetcher;

        public SalesReportController(IReportQueryService reportQueryService, IFetcher fetcher)
        {
            _reportQueryService = reportQueryService;
            _fetcher = fetcher;
        }

        public ActionResult ProjectSalesPersonReport(SalesPersonReportQuery query)
        {
            var now = DateTime.Now;

            query.StartDate = new DateTime(now.Year, 1, 1);
            query.EndDate = now;


            var data = _reportQueryService.GetSalesPersonReport(query);
            var weeks = GenerateWeeks(query.StartDate.Year, query.EndDate);

            var projects = WebAppContext.Current.User.GetAllowedProjects().Where(x => !query.ProjectId.HasValue || query.ProjectId == x.Id).ToList();
            if (projects.Count > 1)
            {
                projects.Insert(0, new Project());
            }
            
            var viewModel = new ProjectSalesPersonReportViewModel
            {
                Projects = projects,
                // ReSharper disable PossibleMultipleEnumeration
                Weeks = weeks,
                WeeklyData = projects.ToDictionary(x => x, project => weeks.ToDictionary(week => week, week => new SalesPersonReportData
                {
                    IncomingPhoneCount = data.Where(r => (r.ProjectId == project.Id || project.Id == 0) && week.Include(r.ReportDate)).Sum(r => r.IncomingPhoneCount),
                    IncomingPhoneToVisitCount = data.Where(r => (r.ProjectId == project.Id || project.Id == 0) && week.Include(r.ReportDate)).Sum(r => r.IncomingPhoneToVisitCount),
                    RoomCount = data.Where(r => (r.ProjectId == project.Id || project.Id == 0) && week.Include(r.ReportDate)).Sum(r => r.RoomCount),
                    ValidIncomingPhoneCount = data.Where(r => (r.ProjectId == project.Id || project.Id == 0) && week.Include(r.ReportDate)).Sum(r => r.ValidIncomingPhoneCount),
                    ValidVisitCount = data.Where(r => (r.ProjectId == project.Id || project.Id == 0) && week.Include(r.ReportDate)).Sum(r => r.ValidVisitCount),
                    VisitCount = data.Where(r => (r.ProjectId == project.Id || project.Id == 0) && week.Include(r.ReportDate)).Sum(r => r.VisitCount),
                })),
                Query = query
            };

            return View("~/Views/Reports/SalesReport/ProjectSalesPersonReport.cshtml", viewModel);
        }
        
        public ActionResult SalesPersonReport(SalesPersonReportQuery query)
        {
            var now = DateTime.Now;

            query.StartDate = new DateTime(now.Year, 1, 1);
            query.EndDate = now;


            var projects = WebAppContext.Current.User.GetAllowedProjects();

            if (!query.ProjectId.HasValue)
            {
                query.ProjectId = projects.First().Id;
            }

            var data = _reportQueryService.GetSalesPersonReport(query);
            var weeks = GenerateWeeks(query.StartDate.Year, query.EndDate);

            var users = _fetcher.Query<User>().Where(x => x.Roles.Any(r => r.RoleType == RoleType.销售人员) && x.Projects.Any(p => p.Id == query.ProjectId.Value));

            var viewModel = new SalesPersonReportViewModel
            {
                Projects = projects,
                Query = query,
                Weeks = weeks,
                WeeklyData = users.ToDictionary(x => x, user => weeks.ToDictionary(week => week, week => data.Where(record => week.Include(record.ReportDate) && record.UserId == user.Id)))
            };

            return View("~/Views/Reports/SalesReport/SalesPersonReport.cshtml", viewModel);
        }

        private IEnumerable<Week> GenerateWeeks(int year, DateTime endDate)
        {
            var jan1 = new DateTime(year, 1, 1);
            //beware different cultures, see other answers
            var startOfFirstWeek = jan1.AddDays(1 - (int)(jan1.DayOfWeek));
            
            var weeks =
                Enumerable
                    .Range(0, 54)
                    .Select(i => new
                    {
                        weekStart = startOfFirstWeek.AddDays(i * 7)
                    })
                    .TakeWhile(x => x.weekStart.Year <= jan1.Year && x.weekStart < endDate)
                    .Select(x => new
                    {
                        x.weekStart,
                        weekFinish = x.weekStart.AddDays(6)
                    })
                    .SkipWhile(x => x.weekFinish < jan1.AddDays(1))
                    .Select((x, i) => new Week
                    {
                        Start = x.weekStart,
                        End = x.weekFinish,
                        Number = i + 1
                    }).ToList();

            if (weeks[0].Start.Year != year)
            {
                weeks[0].Start = jan1;
            }
            return weeks;
        }

        public ActionResult MonthlyReport(MonthlyReportQuery query)
        {
            var now = DateTime.Now;

            query.StartDate = query.StartDate ?? new DateTime(now.Year, 1, 1);
            query.EndDate = query.EndDate ?? now;
            var range = new DateRange(query.StartDate.Value, query.EndDate.Value);

            var viewModel = new MonthlyReportViewModel
            {
                Projects = WebAppContext.Current.User.GetAllowedProjects(),
                Query = query,
                YearAndMonth = range.GroupByYearForMonth(),
                Reports = _reportQueryService.GetMonthlyReport(query).ToDictionary(x => x.Key, x => x.Value.ToDictionary(report => report.Month))
            };

            return View("~/Views/Reports/SalesReport/MonthlyReport.cshtml", viewModel);
        }
    }

    public class DailyReportViewModel
    {
        public SalesDailyReportData ReportData { get; set; }
        public SalesDailyReportQuery Query { get; set; }
    }


    public class Week
    {
        public DateTime Start;
        public DateTime End;
        public int Number;

        public override bool Equals(object obj)
        {
            var anotherWeek = obj as Week;

            return anotherWeek != null && Start == anotherWeek.Start;
        }

        public override int GetHashCode()
        {
            return Start.GetHashCode();
        }

        public bool Include(DateTime date)
        {
            return Start <= date.Date && date.Date <= End;
        }

        public string GetDateRangeString()
        {
            return $"{Start.ToString("yyyy-MM-dd")} - {End.ToString("yyyy-MM-dd")}";
        }
    }



    public class SalesPersonReportViewModel
    {
        public SalesPersonReportQuery Query { get; set; }
        public IEnumerable<User> Users { get; set; }
        public IEnumerable<Week> Weeks { get; set; }
        public Dictionary<User, Dictionary<Week, IEnumerable<SalesPersonReportData>>> WeeklyData { get; set; }
        public IEnumerable<Project> Projects { get; set; }
    }

    public class ProjectSalesPersonReportViewModel
    {
        public IEnumerable<Project> Projects { get; set; }
        public IEnumerable<Week> Weeks { get; set; }
        public Dictionary<Project, Dictionary<Week, SalesPersonReportData>> WeeklyData { get; set; }
        public SalesPersonReportQuery Query { get; set; }
    }
}