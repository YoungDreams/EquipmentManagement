using System.Web.Mvc;
using Foundation.Data;
using PensionInsurance.Commands;
using PensionInsurance.Query;
using PensionInsurance.Web.Common;

namespace PensionInsurance.Web.Views.SystemSetting.ServicePackCatalog
{
    public class IndexViewModel
    {
        private readonly UrlHelper _urlHelper;

        public IndexViewModel(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public ServicePackCatalogQuery Query { get; set; }
        public PagedData<Entities.ServicePackCatalog> ServicePackCatalog { get; set; }

        public object DeleteCommand(int id)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("Delete", "ServicePackCatalog"),
                Command = new DeleteEntityCommand { EntityId = id }
            };
        }

        public string viewPrice(decimal price)
        {
            string value = "";
            if (price != 0)
            {
                value = price.ToString("0");
            }
            return value;
        }
    }

}