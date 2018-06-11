namespace PPM.Entities
{
    //角色权限
    public class RolePermission : MyEntity
    {
        public virtual Role Role { get; set; }
        public virtual ModuleType ModuleType { get; set; }
        public virtual Permission Permission { get; set; }
    }
}
