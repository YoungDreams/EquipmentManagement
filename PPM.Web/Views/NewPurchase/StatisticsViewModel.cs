using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PensionInsurance.Entities;
using PensionInsurance.Entities.Reports;
using PensionInsurance.Query;
using PensionInsurance.Web.Views.Purchase;

namespace PensionInsurance.Web.Views.NewPurchase
{
    public class StatisticsViewModel
    {
        public IEnumerable<SelectListItem> ProjectList { get; set; }
        public IEnumerable<SelectListItem> DepartmentList { get; set; }
        public PurchaseStatisticsQuery Query { get; set; }
        public ProductCategoryTreeView ProductCategoryTreeView { get; set; }
        public string CategoryText { get; set; }
        public IEnumerable<SelectListItem> ProductList { get; set; }

        public PurchaseStatistics Result { get; set; }

        public int GetDepartmentRows(string departmentName)
        {
            return Result.StatisticsOrderItems.Count(x => x.DepartmentName == departmentName);
        }

        public ProjectPurchaseTotal GetProjectpurchaseTotalAmount( string projectName)
        {
            var projectPurchaseTotal = new ProjectPurchaseTotal();
            if (string.IsNullOrWhiteSpace(projectName))
            {
                return projectPurchaseTotal;
            }
            var items = Result.StatisticsOrderItems.Where(x => x.Projects.Any(z => z.ProjectName == projectName)).ToList();
            projectPurchaseTotal.TotalAmount= items.Sum(item => item.Projects.First(x => x.ProjectName == projectName).Amount);

            projectPurchaseTotal.TotalStockAmount = items.Sum(item => item.Projects.First(x => x.ProjectName == projectName).Stock * item.PurchasePrice);
            return projectPurchaseTotal;
        }
        
        public class ProjectPurchaseTotal
        {
            public decimal TotalAmount { get; set; }
            public decimal TotalStockAmount { get; set; }
        }

    }
}