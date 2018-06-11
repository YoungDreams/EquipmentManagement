using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PensionInsurance.Entities;
using PensionInsurance.Entities.DetailViews;

namespace PensionInsurance.Web.Views.Sales.SalesRoomChart
{
    public class IndexViewModel
    {
        public IEnumerable<SalesRoomChartDetail> SalesRoomChartDetails { get; set; }
    }
}