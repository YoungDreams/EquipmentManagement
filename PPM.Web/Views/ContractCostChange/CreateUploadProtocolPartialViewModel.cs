using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PensionInsurance.Entities;

namespace PensionInsurance.Web.Views.ContractCostChange
{
    public class CreateUploadProtocolPartialViewModel
    {
        public string Title { get; set; }
        public int RalatedId { get; set; }
        public AdditionalType Type { get; set; }
        public int CustomerAccountId { get; set; }

    }
}