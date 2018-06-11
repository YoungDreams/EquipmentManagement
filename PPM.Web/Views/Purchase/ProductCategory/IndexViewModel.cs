using System.Collections.Generic;
using System.Web.Mvc;
using Foundation.Data;
using PensionInsurance.Commands;
using PensionInsurance.Query;
using PensionInsurance.Web.Common;

namespace PensionInsurance.Web.Views.Purchase.ProductCategory
{
    public class IndexViewModel
    {
        private readonly UrlHelper _urlHelper;
        public IndexViewModel(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }
        public ProductCategoryQuery Query { get; set; }
        public PagedData<Entities.PurchaseProductCategory> Items { get; set; }
        public FancyTreeNodeView FancyTreeNodeView { get; set; }
        public object DeleteCommand(int id, string strUrl)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("Delete", "ProductCategory"),
                Command = new DeleteProductCategoryCommand { Id = id, ReturnUrl = strUrl }
            };
        }
    }
}