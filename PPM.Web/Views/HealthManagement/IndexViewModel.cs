using Foundation.Data;
using PensionInsurance.Entities;
using PensionInsurance.Entities.DetailViews;
using PensionInsurance.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PensionInsurance.Web.Views.HealthManagement
{
    public class IndexViewModel
    {
        private readonly UrlHelper _url;

        public IndexViewModel(UrlHelper url)
        {

            _url = url;
        }
        public int EvaluateCount { get; set; }

        public PagedData<CustomerHealthCheckInDetail> Items { get; set; }

        public CustomerHealthQuery Query { get; set; }
    }
}