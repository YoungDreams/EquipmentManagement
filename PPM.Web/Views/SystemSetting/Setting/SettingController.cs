using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Windows.Interop;
using Foundation.Data;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Entities.Exceptions;
using PensionInsurance.Query;
using PensionInsurance.Shared;
using PensionInsurance.Web.Views.Account;
using PensionInsurance.Workflows;
using WebGrease.Css.Ast.Selectors;

namespace PensionInsurance.Web.Views.SystemSetting.Setting
{
    public class SettingController : AuthorizedController
    {
        private readonly ISettingQueryService _settingQuery;
        private readonly ICommandService _commandService;
        private readonly IRepository _repository;
        private readonly IUserBacklogProcess _userBacklogProcess;

        public SettingController(ISettingQueryService settingQuery, ICommandService commandService, IRepository repository, IUserBacklogProcess userBacklogProcess)
        {
            _settingQuery = settingQuery;
            _commandService = commandService;
            _repository = repository;
            _userBacklogProcess = userBacklogProcess;
        }
        /// <summary>
        /// 数据字典查询分页
        /// </summary>
        /// <param name="page">当前页</param>
        /// <param name="pageSize">分页数</param>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public ActionResult Index(int page = 1, int pageSize = Web.Common.PaginationSetttings.PageSize, SettingQuery query = null)
        {
            var viewModel = new IndexViewModel(Url)
            {
                Query = query,
                Settings = _settingQuery.Query(page, pageSize, query)
            };
            return View("~/Views/SystemSetting/Setting/Index.cshtml", viewModel);
        }
        /// <summary>
        /// 创建数据字典/新增视图
        /// </summary>
        /// <returns>视图</returns>
        [HttpGet]
        public ActionResult Create()
        {
            var viewModel = new CreateViewModel();
            return View("~/Views/SystemSetting/Setting/Create.cshtml", viewModel);
        }
        /// <summary>
        /// 新增数据字典
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(CreateSettingCommand command)
        {
            _commandService.Execute(command);

            return RedirectToAction("Index", new SettingQuery
            {
                SettingType = command.Type
            });
        }
        /// <summary>
        /// 删除数据字典
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(DeleteEntityCommand command)
        {
            Entities.Setting setting = _settingQuery.Get(command.EntityId);
            if (setting == null)
                throw new ApplicationException("Setting cannot be found");

            _commandService.Execute(DeleteEntityCommand.Of(setting));

            return RedirectToAction("Index", new SettingQuery
            {
                SettingType = setting.Type
            });
        }

        //public ActionResult UpdateUserBaklog(string orderNo)
        //{
        //    var purchaseOrder = _repository.Query<PurchaseOrder>().FirstOrDefault(m => m.OrderNo == orderNo.Trim());
        //    var workflow = GetWorkflow(purchaseOrder);
        //    var workflowStep = GetHousekeeperApprovalWorkflowStep(workflow);
        //    var userbackLog = _repository.Query<Entities.UserBacklog>().FirstOrDefault(m =>
        //        m.User.Id == purchaseOrder.User.Id && m.UserBacklogCategory == UserBacklogCategory.采购 && m.UserBacklogActionType ==
        //        UserBacklogActionType.提交 && m.ActionId == purchaseOrder.Id);
        //    if (workflowStep == null || userbackLog == null) throw new Exception("null exception");
        //    _repository.Execute(@"update [UserBacklog] set UserId = " + workflowStep.User.Id + ",UserBacklogActionType = 26 where Id=" + userbackLog.Id);

        //    WorkflowProgress workflowProgress = new WorkflowProgress
        //    {
        //        Workflow = workflow,
        //        RalatedId = purchaseOrder.Id,
        //        Status = WorkflowProgressStatus.开启,
        //        CreatedBy = WebAppContext.Current.User.RealName
        //    };
        //    _repository.Create(workflowProgress);

        //    return Content("OK");
        //}

        //private Workflow GetWorkflow(PurchaseOrder order)
        //{
        //    WorkflowCategory workflowCategory = WorkflowCategory.计划采购验收;

        //    if (order.OrderType == OrderType.运营紧急采购)
        //    {
        //        workflowCategory = WorkflowCategory.紧急采购验收;
        //    }
        //    if (order.OrderType == OrderType.食材紧急采购)
        //    {
        //        workflowCategory = WorkflowCategory.食材紧急采购验收;
        //    }
        //    if (order.OrderType == OrderType.食材采购单)
        //    {
        //        workflowCategory = WorkflowCategory.食材采购验收;
        //    }
        //    if (order.OrderType == OrderType.餐饮部计划采购)
        //    {
        //        workflowCategory = WorkflowCategory.餐饮部计划采购验收;
        //    }

        //    var workflow =
        //        _repository.Query<Entities.Workflow>()
        //            .FirstOrDefault(x => x.Project == order.Project && x.WorkflowCategory == workflowCategory && x.Status == WorkflowStatus.启用);
        //    return workflow;
        //}

        //private WorkflowStep GetHousekeeperApprovalWorkflowStep(Workflow workflow)
        //{
        //    WorkflowStep workflowStep = _repository.Query<WorkflowStep>()
        //        .Where(x => x.Workflow.Id == workflow.Id && x.WorkflowStepCategory == WorkflowStepCategory.管家入库)
        //        .OrderBy(x => x.Step)
        //        .FirstOrDefault();
        //    return workflowStep;
        //}

        public ActionResult UpdateOrderByDone()
        {
            var time = new DateTime(2018, 03, 16);
            var strmsg = "已处理：";
            var num = 0;
            var orderIds = _repository.Query<PurchaseOrderAcceptance>().Select(s => s.PurchaseOrder.Id).Distinct().ToList();

            var orders = _repository.Query<PurchaseOrder>().Where(x => x.Status == OrderStatus.已完成 && x.OrderDate < time && !orderIds.Contains(x.Id)).ToList();

            foreach (var item in orders)
            {
                var workflow = GetCheckSubmitUser(item).Workflow;
                if (workflow == null)
                {
                    continue;
                }
                var workflowProgress = _repository.Query<WorkflowProgress>().Where(x => x.RalatedId == item.Id && x.CreatedOn < time && x.Workflow == workflow).ToList();

                GenerateOrderAcceptance(item, WebAppContext.Current.User, workflowProgress);

                foreach (var wp in workflowProgress)
                {
                    foreach (var r in wp.WorkflowTrackingResults)
                    {
                        _repository.Execute(@"delete from [WorkflowTrackingResult]  where Id=" + r.Id);
                    }
                    _repository.Execute(@"delete from [WorkflowProgress]  where Id=" + wp.Id);
                }
                strmsg += "订单号：" + item.OrderNo + ",";
                num++;
            }

            return Content("总计：" + num + "条数据，" + strmsg);
        }

        /// <summary>
        /// 将采购供应商的状态改成 有效。
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateSupplierStatus()
        {
            _repository.Execute(@"update [PurchaseSupplier] set Status = 1");
            return Content("Ok");
        }

        ///// <summary>
        ///// 修改已审批的订单
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult UpdateOrderByApproval()
        //{
        //    var time = new DateTime(2018, 03, 16);
        //    var strmsg = "已处理：";
        //    var num = 0;
        //    var orderIds = _repository.Query<PurchaseOrderAcceptance>().Select(s => s.PurchaseOrder.Id).Distinct();

        //    var orders = _repository.Query<PurchaseOrder>().Where(x => x.Status == OrderStatus.已审批 && x.OrderDate < time && !orderIds.Contains(x.Id));

        //    foreach (var item in orders)
        //    {
        //        var workflow = GetCheckSubmitUser(item).Workflow;
        //        var checkuser = GetCheckSubmitUser(item).User;
        //        //删除之前的代办
        //        var userbacklog = _repository
        //            .Query<Entities.UserBacklog>().FirstOrDefault(x => x.UserBacklogCategory == UserBacklogCategory.采购
        //            && x.ActionId == item.Id
        //            && (x.User==item.User|| x.User== checkuser)
        //            && x.UserBacklogActionType == UserBacklogActionType.管家入库);



        //        var workflowProgress = _repository.Query<WorkflowProgress>()
        //            .FirstOrDefault(x => x.RalatedId == item.Id && x.Workflow == workflow && x.Status == WorkflowProgressStatus.开启);
        //        if (userbacklog != null)
        //        {
        //            _repository.Execute(@"delete from [UserBacklog]  where Id=" + userbacklog.Id);
        //        }
        //        if (workflowProgress != null)
        //        {
        //            var workflowResults = workflowProgress.WorkflowTrackingResults;
        //            foreach (var result in workflowResults)
        //            {
        //                _repository.Execute(@"delete from [WorkflowTrackingResult]  where Id=" + result.Id);
        //            }
        //            //删除审批记录与进行中的审批流程记录
        //            _repository.Execute(@"delete from [WorkflowProgress]  where Id=" + workflowProgress.Id); 
        //        }
        //        //拆分验收单，发送代办
        //        GenerateOrderAcceptance(item, WebAppContext.Current.User);
        //        strmsg += "订单号：" + item.OrderNo;
        //        num++;
        //        _repository.Execute(@"update [PurchaseOrder] set IsUpdate = 1 where Id=" + item.Id);
        //    }

        //    return Content("总计：" + num + "条数据，" + strmsg);
        //}

        ///// <summary>
        ///// 修改已审批的订单
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult UpdateOrderByCheck()
        //{
        //    var time = new DateTime(2018, 03, 16);
        //    var strmsg = "已处理：";
        //    var num = 0;

        //    var orderIds = _repository.Query<PurchaseOrderAcceptance>().Select(s => s.PurchaseOrder.Id).Distinct();

        //    var orders = _repository.Query<PurchaseOrder>().Where(x => x.Status == OrderStatus.待验收 && x.OrderDate < time && !orderIds.Contains(x.Id));

        //    foreach (var item in orders)
        //    {
        //        var workflow = GetCheckSubmitUser(item).Workflow;
        //        var checkuser = GetCheckSubmitUser(item).User;

        //        var workflowProgress = _repository.Query<WorkflowProgress>()
        //            .FirstOrDefault(x => x.RalatedId == item.Id && x.Workflow == workflow && x.Status == WorkflowProgressStatus.开启);

        //        //删除管家入库的代办
        //        var userbacklog = _repository
        //            .Query<Entities.UserBacklog>().FirstOrDefault(x => x.UserBacklogCategory == UserBacklogCategory.采购
        //                                                               && x.ActionId == item.Id
        //                                                               && x.User== checkuser
        //                                                               && x.UserBacklogActionType == UserBacklogActionType.管家入库);

        //        var olduserbacklog = _repository
        //            .Query<Entities.UserBacklog>().FirstOrDefault(x => x.UserBacklogCategory == UserBacklogCategory.采购
        //                                                               && x.ActionId == item.Id
        //                                                               && x.User == item.User
        //                                                               && x.UserBacklogActionType == UserBacklogActionType.管家入库);
        //        //删除入库确认的代办
        //        var createuserbacklog = _repository
        //            .Query<Entities.UserBacklog>().FirstOrDefault(x =>  x.UserBacklogCategory == UserBacklogCategory.采购
        //                                                               && x.ActionId == item.Id
        //                                                               && x.User==item.User
        //                                                               && x.UserBacklogActionType == UserBacklogActionType.入库确认);

        //        if (olduserbacklog != null)
        //        {
        //            _repository.Execute(@"delete from [UserBacklog]  where Id=" + olduserbacklog.Id);
        //        }
        //        if (createuserbacklog != null)
        //        {
        //            _repository.Execute(@"delete from [UserBacklog]  where Id=" + createuserbacklog.Id);
        //        }
        //        if (userbacklog != null)
        //        {
        //            _repository.Execute(@"delete from [UserBacklog]  where Id=" + userbacklog.Id);
        //        }
        //        if (workflowProgress != null)
        //        {
        //            var workflowResults = workflowProgress.WorkflowTrackingResults;
        //            foreach (var result in workflowResults)
        //            {
        //                _repository.Execute(@"delete from [WorkflowTrackingResult]  where Id=" + result.Id);
        //            }
        //            //删除审批记录与进行中的审批流程记录
        //            _repository.Execute(@"delete from [WorkflowProgress]  where Id=" + workflowProgress.Id);
        //            _repository.Execute(@"update [PurchaseOrder] set IsUpdate = 1 where Id=" + item.Id);
        //        }
        //        //拆分验收单，发送代办
        //        GenerateOrderAcceptance(item, WebAppContext.Current.User);
        //        strmsg += "订单号：" + item.OrderNo;
        //        num++;
        //    }

        //    return Content("总计：" + num + "条数据，" + strmsg);
        //}

        public ActionResult Export(ExportCustomerAccountDetailLedgerReportCommand command)
        {
            var result = _commandService.ExecuteFoResult(command);

            return Json(new
            {
                success = result.IsSucceed,
                redirect = $"{Url.Content("~/Attachments/Report/")}{result.FileName}"
            }, JsonRequestBehavior.AllowGet);
        }


        private WorkflowStep GetCheckSubmitUser(PurchaseOrder order)
        {
            WorkflowCategory workflowCategory = WorkflowCategory.计划采购验收;

            if (order.OrderType == OrderType.运营紧急采购)
            {
                workflowCategory = WorkflowCategory.紧急采购验收;
            }
            if (order.OrderType == OrderType.食材紧急采购)
            {
                workflowCategory = WorkflowCategory.食材紧急采购验收;
            }
            if (order.OrderType == OrderType.食材采购单)
            {
                workflowCategory = WorkflowCategory.食材采购验收;
            }
            if (order.OrderType == OrderType.餐饮部计划采购)
            {
                workflowCategory = WorkflowCategory.餐饮部计划采购验收;
            }
            if (order.OrderType == OrderType.营销类计划性采购)
            {
                workflowCategory = WorkflowCategory.营销类计划采购验收;
            }
            if (order.OrderType == OrderType.营销类紧急性采购)
            {
                workflowCategory = WorkflowCategory.营销类紧急采购验收;
            }
            if (order.OrderType == OrderType.营销类特殊性采购)
            {
                workflowCategory = WorkflowCategory.营销类特殊采购验收;
            }

            var workflow = _repository.Query<Workflow>().FirstOrDefault(x => x.Project == order.Project && x.WorkflowCategory == workflowCategory && x.Status == WorkflowStatus.启用);

            if (workflow == null)
            {
                throw new DomainValidationException("操作失败，请先联系管理员添加审批流程");
            }
            return workflow.WorkflowSteps.FirstOrDefault(x => x.WorkflowStepCategory == WorkflowStepCategory.管家入库);
        }

        private void GenerateOrderAcceptance(PurchaseOrder order, Entities.User operatorUser, List<WorkflowProgress> wps)
        {
            var suppliers = order.OrderItems.Select(m => m.PurchaseProductSupplier.PurchaseSupplier).Distinct().ToList();

            if (suppliers.Any())
            {
                foreach (var item in suppliers)
                {
                    var acceptance = new PurchaseOrderAcceptance
                    {
                        PurchaseOrder = order,
                        PurchaseSupplier = item,
                        CreatedBy = operatorUser.Username,
                        CreatedOn = DateTime.Now,
                    };
                    acceptance.Status = order.OrderType == OrderType.食材采购单 ? AcceptanceStatus.已确认 : AcceptanceStatus.已验收;
                    acceptance.CheckRemarks = order.CheckRemarks;
                    acceptance.CheckTime = order.CheckTime;
                    acceptance.FileName = order.FileName;
                    acceptance.FilePath = order.FilePath;
                    acceptance.PrintFilePath = order.PrintOrderFilePath;
                    acceptance.AttachmentBytes = order.AttachmentBytes;
                    _repository.Create(acceptance);

                    foreach (var w in wps)
                    {
                        var wp = new WorkflowProgress
                        {
                            RalatedId = acceptance.Id,
                            Status = w.Status,
                            Workflow = w.Workflow,
                        };
                        _repository.Create(wp);

                        foreach (var result in w.WorkflowTrackingResults)
                        {
                            var wresult = new WorkflowTrackingResult
                            {
                                User = result.User,
                                Result = result.Result,
                                Description = result.Description,
                                WorkflowStep = result.WorkflowStep,
                                NextWorkflowStep = result.NextWorkflowStep,
                                WorkflowProgress = wp,
                                CreatedOn = result.CreatedOn,
                                CreatedBy = result.CreatedBy,
                            };
                            _repository.Create(wresult);
                        }
                    }
                }
            }

            var otherItems = order.OtherOrderItems.ToList();

            if (otherItems.Any())
            {
                var acceptance = new PurchaseOrderAcceptance
                {
                    PurchaseOrder = order,
                    PurchaseSupplier = null,
                    CreatedBy = operatorUser.Username,
                    CreatedOn = DateTime.Now,
                };
                acceptance.Status = order.OrderType == OrderType.食材采购单 ? AcceptanceStatus.已确认 : AcceptanceStatus.已验收;
                acceptance.CheckRemarks = order.CheckRemarks;
                acceptance.CheckTime = order.CheckTime;
                acceptance.FileName = order.FileName;
                acceptance.FilePath = order.FilePath;
                acceptance.PrintFilePath = order.PrintOrderFilePath;
                acceptance.AttachmentBytes = order.AttachmentBytes;
                _repository.Create(acceptance);

                foreach (var w in wps)
                {
                    var wp = new WorkflowProgress
                    {
                        RalatedId = acceptance.Id,
                        Status = w.Status,
                        Workflow = w.Workflow,
                    };
                    _repository.Create(wp);

                    foreach (var result in w.WorkflowTrackingResults)
                    {
                        var wresult = new WorkflowTrackingResult
                        {
                            User = result.User,
                            Result = result.Result,
                            Description = result.Description,
                            WorkflowStep = result.WorkflowStep,
                            NextWorkflowStep = result.NextWorkflowStep,
                            WorkflowProgress = wp,
                            CreatedOn = result.CreatedOn,
                            CreatedBy = result.CreatedBy,
                        };
                        _repository.Create(wresult);
                    }
                }
            }
        }
    }
}
