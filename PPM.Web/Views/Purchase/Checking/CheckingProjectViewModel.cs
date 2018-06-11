using System.Web.Mvc;
using Foundation.Data;
using PensionInsurance.Query;
using PensionInsurance.Web.Common;
using PensionInsurance.Commands;

namespace PensionInsurance.Web.Views.Purchase.Checking
{
    public class CheckingProjectViewModel
    {
        private readonly UrlHelper _urlHelper;
        public CheckingProjectViewModel(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }
        public CheckingProjectQuery Query { get; set; }
        public PagedData<Entities.CheckingProject> Items { get; set; }
        public object DeleteCommand(int id, string strUrl)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("Delete", "Supplier"),
                Command = new DeletePurchaseSupplierCommand { Id = id, ReturnUrl = strUrl }
            };
        }
    }
}