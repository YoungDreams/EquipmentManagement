using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace PPM.Entities.Mappings
{
    public class EquipmentInfoMapping : IAutoMappingOverride<EquipmentInfo>
    {
        public void Override(AutoMapping<EquipmentInfo> mapping)
        {
            mapping.HasMany(x => x.EquipmentInfoColumnValues).Cascade.All();
            mapping.References(x => x.EquipmentCategory);
        }
    }

    public class EquipmentCategoryMapping : IAutoMappingOverride<EquipmentCategory>
    {
        public void Override(AutoMapping<EquipmentCategory> mapping)
        {
            mapping.HasMany(x => x.Columns).Cascade.All();
        }
    }

    public class EquipmentCategoryColumnMapping : IAutoMappingOverride<EquipmentCategoryColumn>
    {
        public void Override(AutoMapping<EquipmentCategoryColumn> mapping)
        {
            mapping.References(x => x.EquipmentCategory).Not.Nullable();
        }
    }
}