using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace PPM.Entities.Mappings
{
    public class UserMapping : IAutoMappingOverride<User>
    {
        public void Override(AutoMapping<User> mapping)
        {
            mapping.Map(x => x.Username).Not.Nullable().Unique();

            mapping.HasManyToMany(x => x.Roles).Not.LazyLoad().Cascade.All()
                .ParentKeyColumn("UserId")
                .ChildKeyColumn("RoleId")
                .Table("User_Role");
        }
    }
}
