using System;
using System.Web.Mvc;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Web.Common;

namespace PensionInsurance.Web.Views.Contract
{
    public class DetailViewModel : EditViewModel
    {
        public CustomerAccount CustomerAccount { get; set; }
        public Project Project { get; set; }
        public WorkflowStep CurrentWorkFlowStep { get; set; }
        public int CurrentWorkflowStepId { get; set; }
        public DateTime CreateTime { get; set; }
        
        private readonly UrlHelper _urlHelper;
        public DetailViewModel(UrlHelper urlHelper) : base(urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public WebCommand ApprovalContractCommand(int id)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("ApprovalContract", "Contract"),
                Command = new ApprovalContractCommand { ContractId = id }
            };
        }

        public WebCommand PrintContractCommand(int id)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("PrintContract", "Contract"),
                Command = new PrintContractCommand { ContractId = id }
            };
        }

        public WebCommand SubmitAttachmentCommand(int id, int stepId)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("SubmitAttachment", "Contract"),
                Command = new SubmitContractAttachmentCommand() { ContractId = id, CurrentWorkflowStepId = stepId}
            };
        }
    }
    
}