using System.Collections.Generic;
using System.Web.Mvc;
using PensionInsurance.Query;

namespace PensionInsurance.Web.Views.Reports.CustomerCheckInRoomAndMealsComparisonReport
{
    public class IndexViewModel
    {
        public CustomerCheckInRoomAndMealsComparisonReportQuery Query { get; set; }
        public IEnumerable<Entities.Reports.CustomerCheckInRoomAndMealsComparisonReport> CustomerCheckInRoomAndMealsComparisonReport { get; set; }
        public IEnumerable<SelectListItem> ProjectList { get; set; }
    }
}