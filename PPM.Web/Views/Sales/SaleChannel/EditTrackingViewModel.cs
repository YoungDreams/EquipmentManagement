using System;

namespace PensionInsurance.Web.Views.Sales.SaleChannel
{
    public class EditTrackingViewModel
    {
        public int TrackingId { get; set; }
        public string TrackingType { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
    }
}