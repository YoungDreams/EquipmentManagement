using Foundation.Messaging;

namespace PPM.Commands
{
    public class DeleteEquipmentCategoryCommand : Command
    {
        public int Id { get; set; }
        public string ReturnUrl { get; set; }
    }
}
