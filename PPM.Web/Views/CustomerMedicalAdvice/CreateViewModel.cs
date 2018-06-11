using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PensionInsurance.Commands;
using PensionInsurance.Entities;

namespace PensionInsurance.Web.Views.CustomerMedicalAdvice
{
    public class CreateViewModel :CreateCustomerMedicalAdviceCommand
    {
        public Entities.Customer Customer { get; set; }

    }
}