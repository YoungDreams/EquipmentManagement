using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PensionInsurance.Entities;
using PensionInsurance.Query;

namespace PensionInsurance.Web.Views.Reports.OperatorConsultingReport
{
    public class IndexViewModel
    {
        public IEnumerable<SelectListItem> ProjectList { get; set; }
        public OperatorConsultingReportQurey Query { get; set; }
    }

    public class TotalViewModel : IndexViewModel
    {
        public IList<OperatorConsultingTotalReport> Report { get; set; }
    }

    public class OperatorsViewModel : IndexViewModel
    {
        public IList<OperatorConsultingOperatorsReport> Report { get; set; }
    }

    public class ProjectsViewModel : IndexViewModel
    {
        public IList<OperatorConsultingProjectsReport> Report { get; set; }
    }

    
}