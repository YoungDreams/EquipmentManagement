using System;
using System.Collections.Generic;
using System.Web.Mvc;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Entities.DetailViews;
using PensionInsurance.Web.Common;
using PensionInsurance.Web.Views.TrackingResult;

namespace PensionInsurance.Web.Views.ContractServicePackChange
{
    public class EditViewModel
    {
        private readonly UrlHelper _urlHelper;
        public EditViewModel(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }
        public int ProjectId { get; set; }
        /// <summary>
        /// 合同ID
        /// </summary>
        public int ContractId { get; set; }

        public Entities.Contract Contract { get; set; }
        public int CustomerAccountId { get; set; }
        /// <summary>
        /// 服务包协议ID
        /// </summary>
        public int ContractServicePackChangeId { get; set; }
        /// <summary>
        /// 服务包变更补充协议编号
        /// </summary>
        public string ContractServicePackChangeNo { get; set; }
        /// <summary>
        /// 客户姓名
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 服务包变更日期
        /// </summary>
        public DateTime ChangeDate { get; set; }
        /// <summary>
        /// 服务包变更结束日期
        /// </summary>
        public DateTime ChangeEndDate { get; set; }
        /// <summary>
        /// 生活能力评估级别
        /// </summary>
        public string ConcernType { get; set; }
        /// <summary>
        /// 生活照料服务包等级
        /// </summary>
        public string ServiceLevel { get; set; }
        /// <summary>
        /// 生活照料服务包服务项明细
        /// </summary>
        public string ServicePakgeDetail { get; set; }
        /// <summary>
        /// 乙方应缴纳生活照料服务包月费（短期）
        /// </summary>
        public decimal ShortServiceMonthlyAmount { get; set; }
        /// <summary>
        /// 乙方应缴失智照护费（短期）
        /// </summary>
        public decimal ShortConcernAmount { get; set; }
        /// <summary>
        /// 乙方应缴纳生活照料服务包月费（长期）
        /// </summary>
        public decimal LongServiceMonthlyAmount { get; set; }
        /// <summary>
        /// 乙方应缴失智照护费（长期） 
        /// </summary>
        public decimal LongConcernAmount { get; set; }
        /// <summary>
        /// 协议状态
        /// </summary>
        public ContractAddtionalStatus Status { get; set; }
        /// <summary>
        /// 附件文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 附件文件路径
        /// </summary>
        public string FilePath { get; set; }
        public bool IsLockedAttachment { get; set; }
        /// <summary>
        /// 长期附加费用
        /// </summary>
        public decimal LongAttachAmount { get; set; }
        /// <summary>
        /// 短期附加费用
        /// </summary>
        public decimal ShortAttachAmount { get; set; }
        public TrackingResultViewModel TrackingResult { get; set; }
        public WorkflowStep CurrentWorkFlowStep { get; set; }
        public int CurrentWorkflowStepId { get; set; }

        public WebCommand Submit(int id, int contractId)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("Submit", "ContractServicePackChange"),
                Command = new SubmitContractServicePackChangeCommand { ContractServicePackChangeId = id, ContractId = contractId }
            };
        }
        
        public WebCommand DraftAndDelete(int id, int contractId)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("DraftAndDelete", "ContractServicePackChange"),
                Command = new DraftAndDeleteContractServicePackChangeCommand { ContractServicePackChangeId = id, ContractId = contractId }
            };
        }

        public object Print(int contractServicePackChangeId)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("Print", "ContractServicePackChange"),
                Command = new PrintContractServicePackChangeWordToPdfCommand { ContractServicePackChangeId = contractServicePackChangeId }
            };
        }

        public WebCommand LockedAttachment(int id, int contractId,int stepId ,WorkflowResult result)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("LockedAttachment", "ContractServicePackChange"),
                Command = new LockedContractServicePackChangeAttachmentCommand { ContractServicePackChangeId = id, ContractId = contractId ,CurrentWorkflowStepId = stepId,Result = result}
            };
        }
    }
}