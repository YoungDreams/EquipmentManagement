using PensionInsurance.Commands;

namespace PensionInsurance.Web.Views.SystemSetting.Bed
{
    public class EditViewModel : EditBedCommand
    {
        public string HeaderText { get; set; }
        public string CreatedTime { get; set; }
        public string CreatorName { get; set; }
    }
}