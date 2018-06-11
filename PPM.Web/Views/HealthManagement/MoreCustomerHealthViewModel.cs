using Foundation.Data;
using PensionInsurance.Entities.DetailViews;
using PensionInsurance.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PensionInsurance.Web.Views.HealthManagement
{
    public class MoreCustomerHealthViewModel
    {
        private readonly UrlHelper _url;

        public MoreCustomerHealthViewModel(UrlHelper url)
        {

            _url = url;
        }

        public PagedData<CustomerHealthCheckOutDetail> Items { get; set; }

        public MoreCustomerHealthQuery Query { get; set; }
    }
}