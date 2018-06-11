using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foundation.Data;
using PensionInsurance.Commands;
using PensionInsurance.Web.Common;

namespace PensionInsurance.Web.Views.Finance.CustomerAccount
{
    public class CustomerPointsViewModel
    {
        private readonly UrlHelper _urlHelper;
        public CustomerPointsViewModel(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }
        public PagedData<Entities.CustomerPoint> CustomerPointList { get; set; }

        // 删除记录
        public object DeletePoint(int id)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("DeletePoint", "CustomerAccount"),
                Command = new DeleteEntityCommand { EntityId = id }
            };
        }

        // 记录确认
        public object ConfirmPoint(int customerPointId)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("SubmitCustomerPoint", "CustomerAccount"),
                Command = new SubmitCustomerPointCommand { CustomerPointId = customerPointId }
            };
        }
    }
}