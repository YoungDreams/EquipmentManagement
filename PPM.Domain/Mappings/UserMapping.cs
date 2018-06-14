using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace PPM.Entities.Mappings
{
    public class UserMapping : IAutoMappingOverride<User>
    {
        public void Override(AutoMapping<User> mapping)
        {
            mapping.Map(x => x.Username).Not.Nullable().Unique();
            mapping.Map(x => x.RoleType).CustomType<int>().Default("1").Not.Nullable();

            mapping.HasManyToMany(x => x.Roles).Not.LazyLoad().Cascade.All()
                .ParentKeyColumn("UserId")
                .ChildKeyColumn("RoleId")
                .Table("User_Role");
        }
    }
}
