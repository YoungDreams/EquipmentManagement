using System.Collections.Generic;
using System.Web.Mvc;
using Foundation.Data;
using PensionInsurance.Commands;
using PensionInsurance.Query;
using PensionInsurance.Web.Common;

namespace PensionInsurance.Web.Views.InvestmentProjectLand
{
    public class IndexViewModel
    {
        private readonly UrlHelper _urlHelper;

        public IndexViewModel(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public InvestmentProjectLandQuery Query { get; set; }
        public PagedData<Entities.InvestmentProjectLand> InvestmentProjectLands { get; set; }

        public object DeleteCommand(int id, string strUrl)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("Delete", "InvestmentProjectLand"),
                Command = new DeleteEntityCommand { EntityId = id, ReturnUrl = strUrl }
            };
        }
    }
}