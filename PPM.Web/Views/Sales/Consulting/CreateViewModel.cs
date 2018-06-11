using System.Collections.Generic;
using System.Web.Mvc;
using Foundation.Data;
using PensionInsurance.Commands;
using PensionInsurance.Entities;

namespace PensionInsurance.Web.Views.Sales.Consulting
{
    public class CreateViewModel : CreateConsultingCommand
    {
        public IEnumerable<SelectListItem> ProjectList { get; set; }
        public IEnumerable<SelectListItem> FocusOnList { get; set; }
        public IEnumerable<SelectListItem> HouseTypeList { get; set; }
        public IEnumerable<SelectListItem> MoveInRequirementList { get; set; }
        public IEnumerable<Entities.ConsultingTracking> Items { get; set; }
        public int? ConsultingId { get; set; }
        public string ConsultingName { get; set; }
        public int? ConsultingFamilyId { get; set; }
        public string PostUrl { get; set; }
        public string HeaderText { get; set; }
        public int? NextConsultingId { get; set; }
        public int? Page { get; set; }
        public string ConsultingAreaText { get; set; }
        public string ConsultingFamilyAreaText { get; set; }
        private readonly UrlHelper _urlHelper;
        
        public CreateViewModel(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public object DeleteCommand(int id)
        {
            return new Web.Common.WebCommand
            {
                Url = _urlHelper.Action("DeleteConsultingTracking", "Consulting"),
                Command = new DeleteConsultingTrackingCommand { ConsultingTrackingId = id, ConsultingId = ConsultingId.Value }
            };
        }
    }

    public class EditViewModel : EditConsultingCommand
    {
        public IEnumerable<Project> ProjectList { get; set; }
    }
}