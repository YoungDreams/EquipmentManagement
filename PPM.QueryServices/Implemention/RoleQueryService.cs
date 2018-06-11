using System.Collections.Generic;
using Foundation.Data;
using PPM.Entities;

namespace PPM.Query.Implemention
{
    internal class RoleQueryService : IRoleQueryService
    {
        private readonly IFetcher _fetcher;

        public RoleQueryService(IFetcher fetcher)
        {
            _fetcher = fetcher;
        }

        public PagedData<Role> Query(int page, int pageSize, RoleQuery query)
        {
            var sql = "SELECT * FROM [Role] WHERE 1=1";

            if (!string.IsNullOrEmpty(query.RoleName))
            {
                sql += " AND Name=@RoleName";
            }
            return _fetcher.QueryPaged<Role>(sql, "Id", page, pageSize, query);
        }

        public IEnumerable<Role> QueryAll()
        {
            return _fetcher.Query<Role>();
        }

        public Role Get(int roleId)
        {
            return _fetcher.Get<Role>(roleId);
        }

        public IEnumerable<RolePermission> QueryRolePermissions()
        {
            return _fetcher.Query<RolePermission>();
        }

        public IEnumerable<RolePermission> QueryRolePermissions(RolePermissionQuery query)
        {
            var sql = "SELECT * FROM RolePermission WHERE 1=1";
            sql += " AND RoleId=@RoleId";
            return _fetcher.Query<RolePermission>(sql, query);
        }
    }
}