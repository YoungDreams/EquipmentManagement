using System.Collections.Generic;
using System.Web.Mvc;
using PPM.Commands;
using PPM.Entities;

namespace PPM.MVC.Views.Equipment.Info
{
    public class EditViewModel: EditEquipmentInfoCommand
    {
        public EquipmentInfo EquipmentInfo { get; set; }
    }
}