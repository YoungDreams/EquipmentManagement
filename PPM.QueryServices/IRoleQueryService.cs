using System.Collections.Generic;
using Foundation.Data;
using PPM.Entities;

namespace PPM.Query
{
    public class RoleQuery
    {
        public string RoleName { get; set; }
    }
    public class RolePermissionQuery
    {
        public int RoleId { get; set; }
    }
    public interface IRoleQueryService
    {
        PagedData<Role> Query(int page, int pageSize, RoleQuery query);
        IEnumerable<Role> QueryAll();
        Role Get(int roleId);
        IEnumerable<RolePermission> QueryRolePermissions();
        IEnumerable<RolePermission> QueryRolePermissions(RolePermissionQuery query);
    }
}