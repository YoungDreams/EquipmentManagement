using System.Collections.Generic;
using System.Web.Mvc;
using PensionInsurance.Query;

namespace PensionInsurance.Web.Views.InvestmentProject
{
    public class MapViewModel
    {
        public InvestmentProjectQuery Query { get; set; }
        public IEnumerable<Entities.InvestmentProject> Items { get; set; }
        public List<SelectListItem> Types { get; set; }
        public List<SelectListItem> Status { get; set; }
    }
}