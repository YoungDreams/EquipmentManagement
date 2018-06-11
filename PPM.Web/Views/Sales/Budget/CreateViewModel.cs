using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PensionInsurance.Web.Views.Sales.Budget
{
    public class CreateViewModel
    {
        public int ProjectId { get; set; }
        public DateTime Year { get; set; }
        public IEnumerable<SelectListItem> ProjectList { get; set; }
    }
}