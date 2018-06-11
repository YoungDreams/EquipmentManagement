using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PensionInsurance.Entities;
using PensionInsurance.Web.Views.TrackingResult;

namespace PensionInsurance.Web.Views.Finance.ReliefBill
{
    public class DetailViewModel
    {
        public string CustomerName { get; set; }
        public CustomerBill PreCustomerBill { get; set; }
        public CustomerBill CustomerBill { get; set; }
        public int BillId { get; set; }
        public decimal CustomerCurrentYearDiscount { get; set; }
        public decimal ProjectYearDiscount { get; set; }

        public WorkflowStep CurrentWorkFlowStep { get; set; }
        public int CurrentWorkflowStepId { get; set; }
        public TrackingResultViewModel TrackingResult { get; set; }

    }
}