using System.Collections.Generic;
using System.Web.Mvc;
using Foundation.Data;
using PensionInsurance.Commands;
using PensionInsurance.Query;
using PensionInsurance.Web.Common;

namespace PensionInsurance.Web.Views.InvestmentProject
{
    public class IndexViewModel
    {
        private readonly UrlHelper _urlHelper;
        public IndexViewModel(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public InvestmentProjectQuery Query { get; set; }
        public PagedData<Entities.InvestmentProject> Items { get; set; }
        public List<SelectListItem> Types { get; set; }
        public List<SelectListItem> Status { get; set; }
        public object DeleteCommand(int id)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("Delete", "InvestmentProject"),
                Command = new DeleteInvestmentProjectCommand { InvestmentProjectId = id }
            };
        }
    }
}