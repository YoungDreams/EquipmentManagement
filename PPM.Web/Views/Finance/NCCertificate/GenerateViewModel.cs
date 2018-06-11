using System;
using System.Collections.Generic;
using System.Web.Mvc;
using PensionInsurance.Entities;

namespace PensionInsurance.Web.Views.Finance.NCCertificate
{
    public class GenerateViewModel
    {
        public string Title { get; set; }
        public int ProjectId { get; set; }
        public NCCertificateType NCCertificateType { get; set; }
        public DateTime NCCertificateMonth { get; set; }
        public DateTime PreparedDate { get; set; }
        public IEnumerable<SelectListItem > ProjectList { get; set; }
    }
}