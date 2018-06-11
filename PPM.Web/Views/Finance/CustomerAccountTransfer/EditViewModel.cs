using PensionInsurance.Commands;
using System.Web.Mvc;
using PensionInsurance.Entities;
using PensionInsurance.Web.Common;
using PensionInsurance.Web.Views.TrackingResult;

namespace PensionInsurance.Web.Views.Finance.CustomerAccountTransfer
{
    public class EditViewModel 
    {
        private readonly UrlHelper _urlHelper;
        public EditViewModel(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public int CustomerAccountTransferId { get; set; }
        public int TransferFrom { get; set; }
        public string TransferFromCustomerName { get; set; }
        public int TransferTo { get; set; }
        public string TransferToCustomerName { get; set; }
        public string TransferToCustomerNo { get; set; }
        public string TransferToProjectName { get; set; }
        public decimal TransferBalance { get; set; }
        public decimal TransferAmount { get; set; }
        public string Reason { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public int ProjectId { get; set; }
        public CustomerAccountTransferStatus Status { get; set; }
        public TrackingResultViewModel TrackingResult { get; set; }
        public WorkflowStep CurrentWorkFlowStep { get; set; }
        public int CurrentWorkflowStepId { get; set; }

        //public WebCommand DraftAndDelete(int id, int contractId)
        //{
        //    return new WebCommand
        //    {
        //        Url = _urlHelper.Action("DraftAndDelete", "ContractRoomChange"),
        //        Command = new DraftAndDeleteContractRoomChangeCommand { ContractRoomChangeId = id, ContractId = contractId }
        //    };
        //}

        public object Print(int customerAccountTransferId)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("Print", "CustomerAccountTransfer"),
                Command = new PrintCustomerAccountTransferWordToPdfCommand { CustomerAccountTransferId = customerAccountTransferId }
            };
        }

        public WebCommand LockedAttachment(int id, int stepId, WorkflowResult result)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("LockedAttachment", "CustomerAccountTransfer"),
                Command = new LockedCustomerAccountTransferAttachmentCommand
                {
                    CustomerAccountTransferId = id,
                    CurrentWorkflowStepId = stepId,
                    Result = result
                }
            };
        }
    }
}