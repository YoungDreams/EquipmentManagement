using System.Web.Mvc;
using Foundation.Data;
using PensionInsurance.Commands;
using PensionInsurance.Query;
using PensionInsurance.Web.Common;

namespace PensionInsurance.Web.Views.SystemSetting.Bed
{
    public class IndexViewModel
    {
        private readonly UrlHelper _urlHelper;
        public IndexViewModel(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public BedQuery Query { get; set; }
        public PagedData<Entities.Bed> Beds { get; set; }

        public object DeleteCommand(int id, int roomId, string strUrl)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("Delete", "Bed"),
                Command = new DeleteBedCommand { RoomId = roomId, BedId = id, ReturnUrl = strUrl }
            };
        }
    }
}