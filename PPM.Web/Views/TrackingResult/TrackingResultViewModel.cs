using System.Collections.Generic;
using PensionInsurance.Entities;

namespace PensionInsurance.Web.Views.TrackingResult
{
    public class TrackingResultViewModel
    {
        public TrackingResultViewModel()
        {
            WorkflowHistoryTrackingResults = new List<WorkflowTrackingResult>();
            WorkflowTrackingResults = new List<WorkflowTrackingResult>();
        }
        public List<WorkflowTrackingResult> WorkflowHistoryTrackingResults { get; set; }
        public List<WorkflowTrackingResult> WorkflowTrackingResults { get; set; }
    }
}