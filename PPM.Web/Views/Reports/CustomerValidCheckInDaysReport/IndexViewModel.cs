using System.Collections.Generic;
using System.Web.Mvc;
using Foundation.Data;
using PensionInsurance.Entities;
using PensionInsurance.Query;

namespace PensionInsurance.Web.Views.Reports.CustomerValidCheckInDaysReport
{
    public class IndexViewModel
    {
        public CustomerValidCheckInDaysReportQuery Query { get; set; }
        public IEnumerable<CustomerValidCheckInDaysReportDetail> CustomerValidCheckInDays { get; set; }
        public IEnumerable<SelectListItem> ProjectList { get; set; }
    }
}