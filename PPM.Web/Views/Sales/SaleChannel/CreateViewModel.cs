using System.Collections.Generic;
using System.Web.Mvc;
using PensionInsurance.Commands;
using PensionInsurance.Entities;

namespace PensionInsurance.Web.Views.Sales.SaleChannel
{
    public class CreateViewModel : CreateSaleChannelCommand
    {
        public IEnumerable<SelectListItem> ProjectList { get; set; }
        public string PostUrl { get; set; }
        public string HeaderText { get; set; }
        public string CreatedTime { get; set; }
        public string CreatorName { get; set; }
    }
}