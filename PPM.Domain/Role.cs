using System.Collections.Generic;

namespace PPM.Entities
{
    public class Role : MyEntity
    {
        public virtual string Name { get; set; }
        public virtual RoleType RoleType { get; set; }
        public virtual string Description { get; set; }
        public virtual IList<RolePermission> RolePermissions { get; set; }
    }
}
