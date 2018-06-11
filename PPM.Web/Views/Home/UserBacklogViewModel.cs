using System;
using System.Web.Mvc;
using PensionInsurance.Entities;

namespace PensionInsurance.Web.Views.Home
{
    public class UserBacklogViewModel
    {
        public User User { get; set; }
        public UserBacklogStatus UserBacklogStatus { get; set; }
        public UserBacklogCategory UserBacklogCategory { get; set; }
        public int ActionId { get; set; }
        public UserBacklogActionType UserBacklogActionType { get; set; }
        public string Description { get; set; }
        public DateTime CreateOn { get; set; }
    }

    public static class UserBacklogViewModelExtensions
    {
        public static string GenarateActionUrl(this Entities.UserBacklog viewModel, UrlHelper url)
        {
            if (viewModel.UserBacklogCategory == UserBacklogCategory.合同)
            {
                if (viewModel.UserBacklogStatus == UserBacklogStatus.已处理)
                {
                    return url.Action("Detail", "Contract", new { id = viewModel.ActionId });
                }

                switch (viewModel.UserBacklogActionType)
                {
                    case UserBacklogActionType.提交:
                        return url.Action("Edit", "Contract", new { id = viewModel.ActionId });
                    case UserBacklogActionType.审批:
                    case UserBacklogActionType.打印签字:
                    case UserBacklogActionType.生效:
                    case UserBacklogActionType.扫描件上传:
                    case UserBacklogActionType.扫描件确认:
                        return url.Action("Detail", "Contract", new { id = viewModel.ActionId });
                    default:
                        return "javascript:;";
                }
            }
            if (viewModel.UserBacklogCategory == UserBacklogCategory.服务包补充协议)
            {
                if (viewModel.UserBacklogStatus == UserBacklogStatus.已处理)
                {
                    return url.Action("Detail", "ContractServicePackChange", new { id = viewModel.ActionId });
                }

                switch (viewModel.UserBacklogActionType)
                {
                    case UserBacklogActionType.提交:
                        return url.Action("Edit", "ContractServicePackChange", new { id = viewModel.ActionId });
                    case UserBacklogActionType.审批:
                    case UserBacklogActionType.打印签字:
                    case UserBacklogActionType.生效:
                    case UserBacklogActionType.扫描件上传:
                    case UserBacklogActionType.扫描件确认:
                        return url.Action("Detail", "ContractServicePackChange", new { id = viewModel.ActionId });
                    default:
                        return "javascript:;";
                }
            }
            if (viewModel.UserBacklogCategory == UserBacklogCategory.换房补充协议)
            {
                if (viewModel.UserBacklogStatus == UserBacklogStatus.已处理)
                {
                    return url.Action("Detail", "ContractRoomChange", new { id = viewModel.ActionId });
                }

                switch (viewModel.UserBacklogActionType)
                {
                    case UserBacklogActionType.提交:
                        return url.Action("Edit", "ContractRoomChange", new { id = viewModel.ActionId });
                    case UserBacklogActionType.审批:
                    case UserBacklogActionType.打印签字:
                    case UserBacklogActionType.生效:
                    case UserBacklogActionType.扫描件上传:
                    case UserBacklogActionType.扫描件确认:
                        return url.Action("Detail", "ContractRoomChange", new { id = viewModel.ActionId });
                    default:
                        return "javascript:;";
                }
            }
            if (viewModel.UserBacklogCategory == UserBacklogCategory.客户退住退款)
            {
                if (viewModel.UserBacklogStatus == UserBacklogStatus.已处理)
                {
                    return url.Action("Detail", "CustomerAccountCheckOutRefund", new { id = viewModel.ActionId });
                }

                switch (viewModel.UserBacklogActionType)
                {
                    case UserBacklogActionType.提交:
                        return url.Action("Edit", "CustomerAccountCheckOutRefund", new { id = viewModel.ActionId });
                    case UserBacklogActionType.审批:
                    case UserBacklogActionType.扫描件上传:
                    case UserBacklogActionType.扫描件确认:
                    case UserBacklogActionType.退款:
                        return url.Action("Detail", "CustomerAccountCheckOutRefund", new { id = viewModel.ActionId });
                    default:
                        return "javascript:;";
                }
            }
            if (viewModel.UserBacklogCategory == UserBacklogCategory.费用变更补充协议)
            {
                if (viewModel.UserBacklogStatus == UserBacklogStatus.已处理)
                {
                    return url.Action("Detail", "ContractCostChange", new { id = viewModel.ActionId });
                }

                switch (viewModel.UserBacklogActionType)
                {
                    case UserBacklogActionType.提交:
                        return url.Action("Edit", "ContractCostChange", new { id = viewModel.ActionId });
                    case UserBacklogActionType.审批:
                    case UserBacklogActionType.打印签字:
                    case UserBacklogActionType.生效:
                    case UserBacklogActionType.扫描件上传:
                    case UserBacklogActionType.扫描件确认:
                        return url.Action("Detail", "ContractCostChange", new { id = viewModel.ActionId });
                    default:
                        return "javascript:;";
                }
            }

            if (viewModel.UserBacklogCategory == UserBacklogCategory.账单减免)
            {
                if (viewModel.UserBacklogStatus == UserBacklogStatus.已处理)
                {
                    return url.Action("Detail", "ReliefBill", new { id = viewModel.ActionId });
                }

                switch (viewModel.UserBacklogActionType)
                {
                    case UserBacklogActionType.提交:
                        return url.Action("Edit", "ReliefBill", new { id = viewModel.ActionId });
                    case UserBacklogActionType.审批:
                        return url.Action("Detail", "ReliefBill", new { id = viewModel.ActionId });
                    default:
                        return "javascript:;";
                }
            }

            if (viewModel.UserBacklogCategory == UserBacklogCategory.客户退住通知单)
            {
                if (viewModel.UserBacklogStatus == UserBacklogStatus.已处理)
                {
                    return url.Action("Detail", "CustomerMoveOutTicket", new { id = viewModel.ActionId });
                }

                switch (viewModel.UserBacklogActionType)
                {
                    case UserBacklogActionType.填写退住总结:
                        return url.Action("Create", "CustomerMoveOutTicket", new { id = viewModel.ActionId });
                    case UserBacklogActionType.填写退住意见:
                        return url.Action("Opinion", "CustomerMoveOutTicket", new { id = viewModel.ActionId });
                    default:
                        return "javascript:;";
                }
            }

            if (viewModel.UserBacklogCategory == UserBacklogCategory.转账)
            {
                if (viewModel.UserBacklogStatus == UserBacklogStatus.已处理)
                {
                    return url.Action("Detail", "CustomerAccountTransfer", new { id = viewModel.ActionId });
                }

                switch (viewModel.UserBacklogActionType)
                {
                    case UserBacklogActionType.提交:
                        return url.Action("Edit", "CustomerAccountTransfer", new { id = viewModel.ActionId });
                    case UserBacklogActionType.审批:
                    case UserBacklogActionType.打印签字:
                    case UserBacklogActionType.扫描件上传:
                    case UserBacklogActionType.扫描件确认:
                        return url.Action("Detail", "CustomerAccountTransfer", new { id = viewModel.ActionId });
                    default:
                        return "javascript:;";
                }
            }
            if (viewModel.UserBacklogCategory == UserBacklogCategory.采购)
            {
                if (viewModel.UserBacklogStatus == UserBacklogStatus.已处理)
                {
                    return url.Action("Detail", "Order", new { id = viewModel.ActionId });
                }

                switch (viewModel.UserBacklogActionType)
                {
                    case UserBacklogActionType.重新提交:
                        return url.Action("AgainEdit", "Order", new { id = viewModel.ActionId });
                    case UserBacklogActionType.公寓审批:
                    case UserBacklogActionType.财务总监审批:
                    case UserBacklogActionType.运营采购审批:
                    case UserBacklogActionType.营销业务审批:
                    case UserBacklogActionType.销售总监审批:
                    case UserBacklogActionType.市场总监审批:
                    case UserBacklogActionType.入库确认:
                        return url.Action("Detail", "Order", new { id = viewModel.ActionId });
                    default:
                        return "javascript:;";
                }
            }

            if (viewModel.UserBacklogCategory == UserBacklogCategory.采购验收)
            {
                if (viewModel.UserBacklogStatus == UserBacklogStatus.已处理)
                {
                    return url.Action("Detail", "OrderAcceptance", new { id = viewModel.ActionId });
                }

                switch (viewModel.UserBacklogActionType)
                {
                    case UserBacklogActionType.管家入库:
                    case UserBacklogActionType.资产管理员入库:
                        return url.Action("Edit", "OrderAcceptance", new { id = viewModel.ActionId });
                    case UserBacklogActionType.重新提交:
                        return url.Action("Edit", "OrderAcceptance", new { id = viewModel.ActionId });
                    case UserBacklogActionType.公寓审批:
                    case UserBacklogActionType.营销业务审批:
                    case UserBacklogActionType.入库确认:
                        return url.Action("Detail", "OrderAcceptance", new { id = viewModel.ActionId });
                    default:
                        return "javascript:;";
                }
            }

            if (viewModel.UserBacklogCategory == UserBacklogCategory.食材采购确认单)
            {
                if (viewModel.UserBacklogStatus == UserBacklogStatus.已处理)
                {
                    return url.Action("Detail", "PurchaseConfirm", new { id = viewModel.ActionId });
                }

                switch (viewModel.UserBacklogActionType)
                {
                    case UserBacklogActionType.运营采购审批:
                        return url.Action("Detail", "PurchaseConfirm", new { id = viewModel.ActionId });
                    default:
                        return "javascript:;";
                }
            }

            if (viewModel.UserBacklogCategory == UserBacklogCategory.入职审批)
            {
                if (viewModel.UserBacklogStatus == UserBacklogStatus.已处理)
                {
                    return url.Action("Detail", "Employee", new { id = viewModel.ActionId });
                }

                switch (viewModel.UserBacklogActionType)
                {
                    case UserBacklogActionType.提交:
                        return url.Action("Edit", "Employee", new { id = viewModel.ActionId });
                    case UserBacklogActionType.审批:
                        return url.Action("Detail", "Employee", new { id = viewModel.ActionId });
                    default:
                        return "javascript:;";
                }
            }
            if (viewModel.UserBacklogCategory == UserBacklogCategory.入职办理)
            {
                if (viewModel.UserBacklogStatus == UserBacklogStatus.已处理)
                {
                    return url.Action("EntryDetail", "Employee", new { id = viewModel.ActionId });
                }

                switch (viewModel.UserBacklogActionType)
                {
                    case UserBacklogActionType.提交:
                        return url.Action("Entry", "Employee", new { id = viewModel.ActionId });
                    case UserBacklogActionType.审批:
                        return url.Action("EntryDetail", "Employee", new { id = viewModel.ActionId });
                    default:
                        return "javascript:;";
                }
            }
            if (viewModel.UserBacklogCategory == UserBacklogCategory.盘点)
            {
                if (viewModel.UserBacklogStatus == UserBacklogStatus.已处理)
                {
                    return url.Action("CheckingProjectItem", "Checking", new { ProjectId = viewModel.Project.Id, CheckingProjectId = viewModel.ActionId });
                }

                switch (viewModel.UserBacklogActionType)
                {
                    case UserBacklogActionType.提交:
                        return url.Action("CheckingProjectItem", "Checking", new { ProjectId = viewModel.Project.Id, CheckingProjectId = viewModel.ActionId, BacklogId = viewModel.Id });
                    case UserBacklogActionType.重新提交:
                        return url.Action("CheckingProjectItem", "Checking", new { ProjectId = viewModel.Project.Id, CheckingProjectId = viewModel.ActionId, BacklogId = viewModel.Id, IsResubmit = true });
                    case UserBacklogActionType.公寓审批:
                    case UserBacklogActionType.主管会计确认:
                        return url.Action("CheckingApprovalDetail", "Checking", new { id = viewModel.ActionId });
                    case UserBacklogActionType.在用资产盘点:
                        return url.Action("AssetInUseDetail", "Checking", new { id = viewModel.ActionId });
                    default:
                        return "javascript:;";
                }
            }

            return string.Empty;
        }

        public static string GenarateActionIcon(this Entities.UserBacklog viewModel)
        {
            return " fa-folder-o";
        }

    }
}