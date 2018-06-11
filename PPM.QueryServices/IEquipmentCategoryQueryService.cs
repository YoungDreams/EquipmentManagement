using System.Collections.Generic;
using Foundation.Data;
using PPM.Entities;

namespace PPM.Query
{
    public class EquipmentCategoryQuery
    {
        public string Name { get; set; }
        public bool? Published { get; set; }
    }
    public interface IEquipmentCategoryQueryService
    {
        PagedData<EquipmentCategory> Query(int page, int pageSize, EquipmentCategoryQuery query);
        IEnumerable<EquipmentCategory> Query(EquipmentCategoryQuery query);
        IEnumerable<EquipmentCategory> QueryAll();
        IEnumerable<EquipmentCategory> QueryParent();
        IEnumerable<EquipmentCategory> QueryAllValid();
        EquipmentCategory Get(int id);
        List<int> QueryAllValidByParentCategoryFilter(int parentId);
        IEnumerable<EquipmentCategory> GetCategoriesByParentId(int parentId, bool includeParent = false);
    }
}