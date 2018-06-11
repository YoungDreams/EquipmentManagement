using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PensionInsurance.Entities;

namespace PensionInsurance.Web.Views.Contract
{
    public class ChooseContractTemplateViewModel
    {
        public int CustomerId { get; set; }
        public ContractTemplate ContractTemplate { get; set; }
        public int? PreviousContractId { get; set; }
    }
}