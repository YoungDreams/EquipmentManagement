using System.Collections.Generic;
using System.Web.Mvc;
using Foundation.Data;
using PensionInsurance.Query;
using PensionInsurance.Web.Common;
using PensionInsurance.Commands;

namespace PensionInsurance.Web.Views.SystemSetting.Employee
{
    public class IndexViewModel
    {
        private readonly UrlHelper _urlHelper;
        public IndexViewModel(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }
        public EmployeeQuery Query { get; set; }
        public PagedData<Entities.HREmployee> Items { get; set; }
        public IEnumerable<SelectListItem> JobTypes { get; set; }
        public IEnumerable<SelectListItem> Departments { get; set; }

        public object DeleteCommand(int id, string strUrl)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("Delete", "Employee"),
                Command = new DeleteEmployeeCommand { Id = id, ReturnUrl = strUrl }
            };
        }
    }
}