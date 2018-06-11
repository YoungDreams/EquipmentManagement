using Foundation.Data;
using PensionInsurance.Query;
using PensionInsurance.Web.Views.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PensionInsurance.Web.Views.Reports.CustomerDistributionReport
{
    public class CustomerDistributionReportController : AuthorizedController
    {
        private readonly IFetcher _fetcher;
        private readonly IProjectQueryService _projectQueryService;
        private readonly ICustomerDistributionReportQueryService _customerDistributionReportQueryService;
        public CustomerDistributionReportController(IFetcher fetcher,IProjectQueryService projectQueryService, ICustomerDistributionReportQueryService customerDistributionReportQueryService)
        {
            _projectQueryService = projectQueryService;
            _customerDistributionReportQueryService = customerDistributionReportQueryService;
            _fetcher = fetcher;
        }

        public ActionResult Index(CustomerDistributionReportQuery query = null)
        {
            IndexViewModel viewModel = new IndexViewModel
            {
                Query = query,
                Addresses = _customerDistributionReportQueryService.Query(query),
                ProjectList = _projectQueryService.QueryAllValidByProjectFilter().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            return View("~/Views/Reports/CustomerDistributionReport/Index.cshtml", viewModel);
        }
    }
}