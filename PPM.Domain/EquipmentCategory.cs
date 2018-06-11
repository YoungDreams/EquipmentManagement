using System.Collections.Generic;

namespace PPM.Entities
{
    public class EquipmentCategory : MyEntity
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual int? ParentId { get; set; }
        public virtual int Layer { get; set; }
        public virtual bool Published { get; set; }
        public virtual IList<EquipmentCategoryColumn> Columns { get; set; }
    }

    public class EquipmentCategoryColumn : MyEntity
    {
        public virtual EquipmentCategory EquipmentCategory { get; set; }
        public virtual string ColumnName { get; set; }
        public virtual string ColumnType { get; set; }
    }
}