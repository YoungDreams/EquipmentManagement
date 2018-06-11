using System.Collections.Generic;
using System.Web.Mvc;
using PensionInsurance.Commands;
using PensionInsurance.Entities;

namespace PensionInsurance.Web.Views.SystemSetting.Employee
{
    public class CreateViewModel : CreateAndSubmitEmployeeCommand
    {
        public IEnumerable<SelectListItem> Projects { get; set; }
    }
}