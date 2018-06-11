using System.Collections.Generic;

namespace PPM.Entities
{
    public class EquipmentInfo : MyEntity
    {
        public virtual EquipmentCategory EquipmentCategory { get; set; }
        public virtual IList<EquipmentInfoColumnValue> EquipmentInfoColumnValues { get; set; }
        public virtual string QrCodeImage { get; set; }
    }

    public class EquipmentInfoColumnValue : MyEntity
    {
        public virtual EquipmentInfo EquipmentInfo { get; set; }
        public virtual string Value { get; set; }
    }
}