using System.Collections.Generic;
using System.Web.Mvc;
using PensionInsurance.Commands;

namespace PensionInsurance.Web.Views.Sales.ConsultingTracking
{
    public class CreateViewModel : CreateConsultingTrackingCommand
    {
        public IEnumerable<SelectListItem> ClientList { get; set; }
        public IEnumerable<SelectListItem> SalesPersonList { get; set; }
        public string CreatorName { get; set; }
        public int ConsultingTrackingId { get; set; }
        public string HeaderText { get; set; }
        public string ClientName { get; set; }
        public string Status { get; set; }
    }
}