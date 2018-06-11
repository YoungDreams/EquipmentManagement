using System.Collections.Generic;
using System.Web.Mvc;
using Foundation.Data;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Entities.DetailViews;
using PensionInsurance.Query;
using PensionInsurance.Web.Common;

namespace PensionInsurance.Web.Views.SystemSetting.Project
{
    public class IndexViewModel
    {
        private readonly UrlHelper _urlHelper;
        public IndexViewModel(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public ProjectQuery Query { get; set; }
        public PagedData<Entities.Project> Projects { get; set; }
        public List<Entities.HRJobType> JobOccupations { get; set; }

        public object DeleteCommand(int id)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("Delete", "Project"),
                Command = new DeleteEntityCommand { EntityId = id }
            };
        }
    }

    public class EditWorkflowContractViewModel
    {
        public int ProjectId { get; set; }
        public WorkflowCategory WorkflowCategory { get; set; }
        public IEnumerable<WorkflowDetail> WorkflowDetail { get; set; }
        public IEnumerable<SelectListItem> UserSelectListItems { get; set; }
        public IEnumerable<SelectListItem> Users { get; set; }
        public int? UploadUserId { get; set; }
        public int? ConfirmUserId { get; set; }
        public int? PrintUserId { get; set; }
        public int? ApartmentApprovalUserId { get; set; }
        public int? HousekeeperUserId { get; set; }//管家
        public int? EffectUserId { get; set; }
        public int? RefundUserId { get; set; }
        public int? PurchaseUserId { get; set; } //运营采购负责人
        public int? FinanceUserId { get; set; }//财务审批负责人
        public int? ProjectCaterersUserId { get; set; }//项目餐饮负责人

        /// <summary>
        /// 生活健康经理
        /// </summary>
        public int? LivingHealthManagerUserId { get; set; }
        public int? ApartmentGeneralManagerUserId { get; set; }//公寓总经理
        public int? DistrictGovernorUserId { get; set; }//大区总监
        public int? CenterSalaryManagerUserId { get; set; }//中心薪酬经理
        public int? CenterExecutiveUserId { get; set; }//中心人力行政主管
        public int? GeneralManagerUserId { get; set; }//公司总经理
        public int? EntryApprovalUserId { get; set; }//入职审批人
        /// <summary>
        /// 营销业务审批人
        /// </summary>
        public int? MarketingDirectorApprovalUserId { get; set; }
        /// <summary>
        /// 销售总监审批人
        /// </summary>
        public int? SalesApprovalUserId { get; set; }
        /// <summary>
        /// 市场总监审批人
        /// </summary>
        public int? MarketDirectorApprovalUserId { get; set; }
        /// <summary>
        /// 主管会计确认人
        /// </summary>
        public int? DirectorAccountingConfirmUserId { get; set; }
        /// <summary>
        /// 资产管理员审批人
        /// </summary>
        public int? AssetManagerUserId { get; set; }
    }
}