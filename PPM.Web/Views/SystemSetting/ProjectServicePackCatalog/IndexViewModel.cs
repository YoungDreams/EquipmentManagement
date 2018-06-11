using System.Web.Mvc;
using Foundation.Data;
using PensionInsurance.Query;


namespace PensionInsurance.Web.Views.SystemSetting.ProjectServicePackCatalog
{
    public class IndexViewModel
    {
        private readonly UrlHelper _urlHelper;

        public IndexViewModel(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public ServicePackCatalogQuery Query { get; set; }
        public PagedData<Entities.ProjectServicePackCatalog> ProjectServicePackCatalogs { get; set; }

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