using PensionInsurance.Commands;
using System.Collections.Generic;

namespace PensionInsurance.Web.Views.CustomerFamily
{
    public class EditViewModel : EditCustomerFamilyCommand
    {
        public int CustomerId { get; set; }
    }
}