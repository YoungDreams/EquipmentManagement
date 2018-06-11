using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PensionInsurance.Web.Views.Purchase
{
    public class SelectViewModel
    {
        public int ProjectId { get; set; }
        public int ProductCategoryId { get; set; }
        public int OrderType { get; set; }
        public DateTime PurchaseDate { get; set; }
        public bool ShowProjectList { get; set; }
        public IEnumerable<SelectListItem> ProjectList { get; set; }
        public IEnumerable<SelectListItem> CategoryList { get; set; }
    }
}