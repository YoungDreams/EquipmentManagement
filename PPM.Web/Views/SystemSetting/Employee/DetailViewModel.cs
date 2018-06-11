using System;
using System.Collections.Generic;
using System.Web.Mvc;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Web.Views.TrackingResult;

namespace PensionInsurance.Web.Views.SystemSetting.Employee
{
    public class DetailViewModel : EditEmployeeCommand
    {
        public string ProjectName { get; set; }
        public string JobTypeName { get; set; }
        public string DepartmentName { get; set; }

        public TrackingResultViewModel TrackingResult { get; set; }
        public WorkflowStep CurrentWorkFlowStep { get; set; }
        public int CurrentWorkflowStepId { get; set; }
    }
}