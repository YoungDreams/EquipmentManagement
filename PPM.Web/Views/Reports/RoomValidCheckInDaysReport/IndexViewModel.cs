using System.Collections.Generic;
using System.Web.Mvc;
using PensionInsurance.Entities;
using PensionInsurance.Query;

namespace PensionInsurance.Web.Views.Reports.RoomValidCheckInDaysReport
{
    public class IndexViewModel
    {
        public RoomValidCheckInDaysReportQuery Query { get; set; }
        public IEnumerable<RoomValidCheckInDaysReportDetail> RoomValidCheckInDays { get; set; }
        public IEnumerable<SelectListItem> ProjectList { get; set; }
    }
}