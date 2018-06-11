using System.Collections.Generic;
using System.Web.Mvc;
using PensionInsurance.Commands;
using PensionInsurance.Entities;

namespace PensionInsurance.Web.Views.CustomerOrderFood
{
    public class CreateViewModel : CreateCustomerOrderFoodCommand
    {
        public int ProjectId { get; set; }
        public string ServiceLevel { get; set; }
        public IEnumerable<ProjectServicePackCatalog> ProjectServicePackCatalogs { get; set; }
        public Dictionary<string, List<ServiceSelectListItem>> ProjectServicePackCatalogsByTypes { get; set; }
        public ServiceRecordType ServiceRecordType { get; set; }
    }

    public class ServiceSelectListItem : SelectListItem
    {
        public string Json { get; set; }
    }
}