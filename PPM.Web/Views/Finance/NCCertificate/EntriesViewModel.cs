using System.Collections.Generic;
using Foundation.Data;
using PensionInsurance.Entities.DetailViews;

namespace PensionInsurance.Web.Views.Finance.NCCertificate
{
    public class EntriesViewModel
    {
        public IEnumerable<NCCertificateDetail> NCCertificateEntries { get; set; }
    }
}