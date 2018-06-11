using System.Collections.Generic;
using System.Web.Mvc;
using Foundation.Data;
using PensionInsurance.Commands;
using PensionInsurance.Query;
using PensionInsurance.Web.Common;

namespace PensionInsurance.Web.Views.SystemSetting.Department
{
    public class IndexViewModel
    {
        private readonly UrlHelper _urlHelper;
        public IndexViewModel(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }
        
        public DepartmentQuery Query { get; set; }
        public PagedData<Entities.Department> Items { get; set; }
        public IEnumerable<SelectListItem> Projects { get; set; }

        public object DeleteCommand(int id)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("Delete", "Department"),
                Command = new DeleteDepartmentCommand { Id = id }
            };
        }
    }
}