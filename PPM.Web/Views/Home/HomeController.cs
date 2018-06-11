using System;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using Foundation.Core;
using NLog.Fluent;
using PensionInsurance.Query;
using PensionInsurance.Shared;
using PensionInsurance.Web.Views.Account;

namespace PensionInsurance.Web.Views.Home
{
    public class HomeController : AuthorizedController
    {
        private readonly IReportQueryService _reportQueryService;
        private readonly IUserBacklogQueryService _userBacklogQueryService;
        private readonly IHomeQueryService _homeQueryService;
        private readonly IUserNotificationQueryService _noticeQueryService;
        private readonly IConsultingQueryService _consultingQueryService;
        public HomeController(IUserBacklogQueryService userBacklogQueryService, IReportQueryService reportQueryService, IHomeQueryService homeQueryService, IUserNotificationQueryService noticeQueryService, IConsultingQueryService consultingQueryService)
        {
            _userBacklogQueryService = userBacklogQueryService;
            _reportQueryService = reportQueryService;
            _homeQueryService = homeQueryService;
            _noticeQueryService = noticeQueryService;
            _consultingQueryService = consultingQueryService;
        }

        public ActionResult Index()
        {
            if (WebAppContext.Current.User.HasModule(Entities.ModuleType.座席接电管理)&& !WebAppContext.Current.User.RoleTypeIs(Entities.RoleType.超级管理员))
            {
                return RedirectToAction("Index", "OperatorConsulting");
            }

            var viewModel = new IndexViewModel();
            var projectId = WebAppContext.Current.User.Projects.Any() ? WebAppContext.Current.User.Projects.First().Id : new int?();
            viewModel.DailyReport =
                _reportQueryService.GetDailyReports(new SalesDailyReportQuery
                {
                    StartDate = DateTime.Now.Date,
                    EndDate = DateTime.Now.Date
                });
            
            viewModel.IneffectiveContracts = _homeQueryService.IneffectiveContracts();
            viewModel.IneffectiveContractsByToday = _homeQueryService.IneffectiveContractsByToday();
            viewModel.InvalidContracts = _homeQueryService.InvalidContracts();
            viewModel.InvalidContractsByToday = _homeQueryService.InvalidContractsByToday();
            viewModel.InvalidContractsBydays = _homeQueryService.InvalidContractsByDays();

            viewModel.CustomerChangeDesc = _homeQueryService.CustomerChangeDesc();
            viewModel.IneffectiveAddtionals = _homeQueryService.IneffectiveAddtionals();
            viewModel.FirstDayOfNowMonth= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            viewModel.CustomerAccountArrearages = _homeQueryService.CustomerAccountArrearages();
            viewModel.ConsultingDetails = _consultingQueryService.QueryWow(1, 500, new ConsultingQuery { IsWow = true, ProjectId = projectId });

            return View(viewModel);
        }

        public PartialViewResult NavHeader()
        {
            NavHeaderViewModel viewModel = new NavHeaderViewModel();
            viewModel.UserBacklogs =
                _userBacklogQueryService.GetCurrentUnDoUserBacklogs().ToList();
            viewModel.UserNotices = _noticeQueryService.GetCurrentUnDoUserNotifications(WebAppContext.Current.User.Id).ToList();

            return PartialView("_NavHeader", viewModel);
        }

        public ActionResult NoPermission()
        {
            return View("NoPermission");
        }
    }
}