using System.Collections.Generic;
using System.Linq;
using Foundation.Data;
using Foundation.Data.Implemention;
using PPM.Entities;

namespace PPM.Query.Implemention
{
    public class EquipmentInfoQueryService : IEquipmentInfoQueryService
    {
        private readonly IFetcher _fetcher;
        public EquipmentInfoQueryService(IFetcher fetcher)
        {
            _fetcher = fetcher;
        }

        public PagedData<EquipmentInfo> Query(int page, int pageSize, EquipmentInfoQuery query)
        {
            var equipmentInfos = _fetcher.Query<EquipmentInfo>();
            if (query.CategoryId.HasValue && query.CategoryId.Value > 0)
            {
                equipmentInfos = equipmentInfos.Where(x => x.EquipmentCategory.Id == query.CategoryId);
            }
            if (!string.IsNullOrEmpty(query.Name))
            {
                equipmentInfos = equipmentInfos.Where(x => x.Name.Contains(query.Name));
            }
            if (query.BatchNum.HasValue)
            {
                equipmentInfos = equipmentInfos.Where(x => x.BatchNum == query.BatchNum.Value);
            }
            if (query.CategoryId1.HasValue&& query.CategoryId1.Value > 0)
            {
                equipmentInfos = equipmentInfos.Where(x => x.EquipmentCategory1.Id == query.CategoryId1);
            }
            if (!string.IsNullOrEmpty(query.IdentifierNo))
            {
                equipmentInfos = equipmentInfos.Where(x => x.IdentifierNo.Contains(query.IdentifierNo));
            }
            if (query.OutDateTime.HasValue)
            {
                equipmentInfos = equipmentInfos.Where(x => x.OutDateTime == query.OutDateTime);
            }
            return equipmentInfos.ToPagedData(page, pageSize);
        }
        public IEnumerable<EquipmentInfo> Query(EquipmentInfoQuery query)
        {
            var equipmentInfos = _fetcher.Query<EquipmentInfo>();
            if (query.CategoryId.HasValue)
            {
                equipmentInfos = equipmentInfos.Where(x => x.EquipmentCategory.Id == query.CategoryId);
            }
            return equipmentInfos.ToList();
        }
        public IEnumerable<EquipmentInfo> QueryAll()
        {
            return _fetcher.Query<EquipmentInfo>().ToList();
        }
        public EquipmentInfo Get(int id)
        {
            return _fetcher.Get<EquipmentInfo>(id);
        }
    }
}