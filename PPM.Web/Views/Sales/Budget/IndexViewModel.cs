using System;
using System.Collections.Generic;
using System.Linq;
using Foundation.Data;
using PensionInsurance.Entities.DetailViews;
using PensionInsurance.Query;
using System.Web.Mvc;
using PensionInsurance.Entities;

namespace PensionInsurance.Web.Views.Sales.Budget
{
    public class IndexViewModel
    {
        private readonly UrlHelper _urlHelper;
        public IndexViewModel(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public SalesBudgetQuery Query { get; set; }
        public IEnumerable<SelectListItem> ProjectList { get; set; }
        public IEnumerable<DateTime> Months { get; set; }
        public Dictionary<DateTime,SalesBudget> MonthData { get; set; }
    }
}