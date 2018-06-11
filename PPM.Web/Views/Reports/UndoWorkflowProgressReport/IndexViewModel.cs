using System.Collections.Generic;
using System.Web.Mvc;
using Foundation.Data;
using PensionInsurance.Entities.DetailViews;
using PensionInsurance.Query;

namespace PensionInsurance.Web.Views.Reports.UndoWorkflowProgressReport
{
    public class IndexViewModel
    {
        public UserBacklogQuery Query { get; set; }
        public PagedData<WorkflowProgressDetail> UndoWorkflowProgresses { get; set; }
        public IEnumerable<SelectListItem> ProjectList { get; set; }
        public IEnumerable<SelectListItem> WorkflowCategories { get; set; }
    }
}