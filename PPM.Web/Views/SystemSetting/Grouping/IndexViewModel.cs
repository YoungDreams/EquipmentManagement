using System.Web.Mvc;
using Foundation.Data;
using PensionInsurance.Commands;
using PensionInsurance.Query;
using PensionInsurance.Web.Common;

namespace PensionInsurance.Web.Views.SystemSetting.Grouping
{
    public class IndexViewModel
    {
        private readonly UrlHelper _urlHelper;
        public IndexViewModel(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }
        public GroupingQuery Query { get; set; }
        public PagedData<GroupingItemViewmodel> Groupings { get; set; }
        public object DeleteCommand(int id, int departmentId, string strUrl)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("Delete", "Grouping"),
                Command = new DeleteGroupingCommand { Id = id, DepartmentId = departmentId, ReturnUrl = strUrl }
            };
        }
    }

    public class GroupingItemViewmodel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int? ParentId { get; set; }
        public string ParentOrganizationName { get; set; }
        public int Sort { get; set; }
        public int Layer { get; set; }
        public int ProjectId { get; set; }
        public string ProjectNo { get; set; }
        public string ProjectName { get; set; }
        public int DepartementId { get; set; }
        public string DepartementName { get; set; }
    }
}