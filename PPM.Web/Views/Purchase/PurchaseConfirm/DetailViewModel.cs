using System.Collections.Generic;
using System.Linq;
using Foundation.Data;
using PensionInsurance.CommandHandlers;
using PensionInsurance.Entities;
using PensionInsurance.Query;
using PensionInsurance.Shared;
using PensionInsurance.Web.Views.TrackingResult;
using PensionInsurance.Entities.Exceptions;

namespace PensionInsurance.Web.Views.Purchase.PurchaseConfirm
{
    public class DetailViewModel
    {
        public Entities.PurchaseConfirm PurchaseConfirm { get; set; }
        public IEnumerable<PurchaseOrderAcceptance> PurchaseOrderAcceptances { get; set; }
        public TrackingResultViewModel TrackingResult { get; set; }
        public WorkflowStep CurrentWorkFlowStep { get; set; }
        public int CurrentWorkflowStepId { get; set; }
        public decimal TotalAmount { get; set; }
    }
}