using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Foundation.Data;
using PensionInsurance.Entities;
using PensionInsurance.Entities.DetailViews;
using PensionInsurance.Web.Views.Home;

namespace PensionInsurance.Web.Views.UserBacklog
{
    public class IndexViewModel
    {
        public PagedData<Entities.UserBacklog> AllUserBacklogs { get; set; }
        public IEnumerable<Entities.UserBacklog> UndoUserBacklogs { get; set; }
        public PagedData<Entities.UserBacklog> DoneUserBacklogs { get; set; }
        public PagedData<WorkflowProgressDetail> UndoWorkflowProgresses { get; set; }
        public PagedData<WorkflowProgressDetail> DoneWorkflowProgresses { get; set; }
    }

    public static class WorkflowProgressUndoDetailExtensions
    {
        public static string GenarateActionUrl(this WorkflowProgressDetail viewModel, UrlHelper url)
        {
            switch (viewModel.WorkflowCategory)
            {
                case WorkflowCategory.入住合同:
                    return url.Action("Detail", "Contract", new { id = viewModel.RalatedId });
                case WorkflowCategory.费用变更增加:
                case WorkflowCategory.费用变更减少不累计额度:
                case WorkflowCategory.费用变更减少限额内:
                case WorkflowCategory.费用变更减少限额外:
                    return url.Action("Detail", "ContractCostChange", new { id = viewModel.RalatedId });
                case WorkflowCategory.服务包补充协议:
                    return url.Action("Detail", "ContractServicePackChange", new { id = viewModel.RalatedId });
                case WorkflowCategory.换房补充协议:
                    return url.Action("Detail", "ContractRoomChange", new { id = viewModel.RalatedId });
                case WorkflowCategory.客户退住退款:
                    return url.Action("Detail", "CustomerAccountCheckOutRefund", new { id = viewModel.RalatedId });
                case WorkflowCategory.账单减免不累计额度:
                case WorkflowCategory.账单减免累计限额内:
                case WorkflowCategory.账单减免累计限额外:
                    return url.Action("Detail", "ReliefBill", new { id = viewModel.RalatedId });
                case WorkflowCategory.转账:
                    return url.Action("Detail", "CustomerAccountTransfer", new { id = viewModel.RalatedId });
                case WorkflowCategory.客户退住通知单:
                    return url.Action("Show", "CustomerMoveOutTicket", new { id = viewModel.RalatedId });
                case WorkflowCategory.紧急采购审批:
                case WorkflowCategory.食材采购审批:
                case WorkflowCategory.计划采购审批:
                case WorkflowCategory.食材紧急采购审批:
                case WorkflowCategory.餐饮部计划采购审批:
                case WorkflowCategory.营销类计划采购审批:
                case WorkflowCategory.营销类特殊采购审批:
                case WorkflowCategory.营销类紧急采购审批:
                    return url.Action("Detail", "Order", new { id = viewModel.RalatedId });
                case WorkflowCategory.紧急采购验收:
                case WorkflowCategory.食材采购验收:
                case WorkflowCategory.计划采购验收:
                case WorkflowCategory.食材紧急采购验收:
                case WorkflowCategory.餐饮部计划采购验收:
                case WorkflowCategory.营销类计划采购验收:
                case WorkflowCategory.营销类特殊采购验收:
                case WorkflowCategory.营销类紧急采购验收:
                    return url.Action("Detail", "OrderAcceptance", new { id = viewModel.RalatedId });
                case WorkflowCategory.食材采购确认:
                    return url.Action("Detail", "PurchaseConfirm", new { id = viewModel.RalatedId });
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}