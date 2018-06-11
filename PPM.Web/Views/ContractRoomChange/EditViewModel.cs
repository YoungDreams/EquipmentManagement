using System;
using PensionInsurance.Commands;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using PensionInsurance.Entities;
using PensionInsurance.Entities.DetailViews;
using PensionInsurance.Web.Common;
using PensionInsurance.Web.Views.TrackingResult;

namespace PensionInsurance.Web.Views.ContractRoomChange
{
    public class EditViewModel 
    {
        private readonly UrlHelper _urlHelper;
        public EditViewModel(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }
        /// <summary>
        /// 合同ID
        /// </summary>
        public int ContractId { get; set; }
        public Entities.Contract Contract { get; set; }
        public int CustomerAccountId { get; set; }
        /// <summary>
        /// 换房协议ID
        /// </summary>
        public int ContractRoomChangeId { get; set; }
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
        /// 附件文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 附件文件路径
        /// </summary>
        public string FilePath { get; set; }
        public bool IsLockedAttachment { get; set; }
        public string ChargeType { get; set; }
        public string ChargeDescription { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!NewIsCompartment && !NewBedId.HasValue)
                yield return new ValidationResult("请选择床位");
        }
        
        public WorkflowStep CurrentWorkFlowStep { get; set; }
        public int CurrentWorkflowStepId { get; set; }

        public int ProjectId { get; set; }
        public IEnumerable<Entities.Building> BuildingList { get; set; }
        public IEnumerable<Entities.Unit> UnitList { get; set; }
        public IEnumerable<Entities.Floor> FloorList { get; set; }
        public IEnumerable<Entities.Room> RoomList { get; set; }
        public IEnumerable<Entities.Bed> BedList { get; set; }

        public Entities.Building BuildingInfo { get; set; }
        public Entities.Unit UnitInfo { get; set; }
        public Entities.Floor FloorInfo { get; set; }
        public Entities.Room RoomInfo { get; set; }
        public Entities.Bed BedInfo { get; set; }
        public int CurrentRoomId { get; set; }
        public int? CurrentBedId { get; set; }
        public ContractAddtionalStatus Status { get; set; }
        public DateTime ChangeEndDate { get; set; }
        public TrackingResultViewModel TrackingResult { get; set; }

        public WebCommand Submit(int id, int contractId)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("Submit", "ContractRoomChange"),
                Command = new SubmitContractRoomChangeCommand { ContractRoomChangeId = id, ContractId = contractId }
            };
        }
        
        public WebCommand DraftAndDelete(int id, int contractId)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("DraftAndDelete", "ContractRoomChange"),
                Command = new DraftAndDeleteContractRoomChangeCommand { ContractRoomChangeId = id, ContractId = contractId }
            };
        }

        public object Print(int contractRoomChangeId)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("Print", "ContractRoomChange"),
                Command = new PrintContractRoomChangeWordToPdfCommand { ContractRoomChangeId = contractRoomChangeId }
            };
        }

        public WebCommand LockedAttachment(int id, int contractId,int stepId, WorkflowResult result)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("LockedAttachment", "ContractRoomChange"),
                Command = new LockedContractRoomChangeAttachmentCommand { ContractRoomChangeId = id, ContractId = contractId,CurrentWorkflowStepId = stepId,Result = result}
            };
        }
    }
}