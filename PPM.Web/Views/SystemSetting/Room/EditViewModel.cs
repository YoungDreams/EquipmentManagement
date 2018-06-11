using PensionInsurance.Commands;

namespace PensionInsurance.Web.Views.SystemSetting.Room
{
    public class EditViewModel : EditRoomCommand
    {
        public string HeaderText { get; set; }
        public string BuildingName { get; set; }
        public string UnitName { get; set; }
        public string FloorName { get; set; }
    }
}