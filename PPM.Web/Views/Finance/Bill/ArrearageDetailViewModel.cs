using PensionInsurance.Entities;
using System.Collections.Generic;

namespace PensionInsurance.Web.Views.Finance.Bill
{
    public class ArrearageDetailViewModel
    {
        public IEnumerable<CustomerBill> Items { get; set; }
        public Entities.Customer Customer { get; set; }
    }
}