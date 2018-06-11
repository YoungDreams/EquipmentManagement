using System.Collections.Generic;
using System.Web.Mvc;
using Foundation.Data;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Entities.DetailViews;
using PensionInsurance.Query;
using PensionInsurance.Web.Common;

namespace PensionInsurance.Web.Views.SystemSetting.Room
{
    public class IndexViewModel
    {
        private readonly UrlHelper _urlHelper;
        public IndexViewModel(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }
        
        public RoomQuery Query { get; set; }
        public PagedData<RoomDetail> Items { get; set; }
        public int ProjectId { get; set; }
        public IEnumerable<Building> Buildings { get; set; }
        public IEnumerable<Unit> Units { get; set; }
        public IEnumerable<Floor> Floors { get; set; }
        public List<SelectListItem> RoomTypes { get; set; }
        public int FloorCount { get; set; }
        public int UnitCount { get; set; }

        public object DeleteCommand(int id, string strUrl)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("Delete", "Room"),
                Command = new DeleteRoomCommand { RoomId = id, ReturnUrl = strUrl }
            };
        }
    }
}