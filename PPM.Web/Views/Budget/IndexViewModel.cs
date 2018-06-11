using System;
using System.Collections.Generic;
using System.Linq;
using Foundation.Data;
using PensionInsurance.Entities.DetailViews;
using PensionInsurance.Query;
using System.Web.Mvc;
using PensionInsurance.Entities;

namespace PensionInsurance.Web.Views.Budget
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
        public BudgetQuery Query { get; set; }
        public IEnumerable<SelectListItem> ProjectList { get; set; }
        public IEnumerable<DateTime> Months { get; set; }
        public IEnumerable<PurchaseProductCategory> ProductCategories { get;set ;}
        public Dictionary<DateTime,SpendDownBudget> MonthData { get; set; }

        public decimal GetBudgetCost(SpendDownBudget spendDownBudget, PurchaseProductCategory purchaseProductCategory)
        {
            decimal cost = 0.0m;
            spendDownBudget.SpendDownBudgetCategories.TryGetValue(purchaseProductCategory, out cost);
            return cost;
        }
    }
}