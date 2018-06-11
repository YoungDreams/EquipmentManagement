using System;
using System.Collections.Generic;
using System.Web.Mvc;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Web.Views.TrackingResult;

namespace PensionInsurance.Web.Views.CustomerMoveOutTicket
{
    public class CreateViewModel : CreateCustomerMoveOutTicketCommand
    {
        public string Sex { get; set; }
        public int Age { get; set; }
        public string CustomerName { get; set; }
        public string ProjectName { get; set; }

        public TrackingResultViewModel TrackingResult { get; set; }
        public WorkflowStep CurrentWorkFlowStep { get; set; }
    }
}