using System;
using System.Collections.Generic;

namespace PensionInsurance.Web.Views.ContractRoomChange
{
    public class CreateViewModel
    {
        public IEnumerable<Entities.Building> BuildingList { get; set; }

        /// <summary>
        /// 项目ID
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// 合同ID
        /// </summary>
        public int ContractId { get; set; }

        public Entities.Contract Contract { get; set; }
        public int CustomerAccountId { get; set; }
        /// <summary>
        /// 换房补充协议编号
        /// </summary>
        public string ContractRoomChangeNo { get; set; }
        /// <summary>
        /// 客户姓名
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 换房日期
        /// </summary>
        public DateTime ChangeDate { get; set; }
        /// <summary>
        /// 换房结束日期
        /// </summary>
        public DateTime ChangeEndDate { get; set; }
        /// <summary>
        /// 原房型
        /// </summary>
        public string OldRoomType { get; set; }
        /// <summary>
        /// 原楼栋Id
        /// </summary>
        public int? OldBuildingId { get; set; }
        /// <summary>
        /// 原单元Id
        /// </summary>
        public int? OldUnitId { get; set; }
        /// <summary>
        /// 原楼层Id
        /// </summary>
        public int? OldFloorId { get; set; }
        /// <summary>
        /// 原房间Id
        /// </summary>
        public int? OldRoomId { get; set; }
        /// <summary>
        /// 原床位Id
        /// </summary>
        public int? OldBedId { get; set; }
        /// <summary>
        /// 原是否包房
        /// </summary>
        public bool OldIsCompartment { get; set; }
        /// <summary>
        /// 新房型
        /// </summary>
        public string NewRoomType { get; set; }
        /// <summary>
        /// 新楼栋Id
        /// </summary>
        public int? NewBuildingId { get; set; }
        /// <summary>
        /// 新单元Id
        /// </summary>
        public int? NewUnitId { get; set; }
        /// <summary>
        /// 新楼层Id
        /// </summary>
        public int? NewFloorId { get; set; }
        /// <summary>
        /// 新房间Id
        /// </summary>
        public int? NewRoomId { get; set; }
        /// <summary>
        /// 新床位Id
        /// </summary>
        public int? NewBedId { get; set; }
        /// <summary>
        /// 新是否包房
        /// </summary>
        public bool NewIsCompartment { get; set; }
        /// <summary>
        /// 乙方应缴纳基础房费（短期）
        /// </summary>
        public decimal ShortRoomRate { get; set; }
        /// <summary>
        /// 乙方应缴纳餐费（短期）
        /// </summary>
        public decimal ShortMeals { get; set; }
        /// <summary>
        /// 乙方应缴纳合计独立型月费（短期）
        /// </summary>
        public decimal ShortMonthlyAmount { get; set; }
        /// <summary>
        /// 基础服务费（短期） 
        /// </summary>
        public decimal ShortServiceFee { get; set; }
        /// <summary>
        /// 乙方应缴纳基础房费（长期）
        /// </summary>
        public decimal LongRoomRate { get; set; }
        /// <summary>
        /// 乙方应缴纳餐费（长期）
        /// </summary>
        public decimal LongMeals { get; set; }
        /// <summary>
        /// 乙方应缴纳合计独立型月费（长期）
        /// </summary>
        public decimal LongMonthlyAmount { get; set; }
        /// <summary>
        /// 基础服务费（长期） 
        /// </summary>
        public decimal LongServiceFee { get; set; }
        /// <summary>
        /// 协议状态
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 附件文件路径
        /// </summary>
        public string FilePath { get; set; }

        public int CurrentRoomId { get; set; }
        public int? CurrentBedId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string ChargeType { get; set; }
        public string ChargeDescription { get; set; }
    }
}