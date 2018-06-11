using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PensionInsurance.Commands;
using PensionInsurance.Web.Views.TrackingResult;

namespace PensionInsurance.Web.Views.CustomerMoveOutTicket
{
    public class DetailViewModel : OpinionCustomerMoveOutTicketCommand
    {
        public string Sex { get; set; }
        public int Age { get; set; }
        public string CustomerName { get; set; }
        public string ProjectName { get; set; }
        public int? NextId { get; set; }
        public int? Page { get; set; }
        public TrackingResultViewModel TrackingResult { get; set; }
    }
}