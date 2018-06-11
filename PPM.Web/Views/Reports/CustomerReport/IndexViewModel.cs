using System;
using System.Collections.Generic;
using System.Web.Mvc;
using PensionInsurance.Entities;

namespace PensionInsurance.Web.Views.Reports.CustomerReport
{
    public class IndexViewModel
    {
        public List<int> ProjectIds { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool? IsNotLivedForOneMonth { get; set; }
        public CustomerSalesType? CustomerSalesType { get; set; }
        public IEnumerable<SelectListItem> ProjectList { get; set; }
    }

    
}