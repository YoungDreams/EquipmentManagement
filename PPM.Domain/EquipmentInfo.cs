using System;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace PPM.Entities
{
    public class EquipmentInfo : MyEntity
    {
        /// <summary>
        /// 产品大类，可查询
        /// </summary>
        public virtual EquipmentCategory EquipmentCategory { get; set; }
        public virtual IList<EquipmentInfoColumnValue> EquipmentInfoColumnValues { get; set; }
        public virtual string QrCodeImage { get; set; }
        /// <summary>
        /// 生产厂商
        /// </summary>
        public virtual string Manufacturer { get;set; }
        /// <summary>
        /// 批次，可查询
        /// </summary>
        public virtual int BatchNum { get;set; }
        /// <summary>
        /// 产品小类，可查询
        /// </summary>
        public virtual EquipmentCategory EquipmentCategory1 { get; set; }
        /// <summary>
        /// 产品名称，可查询
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 产品图片路径
        /// </summary>
        public virtual string ImageUrl { get; set; }
        /// <summary>
        /// 产品编码，可查询
        /// </summary>
        public virtual string IdentifierNo { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        public virtual string Specification { get; set; }
        /// <summary>
        /// 材质
        /// </summary>
        public virtual string Meterial { get; set; }
        /// <summary>
        /// 技术人员
        /// </summary>
        public virtual string Technician { get; set; }
        /// <summary>
        /// 物资人员
        /// </summary>
        public virtual string Supplier { get; set; }
        /// <summary>
        /// 领料人
        /// </summary>
        public virtual string Picker { get; set; }
        /// <summary>
        /// 出厂日期，可查询
        /// </summary>
        public virtual DateTime? OutDateTime { get; set; }
        /// <summary>
        /// 检测人员
        /// </summary>
        public virtual string Checker { get; set; }
        /// <summary>
        /// 检测结果
        /// </summary>
        public virtual string CheckResult { get; set; }
        /// <summary>
        /// 产品执行标准
        /// </summary>
        public virtual string ExecuteStandard { get; set; }
        /// <summary>
        /// 安装位置
        /// </summary>
        public virtual string SetupLocation { get; set; }
    }

    public class EquipmentInfoColumnValue : MyEntity
    {
        public virtual EquipmentInfo EquipmentInfo { get; set; }
        public virtual string Value { get; set; }
    }
}