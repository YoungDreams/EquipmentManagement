using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace PPM.Entities.Mappings
{
    public class EquipmentColumnValueMapping : IAutoMappingOverride<EquipmentInfoColumnValue>
    {
        public void Override(AutoMapping<EquipmentInfoColumnValue> mapping)
        {
            mapping.References(x => x.EquipmentInfo).Not.Nullable();
        }
    }
}