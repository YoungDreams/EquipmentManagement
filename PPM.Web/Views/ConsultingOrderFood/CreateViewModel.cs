using System.Collections.Generic;
using System.Web.Mvc;
using PensionInsurance.Commands;
using PensionInsurance.Entities;

namespace PensionInsurance.Web.Views.ConsultingOrderFood
{
    public class CreateViewModel : CreateCustomerOrderFoodCommand
    {
        public int ProjectId { get; set; }
        public OrderFoodSourceType SourceType { get; set; }
        public int? ForeignkeyId { get; set; }
        public Dictionary<string, List<ServiceSelectListItem>> ProjectServicePackCatalogsByTypes { get; set; }
    }

    public class ServiceSelectListItem : SelectListItem
    {
        public string Json { get; set; }
    }
}