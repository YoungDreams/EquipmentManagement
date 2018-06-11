using System;
using System.Collections.Generic;
using System.Linq;
using Foundation.Data;
using PensionInsurance.Entities.DetailViews;
using PensionInsurance.Query;
using System.Web.Mvc;
using PensionInsurance.Entities;

namespace PensionInsurance.Web.Views.Purchase
{
    public class IndexViewModel
    {
        private readonly UrlHelper _urlHelper;
        public IndexViewModel(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }
        public string ProjectName { get; set; }
        public BudgetDetailQuery Query { get; set; }
        public IEnumerable<SelectListItem> ProjectList { get; set; }
        public List<SpendingStatistics> MonthData { get; set; }
    }
}