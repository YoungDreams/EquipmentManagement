using System.Collections.Generic;
using System.Web.Mvc;
using Foundation.Data;
using PensionInsurance.Entities;
using PensionInsurance.Query;

namespace PensionInsurance.Web.Views.Reports.CustomerLeaveReport
{
    public class IndexViewModel
    {
        public CustomerLeaveReportQuery Query { get; set; }
        public IEnumerable<SelectListItem> ProjectList { get; set; }
        public PagedData<CustomerLeave> CustomerLeaves { get; set; }
    }
}