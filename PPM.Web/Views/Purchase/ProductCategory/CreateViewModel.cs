using System.Collections.Generic;
using System.Web.Mvc;
using PensionInsurance.Commands;

namespace PensionInsurance.Web.Views.Purchase.ProductCategory
{
    public class CreateViewModel: CreatePurchaseProductCategoryCommand
    {
        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<SelectListItem> Projects { get; set; }
        public IEnumerable<SelectListItem> Departments { get; set; }
        public ProductCategoryTreeView ProductCategoryTreeView { get; set; }
    }
}