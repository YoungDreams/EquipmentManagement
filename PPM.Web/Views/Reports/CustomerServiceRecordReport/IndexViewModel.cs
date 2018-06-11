using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PensionInsurance.Query;

namespace PensionInsurance.Web.Views.Reports.CustomerServiceRecordReport
{
    public class IndexViewModel
    {
        public CustomerServiceRecordReportQuery Query { get; set; }
        public IEnumerable<SelectListItem> ProjectList { get; set; }
        public IEnumerable<Entities.Reports.CustomerServiceRecordReport> CustomerServiceRecords { get; set; }
    }
}