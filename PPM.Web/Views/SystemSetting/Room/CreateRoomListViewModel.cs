using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PensionInsurance.Web.Views.SystemSetting.Room
{
    public class CreateRoomListViewModel
    {
        public List<CreateRoomViewModel> CreateRoomViewModels { get; set; }
        public List<SelectListItem> RoomTypes { get; set; }
    }

    public class CreateRoomViewModel
    {
        public int Id { get; set; }
        public int Floor { get; set; }
        public string FloorName { get; set; }
        public int Unit { get; set; }
        public string UnitName { get; set; }
        public int Building { get; set; }
        public string BuildingName { get; set; }
        public string Remark { get; set; }
        public int? RoomTypeId { get; set; }
    }
}