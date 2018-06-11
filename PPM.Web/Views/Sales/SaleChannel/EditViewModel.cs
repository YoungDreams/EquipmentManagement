using System.Collections.Generic;
using System.Web.Mvc;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Web.Common;

namespace PensionInsurance.Web.Views.Sales.SaleChannel
{
    public class EditViewModel : EditSaleChannelCommand
    {
        private readonly UrlHelper _urlHelper;
        public EditViewModel(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public string HeaderText { get; set; }
        public string CreatedTime { get; set; }
        public string CreatorName { get; set; }
        public IEnumerable<SelectListItem> ProjectList { get; set; }

        public IEnumerable<SaleChannelContact> SaleChannelContacts { get; set; }
        public IEnumerable<SaleChannelTracking> SaleChannelTrackings { get; set; }

        public object DeleteContactCommand(int id)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("DeleteContact", "SaleChannel"),
                Command = new DeleteSaleChannelContactCommand { ContactId = id, SaleChannelId = SaleChannelId }
            };
        }

        public object DeleteTrackingCommand(int id)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("DeleteTracking", "SaleChannel"),
                Command = new DeleteSaleChannelTrackingCommand { TrackingId = id, SaleChannelId = SaleChannelId }
            };
        }
    }
}