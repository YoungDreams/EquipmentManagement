using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PensionInsurance.Commands;

namespace PensionInsurance.Web.Views.Purchase.Product
{
    public class CreateViewModel: CreateProductCommand
    {
        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<SelectListItem> Cities { get; set; }
        public IEnumerable<SelectListItem> Suppliers { get; set; }
        public ProductCategoryTreeView ProductCategoryTreeView { get; set; }
    }
}