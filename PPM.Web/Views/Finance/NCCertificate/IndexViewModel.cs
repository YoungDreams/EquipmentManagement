using System.Collections.Generic;
using Foundation.Data;
using System.Web.Mvc;
using PensionInsurance.Web.Common;
using PensionInsurance.Commands;

namespace PensionInsurance.Web.Views.Finance.NCCertificate
{
    public class IndexViewModel
    {
        private readonly UrlHelper _urlHelper;
        public IndexViewModel(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public IEnumerable<SelectListItem> ProjectList { get; set; }
        public PagedData<Entities.NCCertificate> NCCertificates { get; set; }

        public WebCommand ImportCommand(int id)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("Import", "NCCertificate"),
                Command = new ImportNCCommand { NCCertificateId = id }
            };
        }

        public WebCommand DeleteCommand(int id)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("Delete", "NCCertificate"),
                Command = new DeleteNCCommand { NCCertificateId = id }
            };
        }
    }
}