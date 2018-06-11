using System;
using System.Collections.Generic;
using System.Linq;
using PensionInsurance.Query;
using PensionInsurance.Web.Views.Account;
using System.Web.Mvc;
using Foundation.Data;
using Foundation.Messaging;
using PensionInsurance.CommandHandlers.Exceptions;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Entities.DetailViews;
using PensionInsurance.Shared;
using PensionInsurance.Workflows;

namespace PensionInsurance.Web.Views.Finance.Bill
{
    public class BillController : AuthorizedController
    {
        private readonly ICustomerBillQueryService _customerBillQueryService;
        private readonly ICommandService _commandService;
        private readonly IFetcher _fetcher;
        private readonly IProjectQueryService _projectQueryService;
        public BillController(ICustomerBillQueryService customerBillQueryService, ICommandService commandService, IFetcher fetcher, ICustomerBillCurrentCostQuery customerBillCurrentCostQuery, IProjectQueryService projectQueryService)
        {
            _customerBillQueryService = customerBillQueryService;
            _commandService = commandService;
            _fetcher = fetcher;
            _projectQueryService = projectQueryService;
        }

        public ActionResult Index(CustomerBillQuery query, int page = 1, int pageSize = Web.Common.PaginationSetttings.PageSize)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.月账单管理, Permission.查看))
            {
                return RedirectToAction("NoPermission", "Home");
            }
            
            var viewModel = new IndexViewModel(Url)
            {
                Query = query,
                Items = _customerBillQueryService.Query(page, pageSize, query),
                ProjectList = _projectQueryService.QueryAllValidByProjectFilter().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
            };
            return View("~/Views/Finance/Bill/Index.cshtml", viewModel);
        }

        public ActionResult ArrearageDetail(int customerAccountId, int? contractId)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.月账单管理, Permission.查看))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var viewModel = new ArrearageDetailViewModel
            {
                Customer = _fetcher.Get<Entities.Customer>(customerAccountId)
            };

            var customerBills =
                _fetcher.Query<CustomerBill>()
                    .Where(x => x.Contract.CustomerAccount.Id == customerAccountId && (x.Status == BillStatus.未结清 || x.Status == BillStatus.待确认));
            if (contractId.HasValue)
            {
                customerBills = customerBills.Where(x => x.Contract.Id == contractId);
            }
            viewModel.Items = customerBills.ToList();
            return View("~/Views/Finance/Bill/ArrearageDetail.cshtml", viewModel);
        }

        public ActionResult Detail(int id)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.月账单管理, Permission.查看) &&
                !WebAppContext.Current.User.HasPermission(ModuleType.客户信息管理, Permission.客户信息查看客户账单))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var bill = _fetcher.Get<CustomerBill>(id);
            var payBackBills = _fetcher.Query<CustomerBill>().Where(x => x.Contract == bill.Contract &&
                              (x.BillType == BillType.正式账单 || x.BillType == BillType.调整账单)).ToList();
            var viewModel = new DetailViewModel
            {
                Bill = bill,
                Items =
                    _fetcher.Query<CustomerBill>()
                        .Where(
                            x =>
                                x.Contract.Id == bill.Contract.Id && x.Status == BillStatus.未结清 &&
                                x.StartDate < bill.StartDate),
                ServiceRecordsItems =
                    _fetcher.Query<ServiceRecordDetail>(
                        "SELECT * FROM ServiceRecordDetail WHERE CustomerAccountId=@CustomerAccountId AND ServiceDate>=@StartDate AND ServiceDate<=@EndDate AND ServiceDate>=@ContractStartTime AND ServiceDate<=@ContractEndTime ORDER BY ServiceDate",
                        new
                        {
                            CustomerAccountId = bill.Contract.CustomerAccount.Id,
                            StartDate = bill.StartDate,
                            EndDate = bill.EndDate,
                            ContractStartTime = bill.Contract.StartTime,
                            ContractEndTime = bill.Contract.ActualEndTime
                        }),
                PayBackCostTotal =
                    _fetcher.Query<CustomerBill>()
                        .Where(x => x.Contract == bill.Contract)
                        .Select(s => s.PayBackCost)
                        .Sum()
            };
            viewModel.PayBackCostTotal = payBackBills.Select(s => s.PayBackCost).Sum();
            viewModel.PayBackMealsCostTotal = payBackBills.Select(s => s.PayBackMealsCost).Sum();
            viewModel.PayBackRoomCostTotal = payBackBills.Select(s => s.PayBackRoomCost).Sum();
            viewModel.PayBackPackageServiceCostTotal = payBackBills.Select(s => s.PayBackPackageServiceCost).Sum();
            viewModel.PayBackServiceCostTotal = payBackBills.Select(s => s.PayBackServiceCost).Sum();
            viewModel.PayBackRefundCostTotal = payBackBills.Select(s => (s.ShortTermRefundCost - s.RefundCost)).Sum();
            viewModel.PayBackRoomChangeCostTotal = payBackBills.Select(s => s.RoomChangeCost).Sum();
            viewModel.PayBackMealChangeCostTotal = payBackBills.Select(s => s.MealChangeCost).Sum();
            viewModel.PayBackLevaPackageServiceCostTotal =
                payBackBills.Select(s => (s.ShortTermRefundCostService - s.RefundCostService)).Sum();
            return View("~/Views/Finance/Bill/Detail.cshtml", viewModel);
        }

        [HttpPost]
        public void Recalculate(RecalculateCustomerBillCommand command)
        {
            _commandService.Execute(command);
        }

        [HttpPost]
        public void Confirm(ConfirmBillCommand command)
        {
            command.BillIds = command.BillIds.ToList();
            _commandService.Execute(command);
        }

        [HttpPost]
        public void CalculateCustomerBill(CalculateCustomerBillCommand command)
        {
            _commandService.Execute(command);
        }
        
        [HttpPost]
        public void Adjust(AdjustCustomerBillCommand command)
        {
            _commandService.Execute(command);
        }

        [HttpPost]
        public void AdjustDescription(AdjustDescriptionCustomerBillCommand command)
        {
            _commandService.Execute(command);
        }

        [HttpPost]
        public ActionResult Relief(DiscountBillCommand command)
        {
            try
            {
                _commandService.Execute(command);
            }
            catch (ReliefNotEnoughException exc)
            {
                return Json(new
                {
                    success = false,
                    errorCode = 1,
                    errors = new string[] { exc.Message }
                });
            }
            return Json(new { success = true });
        }

        /// <summary>
        /// 账单分解
        /// </summary>
        /// <param name="billId"></param>
        /// <returns></returns>
        public ActionResult BreakUpBill(int billId)
        {
            var bill = _fetcher.Get<CustomerBill>(billId);
            var payBackBills = _fetcher.Query<CustomerBill>().Where(x => x.Contract == bill.Contract &&
                              (x.BillType == BillType.正式账单 || x.BillType == BillType.调整账单)).ToList();
            var viewModel = new DetailViewModel
            {
                Bill = bill,
                PayBackCostTotal = payBackBills.Select(s => s.PayBackCost).Sum(),
                PayBackMealsCostTotal = payBackBills.Select(s => s.PayBackMealsCost).Sum(),
                PayBackRoomCostTotal = payBackBills.Select(s => s.PayBackRoomCost).Sum(),
                PayBackPackageServiceCostTotal = payBackBills.Select(s => s.PayBackPackageServiceCost).Sum(),
                PayBackServiceCostTotal = payBackBills.Select(s => s.PayBackServiceCost).Sum(),
                PayBackRefundCostTotal = payBackBills.Select(s => (s.ShortTermRefundCost - s.RefundCost)).Sum(),
                // PayBackChangeCostTotal = -(payBackBills.Select(s => s.ChangeCost).Sum()),
                PayBackRoomChangeCostTotal = payBackBills.Select(s => s.RoomChangeCost).Sum(),
                PayBackMealChangeCostTotal = payBackBills.Select(s => s.MealChangeCost).Sum(),
            };
            viewModel.PayBackLevaPackageServiceCostTotal =
                payBackBills.Select(s => (s.ShortTermRefundCostService - s.RefundCostService)).Sum();
            return View("~/Views/Finance/Bill/BreakUpBill.cshtml", viewModel);
        }

        public ActionResult Export(ExportCustomerBillCommand command)
        {
            var result = _commandService.ExecuteFoResult(command);

            return Json(new
            {
                success = result.IsSucceed,
                redirect = result.UrlPath
            }, JsonRequestBehavior.AllowGet);
        }
    }
}