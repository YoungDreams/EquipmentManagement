using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PensionInsurance.Web.Views.Budget
{
    public class ImportViewModel
    {
        public DateTime Year { get; set; }
        public int? ProjectId { get; set; }
        public IEnumerable<SelectListItem> ProjectList { get; set; }
    }
}