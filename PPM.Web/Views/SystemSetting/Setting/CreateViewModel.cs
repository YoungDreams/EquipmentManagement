using PensionInsurance.Entities;

namespace PensionInsurance.Web.Views.SystemSetting.Setting
{
    public class CreateViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public SettingType Type { get; set; }
        public int Sort { get; set; }
    }
}