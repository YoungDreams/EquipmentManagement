using System;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace PPM.Entities
{
    public class EquipmentInfo : MyEntity
    {
        /// <summary>
        /// ��Ʒ���࣬�ɲ�ѯ
        /// </summary>
        public virtual EquipmentCategory EquipmentCategory { get; set; }
        public virtual IList<EquipmentInfoColumnValue> EquipmentInfoColumnValues { get; set; }
        public virtual string QrCodeImage { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        public virtual string Manufacturer { get;set; }
        /// <summary>
        /// ���Σ��ɲ�ѯ
        /// </summary>
        public virtual int BatchNum { get;set; }
        /// <summary>
        /// ��ƷС�࣬�ɲ�ѯ
        /// </summary>
        public virtual EquipmentCategory EquipmentCategory1 { get; set; }
        /// <summary>
        /// ��Ʒ���ƣ��ɲ�ѯ
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// ��ƷͼƬ·��
        /// </summary>
        public virtual string ImageUrl { get; set; }
        /// <summary>
        /// ��Ʒ���룬�ɲ�ѯ
        /// </summary>
        public virtual string IdentifierNo { get; set; }
        /// <summary>
        /// ����ͺ�
        /// </summary>
        public virtual string Specification { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        public virtual string Meterial { get; set; }
        /// <summary>
        /// ������Ա
        /// </summary>
        public virtual string Technician { get; set; }
        /// <summary>
        /// ������Ա
        /// </summary>
        public virtual string Supplier { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        public virtual string Picker { get; set; }
        /// <summary>
        /// �������ڣ��ɲ�ѯ
        /// </summary>
        public virtual DateTime? OutDateTime { get; set; }
        /// <summary>
        /// �����Ա
        /// </summary>
        public virtual string Checker { get; set; }
        /// <summary>
        /// �����
        /// </summary>
        public virtual string CheckResult { get; set; }
        /// <summary>
        /// ��Ʒִ�б�׼
        /// </summary>
        public virtual string ExecuteStandard { get; set; }
        /// <summary>
        /// ��װλ��
        /// </summary>
        public virtual string SetupLocation { get; set; }
    }

    public class EquipmentInfoColumnValue : MyEntity
    {
        public virtual EquipmentInfo EquipmentInfo { get; set; }
        public virtual string Value { get; set; }
    }
}