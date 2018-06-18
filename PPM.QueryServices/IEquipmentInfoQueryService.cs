using System;
using System.Collections.Generic;
using Foundation.Data;
using PPM.Entities;

namespace PPM.Query
{
    public class EquipmentInfoQuery
    {
        /// <summary>
        /// 产品大类
        /// </summary>
        public int? CategoryId { get; set; }
        /// <summary>
        /// 产品小类
        /// </summary>
        public int? CategoryId1 { get; set; }
        /// <summary>
        /// 产品小类
        /// </summary>
        public int? BatchNum { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 产品编号
        /// </summary>
        public string IdentifierNo { get; set; }
        /// <summary>
        /// 出厂日期，大于小于等于
        /// </summary>
        public DateTime? OutDateTime { get; set; }
    }
    public interface IEquipmentInfoQueryService
    {
        PagedData<EquipmentInfo> Query(int page, int pageSize, EquipmentInfoQuery query);
        IEnumerable<EquipmentInfo> Query(EquipmentInfoQuery query);
        IEnumerable<EquipmentInfo> QueryAll();
        EquipmentInfo Get(int id);
    }
}