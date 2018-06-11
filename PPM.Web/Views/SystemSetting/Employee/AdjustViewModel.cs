using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PensionInsurance.Commands;
using PensionInsurance.Entities;

namespace PensionInsurance.Web.Views.SystemSetting.Employee
{
    public class AdjustViewModel :AdjustEmployeeCommand
    {
        public HREmployee Employee { get; set; }
        public IEnumerable<SelectListItem> Projects { get; set; }

    }
}