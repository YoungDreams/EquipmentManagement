using System;

namespace PensionInsurance.Web.Views.Sales.Consulting
{
    public class EditTrackingViewModel
    {
        public int ConsultingId { get; set; }
        public int ConsultingTrackingId { get; set; }
        public string ConsultingTrackingNo { get; set; }
        public string TrackingType { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public int Pid { get; set; }
    }
}