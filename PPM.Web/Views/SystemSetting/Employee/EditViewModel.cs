using System;
using System.Collections.Generic;
using System.Web.Mvc;
using PensionInsurance.Commands;
using PensionInsurance.Web.Views.TrackingResult;

namespace PensionInsurance.Web.Views.SystemSetting.Employee
{
    public class EditViewModel : EditEmployeeCommand
    {
        public IEnumerable<SelectListItem> Projects { get; set; }
        public TrackingResultViewModel TrackingResult { get; set; }
    }
}