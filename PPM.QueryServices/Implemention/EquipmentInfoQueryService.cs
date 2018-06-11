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
            if (query.CategoryId.HasValue)
            {
                equipmentInfos = equipmentInfos.Where(x => x.EquipmentCategory.Id == query.CategoryId);
            }
            else
            {
                return equipmentInfos.Take(0).ToList().ToPagedData(page, pageSize);
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