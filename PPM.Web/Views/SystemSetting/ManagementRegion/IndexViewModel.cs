using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foundation.Data;
using PensionInsurance.Commands;
using PensionInsurance.Query;
using PensionInsurance.Web.Common;

namespace PensionInsurance.Web.Views.SystemSetting.ManagementRegion
{
    public class IndexViewModel
    {
        private readonly UrlHelper _urlHelper;

        public IndexViewModel(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public ManagementRegionQuery Query { get; set; }
        public PagedData<Entities.ManagementRegion> ManagementRegions { get; set; }

        public object DeleteCommand(int id)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("Delete", "ManagementRegion"),
                Command = new DeleteEntityCommand { EntityId = id }
            };
        }
    }
}