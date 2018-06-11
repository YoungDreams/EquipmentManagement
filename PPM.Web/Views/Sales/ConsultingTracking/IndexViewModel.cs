using System.Collections.Generic;
using System.Web.Mvc;
using Foundation.Data;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Entities.DetailViews;
using PensionInsurance.Query;
using PensionInsurance.Web.Common;

namespace PensionInsurance.Web.Views.Sales.ConsultingTracking
{
    public class IndexViewModel
    {
        public List<User> Sales { get; set; }
        public IEnumerable<SelectListItem> ProjectList { get; set; }
        private readonly UrlHelper _urlHelper;

        public IndexViewModel(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public ConsultingTrackingQuery Query { get; set; }
        public PagedData<ConsultingTrackingDetail> Items { get; set; }

        public object DeleteCommand(int id)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("Delete", "ConsultingTracking"),
                Command = new DeleteConsultingTrackingCommand { ConsultingTrackingId = id}
            };
        }
    }
}   