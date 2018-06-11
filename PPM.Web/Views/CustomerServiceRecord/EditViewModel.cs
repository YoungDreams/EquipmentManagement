using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PensionInsurance.Commands;
using PensionInsurance.Entities;

namespace PensionInsurance.Web.Views.CustomerServiceRecord
{
    public class EditViewModel:EditServiceRecordCommand
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string ServiceRemark { get; set; }
        public string UnitPriceRemark { get; set; }
        public IEnumerable<ServicePackCatalog> ProjectServicePackCatalogs { get; set; }
        public IEnumerable<ConsumptiveMaterialService> ConsumptiveMaterialServices { get; set; }
    }
}