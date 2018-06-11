using System.Linq;
using System.Web.Mvc;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Query;
using PensionInsurance.Web.Views.Account;

namespace PensionInsurance.Web.Views.Reports.CustomerSalesChannelReport
{
    public class CustomerSalesChannelReportController : AuthorizedController
    {
        private readonly ICustomerSalesChannelReportQueryService _customerSalesChannelReportQueryService;
        private readonly IProjectQueryService _projectQueryService;
        private readonly ICommandService _commandService;
        private readonly ISettingQueryService _settingQueryService;
        public CustomerSalesChannelReportController(ICommandService commandService, IProjectQueryService projectQueryService, ICustomerSalesChannelReportQueryService customerSalesChannelReportQueryService, ISettingQueryService settingQueryService)
        {
            _commandService = commandService;
            _projectQueryService = projectQueryService;
            _customerSalesChannelReportQueryService = customerSalesChannelReportQueryService;
            _settingQueryService = settingQueryService;
        }

        public ActionResult Index(CustomerSalesChannelReportQuery query = null)
        {
            IndexViewModel viewModel = new IndexViewModel
            {
                Query = query,
                CustomerSalesChannelReport = _customerSalesChannelReportQueryService.Query(query),
                ProjectList = _projectQueryService.QueryAllValidByProjectFilter()
                    .Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }),
                Projects = _projectQueryService.QueryAllValidByProjectFilter(),
                Channels = _settingQueryService.GetSettingsByType(SettingType.获知渠道)
            };

            if (query != null)
            {
                if (query.ProjectIds != null && query.ProjectIds.Any())
                {
                    viewModel.Projects = viewModel.Projects.Where(x => query.ProjectIds.Contains(x.Id));
                }
                if (query.ProjectType.HasValue)
                {
                    viewModel.Projects = viewModel.Projects.Where(x => x.ProjectType == query.ProjectType);
                }
            }

            return View("~/Views/Reports/CustomerSalesChannelReport/Index.cshtml", viewModel);
        }

        public ActionResult Export(ExportCustomerSalesChannelReportCommand command)
        {
            var result = _commandService.ExecuteFoResult(command);

            return Json(new
            {
                success = result.IsSucceed,
                redirect = $"{Url.Content("~/Attachments/Report/")}{result.FileName}"
            }, JsonRequestBehavior.AllowGet);
        }
       
    }
}