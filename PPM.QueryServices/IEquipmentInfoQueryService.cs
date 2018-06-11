using System.Collections.Generic;
using Foundation.Data;
using PPM.Entities;

namespace PPM.Query
{
    public class EquipmentInfoQuery
    {
        public int? CategoryId { get; set; }
    }
    public interface IEquipmentInfoQueryService
    {
        PagedData<EquipmentInfo> Query(int page, int pageSize, EquipmentInfoQuery query);
        IEnumerable<EquipmentInfo> Query(EquipmentInfoQuery query);
        IEnumerable<EquipmentInfo> QueryAll();
        EquipmentInfo Get(int id);
    }
}