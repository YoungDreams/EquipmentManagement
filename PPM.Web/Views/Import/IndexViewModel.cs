using System.Collections.Generic;
using PensionInsurance.Entities.DetailViews;

namespace PensionInsurance.Web.Views.Import
{
    public class IndexViewModel
    {
        public IEnumerable<ConsultingDetail> NeedToTrackConsultings { get; set; }
    }
}