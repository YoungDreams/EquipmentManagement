using System.Collections.Generic;
using System.Linq;
using Foundation.Data;
using Foundation.Data.Implemention;
using PPM.Entities;

namespace PPM.Query.Implemention
{
    public class EquipmentCategoryQueryService : IEquipmentCategoryQueryService
    {
        private readonly IFetcher _fetcher;
        public EquipmentCategoryQueryService(IFetcher fetcher)
        {
            _fetcher = fetcher;
        }

        public PagedData<EquipmentCategory> Query(int page, int pageSize, EquipmentCategoryQuery query)
        {
            var productCategorys = _fetcher.Query<EquipmentCategory>();
            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                productCategorys = productCategorys.Where(x => x.Name.Contains(query.Name));
            }
            if (query.Published.HasValue)
            {
                productCategorys = productCategorys.Where(x => x.Published == query.Published);
            }
            return productCategorys.ToPagedData(page, pageSize);
        }

        public IEnumerable<EquipmentCategory> Query(EquipmentCategoryQuery query)
        {
            var productCategorys = _fetcher.Query<EquipmentCategory>();
            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                productCategorys = productCategorys.Where(x => x.Name.Contains(query.Name));
            }
            if (query.Published.HasValue)
            {
                productCategorys = productCategorys.Where(x => x.Published == query.Published);
            }
            return productCategorys.ToList();
        }

        public IEnumerable<EquipmentCategory> QueryAll()
        {
            return _fetcher.Query<EquipmentCategory>().ToList();
        }

        public IEnumerable<EquipmentCategory> QueryAllValid()
        {
            return _fetcher.Query<EquipmentCategory>().Where(x => x.Published).ToList();
        }

        public IEnumerable<EquipmentCategory> QueryParent()
        {
            return _fetcher.Query<EquipmentCategory>().Where(x => x.Published && x.Layer == 1 && x.ParentId == 0).ToList();
        }

        public EquipmentCategory Get(int id)
        {
            return _fetcher.Get<EquipmentCategory>(id);
        }

        /// <summary>
        /// 获取父类包含的二级类别
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<int> QueryAllValidByParentCategoryFilter(int parentId)
        {
            var categories = _fetcher.Query<EquipmentCategory>().Where(x => x.ParentId == parentId)
                .Select(x => x.Id).ToList();
            categories.Add(parentId);
            return categories;
        }

        public IEnumerable<EquipmentCategory> GetCategoriesByParentId(int parentId, bool includeParent = false)
        {
            if (includeParent)
            {
                return _fetcher.Query<EquipmentCategory>().Where(x => x.ParentId == parentId || x.Id == parentId).ToList();
            }
            return _fetcher.Query<EquipmentCategory>().Where(x => x.ParentId == parentId).ToList();
        }
    }
}