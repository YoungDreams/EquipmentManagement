using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Entities.DetailViews;
using PensionInsurance.Web.Common;
using PensionInsurance.Web.Views.TrackingResult;

namespace PensionInsurance.Web.Views.ContractCostChange
{
    public class EditContractCostViewModel : EditContractCostChangeCommand
    {
        private readonly UrlHelper _urlHelper;
        public EditContractCostViewModel(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }
        public decimal CustomerCurrentYearDiscount { get; set; }
        public decimal ProjectYearDiscount { get; set; }
        public WorkflowStep CurrentWorkFlowStep { get; set; }
        public int CurrentWorkflowStepId { get; set; }
        public TrackingResultViewModel TrackingResult { get; set; }
        public int CustomerAccountId { get; set; }
        public WebCommand Submit(int id, int contractId)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("Submit", "ContractCostChange"),
                Command = new SubmitContractCostChangeCommand { Id = id, ContractId = contractId }
            };
        }
        
        public WebCommand DraftAndDelete(int id, int contractId)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("DraftAndDelete", "ContractCostChange"),
                Command = new DraftAndDeleteContractCostChangeCommand { ContractCostChangeId = id, ContractId = contractId }
            };
        }

        public object Print(int contractCostChangeId)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("Print", "ContractCostChange"),
                Command = new PrintContractCostChangeWordToPdfCommand { ContractCostChangeId = contractCostChangeId }
            };
        }

        public WebCommand LockedAttachment(int id, int contractId)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("LockedAttachment", "ContractCostChange"),
                Command = new LockedContractCostChangeAttachmentCommand { ContractCostChangeId = id, ContractId = contractId }
            };
        }
    }
}