using System;

namespace PensionInsurance.Web.Views.Contract
{
    public class DeleteContractViewModel
    {
        public int ContractId { get; set; }
        public int CustomerAccountId { get; set; }
        public string ContractNo { get; set; }
        public string CustomerName { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}