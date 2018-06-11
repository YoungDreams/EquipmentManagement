using System.Collections.Generic;
using System.Web.Mvc;
using Foundation.Data;
using PensionInsurance.Entities;
using PensionInsurance.Query;

namespace PensionInsurance.Web.Views.Reports.ContractLedgerReport
{
    public class IndexViewModel
    {
        public ContractLedgerReportQuery Query { get; set; }
        public IEnumerable<ContractLedgerReportDetail> ContractLedgers { get; set; }
        public IEnumerable<SelectListItem> ProjectList { get; set; }
        public int QueryTimeMonths { get; set; }
    }
}