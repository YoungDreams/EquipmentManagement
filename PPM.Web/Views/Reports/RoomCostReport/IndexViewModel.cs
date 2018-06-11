using System.Collections.Generic;
using System.Web.Mvc;
using PensionInsurance.Query;

namespace PensionInsurance.Web.Views.Reports.RoomCostReport
{
    public class IndexViewModel
    {
        public IEnumerable<Entities.Reports.RoomCostReport> RoomCostReport { get; set; }
        public RoomCostReportQuery Query { get; set; }
        public IEnumerable<SelectListItem> ProjectList { get; set; }
    }
}