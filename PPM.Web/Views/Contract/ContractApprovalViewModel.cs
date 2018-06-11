using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PensionInsurance.Entities;

namespace PensionInsurance.Web.Views.Contract
{
    public class ContractApprovalViewModel
    {
        public int ContractId { get; set; }
        public int Result { get; set; }
        public string Description { get; set; }
    }
}