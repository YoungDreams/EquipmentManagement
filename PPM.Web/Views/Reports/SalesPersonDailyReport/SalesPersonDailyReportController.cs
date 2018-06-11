using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foundation.Core;
using Foundation.Data;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Query;
using PensionInsurance.Web.Views.Account;

namespace PensionInsurance.Web.Views.Reports.SalesPersonDailyReport
{
    public class SalesPersonDailyReportController : AuthorizedController
    {
        private readonly IProjectQueryService _projectQueryService;
        private readonly IFetcher _fetcher;
        private readonly ICommandService _commandService;
        private readonly ISalesPersonDailyReport _salesPersonDailyReport;

        public SalesPersonDailyReportController(ICommandService commandService, IFetcher fetcher, IProjectQueryService projectQueryService, ISalesPersonDailyReport salesPersonDailyReport)
        {
            _commandService = commandService;
            _fetcher = fetcher;
            _projectQueryService = projectQueryService;
            _salesPersonDailyReport = salesPersonDailyReport;
        }

        // GET: SalesPersonDailyReport
        public ActionResult Index(SalesProjectDailyReportQuery query = null)
        {
            var viewModel = new IndexViewModel
            {
                ProjectList = _projectQueryService.QueryAllValidByProjectFilter().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                ReportData = _salesPersonDailyReport.GetDailyReports(query),
                Query = query
            };
            if (query != null && (query.StartDate.HasValue && query.EndDate.HasValue))
            {
                if (query.ProjectIds == null)
                {
                    viewModel.Projects = _projectQueryService.QueryAllValidByProjectFilter().ToList();
                }
                else
                {
                    viewModel.Projects =
                        _projectQueryService.GetProjectList(string.Join(",", query.ProjectIds.Select(x => x))).ToList();
                }
            }
            return View("~/Views/Reports/SalesPersonDailyReport/Index.cshtml", viewModel);
        }

        public ActionResult WeeklySalesReport(SalesProjectDailyReportQuery query = null)
        {
            var data = _salesPersonDailyReport.GetDailyReports(query);

            var viewModel = new WeeklySalesReportViewModel()
            {
                ProjectList = _projectQueryService.QueryAllValidByProjectFilter().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                Query = query,
            };

            if (query != null && (query.StartDate.HasValue && query.EndDate.HasValue))
            {
                if (query.ProjectIds == null)
                {
                    viewModel.Projects = _projectQueryService.QueryAllValidByProjectFilter().ToList();
                }
                else
                {
                    viewModel.Projects = _projectQueryService.GetProjectList(string.Join(",", query.ProjectIds.Select(x => x))).ToList();
                }

                var weeks = GenerateWeeks(query.StartDate.Value, query.EndDate.Value).ToList();
                viewModel.Weeks = weeks;
                viewModel.WeeklyData = viewModel.Projects.ToDictionary(x => x,
               project => weeks.ToDictionary(week => week, week => new Entities.SalesProjectDailyReport()
               {
                   IncomingCallCount = data.Where(r => (r.Project.Id == project.Id || project.Id == 0) && week.Include(r.ReportDate))
                           .Sum(r => r.IncomingCallCount),
                   NewAddIncomingCallCount = data.Where(r => (r.Project.Id == project.Id || project.Id == 0) && week.Include(r.ReportDate))
                           .Sum(r => r.NewAddIncomingCallCount),
                   ValidIncomingCallCount = data.Where(r => (r.Project.Id == project.Id || project.Id == 0) && week.Include(r.ReportDate))
                           .Sum(r => r.ValidIncomingCallCount),
                   RoomCount =
                       data.Where(r => (r.Project.Id == project.Id || project.Id == 0) && week.Include(r.ReportDate))
                           .Sum(r => r.RoomCount),
                   ValidVisitCount = data.Where(r => (r.Project.Id == project.Id || project.Id == 0) && week.Include(r.ReportDate))
                           .Sum(r => r.ValidVisitCount),
                   IncomingPhoneToVisitCount = data.Where(r => (r.Project.Id == project.Id || project.Id == 0) && week.Include(r.ReportDate))
                           .Sum(r => r.IncomingPhoneToVisitCount),
                   VisitCount = data.Where(r => (r.Project.Id == project.Id || project.Id == 0) && week.Include(r.ReportDate))
                           .Sum(r => r.VisitCount),
               }));
            }

            return View("~/Views/Reports/SalesPersonDailyReport/WeeklySalesReport.cshtml", viewModel);
        }

        public ActionResult MonthlySalesReport(SalesProjectDailyReportQuery query = null)
        {
            var viewModel = new MonthlySalesReportViewModel()
            {
                ProjectList = _projectQueryService.QueryAllValidByProjectFilter().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                Query = query,

                ReportData = _salesPersonDailyReport.GetMonthlyDailyReports(query),
            };

            if (query.ProjectIds == null)
            {
                viewModel.Projects = _projectQueryService.QueryAllValidByProjectFilter().ToList();
            }
            else
            {
                viewModel.Projects =
                    _projectQueryService.GetProjectList(string.Join(",", query.ProjectIds.Select(x => x))).ToList();
            }
            return View("~/Views/Reports/SalesPersonDailyReport/MonthlySalesReport.cshtml", viewModel);
        }

        public ActionResult WeeklySalesUserReport(SalesUserDailyReportQuery query = null)
        {
            var viewModel = new WeeklySalesUserReportViewModel()
            {
                ProjectList = _projectQueryService.QueryAllValidByProjectFilter().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                Query = query,
            };

            if (query != null && (query.StartDate.HasValue && query.EndDate.HasValue))
            {
                var data = _salesPersonDailyReport.GetUserDailyReports(query);

                if (query.ProjectIds == null)
                {
                    viewModel.Projects = _projectQueryService.QueryAllValidByProjectFilter().ToList();
                }
                else
                {
                    viewModel.Projects = _projectQueryService.GetProjectList(string.Join(",", query.ProjectIds.Select(x => x))).ToList();
                }

                var weeks = GenerateWeeks(query.StartDate.Value, query.EndDate.Value).ToList();

                viewModel.Weeks = weeks;

                var users = _fetcher.Query<User>().Where(x => x.Roles.Any(r => r.RoleType == RoleType.销售人员) && x.Projects.Any(p => viewModel.Projects.Contains(p)));

                if (query.IsEnabled.HasValue)
                {
                    users = users.Where(x => x.IsEnabled == query.IsEnabled.Value);
                }
                viewModel.WeeklyData = users.ToDictionary(x => x,
                    user =>
                        weeks.ToDictionary(week => week,
                            week => data.Where(record => week.Include(record.ReportDate) && record.User.Id == user.Id)));


            }
            return View("~/Views/Reports/SalesPersonDailyReport/WeeklySalesUserReport.cshtml", viewModel);
        }

        public ActionResult MonthlySalesUserReport(SalesUserDailyReportQuery query = null)
        {
            var viewModel = new MonthlySalesUserReportViewModel()
            {
                ProjectList = _projectQueryService.QueryAllValidByProjectFilter().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                Query = query,
            };

            if (query != null && (query.StartDate.HasValue && query.EndDate.HasValue))
            {
                var data = _salesPersonDailyReport.GetMonthlyUserDailyReports(query);

                if (query.ProjectIds == null)
                {
                    viewModel.Projects = _projectQueryService.QueryAllValidByProjectFilter().ToList();
                }
                else
                {
                    viewModel.Projects = _projectQueryService.GetProjectList(string.Join(",", query.ProjectIds.Select(x => x))).ToList();
                }

                var months = GetMonths(query.StartDate.Value, query.EndDate.Value).ToList();

                viewModel.Months = months;

                var users = _fetcher.Query<User>().Where(x => x.Roles.Any(r => r.RoleType == RoleType.销售人员) && x.Projects.Any(p => viewModel.Projects.Contains(p)));

                if (query.IsEnabled.HasValue)
                {
                    users = users.Where(x => x.IsEnabled == query.IsEnabled.Value);
                }
                viewModel.MonthData = users.ToDictionary(x => x,
                    user =>
                        months.ToDictionary(month => month,
                            month => data.Where(record => record.ReportDate.Year == month.Year && record.ReportDate.Month == month.Month && record.User.Id == user.Id)));
            }
            return View("~/Views/Reports/SalesPersonDailyReport/MonthlySalesUserReport.cshtml", viewModel);
        }

        public ActionResult ExportMonthlySalesReport(ExportMonthlySalesProjectReportCommand command)
        {
            var result = _commandService.ExecuteFoResult(command);

            return Json(new
            {
                success = result.IsSucceed,
                redirect = $"{Url.Content("~/Attachments/Report/")}{result.FileName}"
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ExportWeeklySalesReport(ExportWeeklySalesProjectReportCommand command)
        {
            var result = _commandService.ExecuteFoResult(command);

            return Json(new
            {
                success = result.IsSucceed,
                redirect = $"{Url.Content("~/Attachments/Report/")}{result.FileName}"
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ExportSalesProjectDailyReport(ExportSalesProjectDailyReportCommand command)
        {
            var result = _commandService.ExecuteFoResult(command);

            return Json(new
            {
                success = result.IsSucceed,
                redirect = $"{Url.Content("~/Attachments/Report/")}{result.FileName}"
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ExportSalesUserDailyReport(ExportSalesUserDailyReportCommand command)
        {
            var result = _commandService.ExecuteFoResult(command);

            return Json(new
            {
                success = result.IsSucceed,
                redirect = $"{Url.Content("~/Attachments/Report/")}{result.FileName}"
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportWeeklySalesUserReport(ExportWeeklySalesUserReportCommand command)
        {
            var result = _commandService.ExecuteFoResult(command);

            return Json(new
            {
                success = result.IsSucceed,
                redirect = $"{Url.Content("~/Attachments/Report/")}{result.FileName}"
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ExportMonthlySalesUserReport(ExportMonthlySalesUserReportCommand command)
        {
            var result = _commandService.ExecuteFoResult(command);

            return Json(new
            {
                success = result.IsSucceed,
                redirect = $"{Url.Content("~/Attachments/Report/")}{result.FileName}"
            }, JsonRequestBehavior.AllowGet);
        }

        public List<DateTime> GetMonths(DateTime startTime, DateTime endTime)
        {
            try
            {
                List<DateTime> months = new List<DateTime>();
                DateTime c1 = Convert.ToDateTime(startTime.ToString("yyyy-MM"));
                DateTime c2 = Convert.ToDateTime(endTime.ToString("yyyy-MM"));
                if (c1 > c2)
                {
                    DateTime tmp = c1;
                    c1 = c2;
                    c2 = tmp;
                }
                while (c2 >= c1)
                {
                    months.Add(c1);
                    c1 = c1.AddMonths(1);
                }
                return months;
            }
            catch { return null; }
        }

        public ActionResult SalesUserDailyReport(SalesUserDailyReportQuery query = null)
        {
            var viewModel = new SalesUserDailyReportViewModel()
            {
                ProjectList = _projectQueryService.QueryAllValidByProjectFilter().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                Query = query,
            };
            if (query != null && (query.StartDate.HasValue && query.EndDate.HasValue))
            {
                var days = GetAllDays(query.StartDate.Value, query.EndDate.Value);
                viewModel.Days = days;
                var data = _salesPersonDailyReport.GetUserDailyReports(query);

                if (query.ProjectIds == null)
                {
                    viewModel.Projects = _projectQueryService.QueryAllValidByProjectFilter().ToList();
                }
                else
                {
                    viewModel.Projects = _projectQueryService.GetProjectList(string.Join(",", query.ProjectIds.Select(x => x))).ToList();
                }

                var users = _fetcher.Query<User>().Where(x => x.Roles.Any(r => r.RoleType == RoleType.销售人员) && x.Projects.Any(p => viewModel.Projects.Contains(p)));

                if (query.IsEnabled.HasValue)
                {
                    users = users.Where(x => x.IsEnabled == query.IsEnabled.Value);
                }
                viewModel.DayData = users.ToDictionary(x => x,
                    user =>
                        days.ToDictionary(day => day,
                            day => data.FirstOrDefault(record => record.ReportDate == day && record.User.Id == user.Id)));
            }
            return View("~/Views/Reports/SalesPersonDailyReport/SalesUserDailyReport.cshtml", viewModel);
        }

        private List<DateTime> GetAllDays(DateTime ds, DateTime de)
        {
            List<DateTime> listDays = new List<DateTime>();

            for (DateTime day = ds; day.CompareTo(de) <= 0; day = day.AddDays(1))
            {
                listDays.Add(day.Date);
            }
            return listDays;
        }

        private int GetWeekOfYear(DateTime dt)
        {
            int firstWeekend = 7 - Convert.ToInt32(new DateTime(dt.Year, 1, 1).DayOfWeek);

            int currentDay = dt.DayOfYear;

            return Convert.ToInt32(Math.Ceiling((currentDay - firstWeekend) / 7.0)) + 1;
        }
        private IEnumerable<Week> GenerateWeeks(DateTime startDate, DateTime endDate)
        {
            var jan1 = startDate;

            var startOfFirstWeek = jan1.AddDays(1 - (int)(jan1.DayOfWeek));

            int weekOfYear = GetWeekOfYear(startOfFirstWeek);

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
                        Number = weekOfYear + i
                    }).ToList();

            if (weeks.Any())
            {
                if (weeks[0].Start.Year != startDate.Year)
                {
                    weeks[0].Start = jan1;
                }
            }

            return weeks;
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
    }
}