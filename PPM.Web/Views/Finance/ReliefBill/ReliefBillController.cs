using System;
using System.Linq;
using System.Web.Mvc;
using Foundation.Data;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Query;
using PensionInsurance.Web.Common;
using PensionInsurance.Web.Views.Account;
using PensionInsurance.Workflows;

namespace PensionInsurance.Web.Views.Finance.ReliefBill
{
    public class ReliefBillController: AuthorizedController
    {
        private readonly ICommandService _commandService;
        private readonly IFetcher _fetcher;
        private readonly ReliefBillWorkflow _reliefBillWorkflow;
        // GET: Bill

        public ReliefBillController(ICommandService commandService, IFetcher fetcher,
            ICustomerBillCurrentCostQuery customerBillCurrentCostQuery,
            ReliefBillWorkflow reliefBillWorkflow)
        {
            _commandService = commandService;
            _fetcher = fetcher;
            _reliefBillWorkflow = reliefBillWorkflow;
        }

        [HttpGet]
        public ActionResult Detail(int id)
        {
            var bill = _fetcher.Get<CustomerBill>(id);
            var viewModel = new DetailViewModel()
            {
                BillId = id,
                CustomerName = bill.Contract.BName,
                PreCustomerBill = bill.PreviousCustomerBill,
                CustomerBill = bill,
            };
            var discountAmount =
                _fetcher.Query<CustomerAccountDiscountLog>()
                    .Where(
                        x => x.CustomerAccount.Project == bill.Contract.Project && x.DiscountTime.Year == DateTime.Now.Year)
                    .ToList();
            viewModel.CustomerCurrentYearDiscount =
                -discountAmount.Where(x => x.CustomerAccount == bill.Contract.CustomerAccount).Sum(s => s.DiscountAmount);
            viewModel.ProjectYearDiscount = -discountAmount.Sum(x => x.DiscountAmount);
            viewModel.CurrentWorkFlowStep = _reliefBillWorkflow.GetCurrentWorkflowStep(bill);
            if (viewModel.CurrentWorkFlowStep != null)
            {
                viewModel.CurrentWorkflowStepId = viewModel.CurrentWorkFlowStep.Id;
            }

            viewModel.TrackingResult = new TrackingResult.TrackingResultViewModel
            {
                WorkflowTrackingResults = _reliefBillWorkflow.GetWorkflowTrackingResults(bill),
                WorkflowHistoryTrackingResults = _reliefBillWorkflow.GetWorkflowHistoryTrackingResults(bill)
            };
            return View("~/Views/Finance/ReliefBill/Detail.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult SubmitAndCreate(SubmitAndCreateReliefBillCommand command)
        {
            if (Request.Files.Count > 0)
            {
                command.FileBytes = Request.Files[0].ReadBytes();
                command.FileName = Request.Files[0].FileName;
            }

            var result = _commandService.ExecuteFoResult(command);
            return RedirectToAction("Detail", new { id = result.ReliefBillId });
        }

        [HttpPost]
        public ActionResult Submit(SubmitReliefBillCommand command)
        {
            if (Request.Files.Count > 0)
            {
                command.FileBytes = Request.Files[0].ReadBytes();
                command.FileName = Request.Files[0].FileName;
            }

            var result = _commandService.ExecuteFoResult(command);
            return RedirectToAction("Detail", new { id = result.ReliefBillId });
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var bill = _fetcher.Get<CustomerBill>(id);
            var customerAccount = _fetcher.Get<Entities.CustomerAccount>(bill.Contract.CustomerAccount.Id);
            var hasCustomerBills =
                _fetcher.Query<CustomerBill>().Where(x => x.Contract.CustomerAccount.Id == customerAccount.Id && x.BillType == BillType.正式账单 && (x.Status == BillStatus.已结清) || x.Status == BillStatus.未结清).ToList();
            var viewModel = new EditViewModel(Url)
            {
                Id = bill.Id,
                CustomerAccount = customerAccount,
                CustomerAccountId = customerAccount.Id,
                CustomerBills = hasCustomerBills,
                BillId = bill.PreviousCustomerBill.Id,
                RoomCost = bill.RoomCost,
                MealsCost = bill.MealsCost,
                PackageServiceCost = bill.PackageServiceCost,
                IncrementCost = bill.IncrementCost,
                ActualPaymentCost = -bill.ActualPaymentCost,
                Description = bill.SpecialDiscountDescription,
                FilePath = bill.ReliefFilePath,
                IsIncluded = bill.IsIncluded,
            };
            //viewModel.MaxReliefCost = GetBillMaxRelieCost(bill.PreviousCustomerBill.Id)+(-bill.ActualPaymentCost);
            var discountAmount =
                _fetcher.Query<CustomerAccountDiscountLog>()
                    .Where(
                        x => x.CustomerAccount.Project == customerAccount.Project && x.DiscountTime.Year == DateTime.Now.Year)
                    .ToList();
            viewModel.CustomerCurrentYearDiscount =
                -discountAmount.Where(x => x.CustomerAccount == customerAccount).Sum(s => s.DiscountAmount);
            viewModel.ProjectYearDiscount = -discountAmount.Sum(x => x.DiscountAmount);
            return View("~/Views/Finance/ReliefBill/Edit.cshtml", viewModel);
        }

        public ActionResult Approval(ApprovalReliefBillCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Detail", new { id = command.ReliefBillId });
        }

        /// <summary>
        /// 账单减免作废
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DraftAndDelete(DraftAndDeleteReliefBillCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("IndexCheckIn", "Customer");
        }

        [HttpGet]
        public ActionResult Create(int customerAccountId)
        {
            var customerAccount = _fetcher.Get<Entities.CustomerAccount>(customerAccountId);
            var hasCustomerBills =
                _fetcher.Query<CustomerBill>().Where(x => x.Contract.CustomerAccount.Id == customerAccount.Id && x.BillType == BillType.正式账单 && (x.Status == BillStatus.未结清 || x.Status == BillStatus.已结清)).OrderByDescending(x => x.EndDate).ToList();
            var viewModel = new CreateViewModel
            {
                CustomerAccount = customerAccount,
                CustomerAccountId = customerAccountId,
                CustomerBills = hasCustomerBills
            };
            var discountAmount =
                _fetcher.Query<CustomerAccountDiscountLog>()
                    .Where(
                        x => x.CustomerAccount.Project == customerAccount.Project && x.DiscountTime.Year == DateTime.Now.Year)
                    .ToList();
            viewModel.CustomerCurrentYearDiscount =
                -discountAmount.Where(x => x.CustomerAccount == customerAccount).Sum(s => s.DiscountAmount);
            viewModel.ProjectYearDiscount = -discountAmount.Sum(x => x.DiscountAmount);

            return View("~/Views/Finance/ReliefBill/Create.cshtml", viewModel);
        }

        //[HttpGet]
        //public JsonResult GetCustomerBill(int customerAccountId)
        //{
        //    var customerAccount = _fetcher.Get<Entities.CustomerAccount>(customerAccountId);

        //    var hasCustomerBills =
        //        _fetcher.Query<CustomerBill>().Where(x => x.Contract.CustomerAccount.Id == customerAccount.Id && x.BillType == BillType.正式账单 && x.Status == BillStatus.未结清).ToList();

        //    if (!hasCustomerBills.Any())
        //    {
        //        return Json(new { success = false, message = "客户不存在未结清的账单" }, JsonRequestBehavior.AllowGet);
        //    }
        //    return Json(new { success = true, redirect = Url.Action("ReliefBill", "Bill", new { customerAccountId = customerAccount.Id }) }, JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult IsCheckOut(int billId ,decimal actualPaymentCost,decimal maxReliefCost)
        //{
        //    var oldBill = _fetcher.Get<CustomerBill>(billId);
        //    if (-actualPaymentCost > maxReliefCost)
        //    {
        //        if (oldBill.EndDate.Date == oldBill.Contract.ActualEndTime.Value.Date && oldBill.EndDate.Date < oldBill.Contract.EndTime.Value.Date)
        //        {
        //            return Json(new { Result = true}, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    return Json(new { Result = false}, JsonRequestBehavior.AllowGet);
        //}

        //public decimal GetBillMaxRelieCost(int billId)
        //{
        //    var bill = _fetcher.Get<CustomerBill>(billId);
        //    var penddingConfirmCustomerPaymentCost = 0.0m;
        //    var reliefBillCost = 0.0m;

        //    var customerPaymentBills = _fetcher.Query<CustomerPaymentBill>()
        //        .Where(x =>
        //               x.CustomerPayment.CustomerAccount.Id == bill.Contract.CustomerAccount.Id && x.CustomerBill.Id == bill.Id &&
        //               x.CustomerPayment.Status == CustomerPaymentStatus.待确认)
        //        .ToList();
        //    var reliefBills =
        //        _fetcher.Query<CustomerBill>()
        //            .Where(x => x.BillType == BillType.减免账单 && x.Status !=BillStatus.已结清 && x.PreviousCustomerBill.Id == bill.Id)
        //            .ToList();

        //    if (customerPaymentBills.Count > 0)
        //    {
        //        penddingConfirmCustomerPaymentCost = customerPaymentBills.Sum(x => x.PayableCost + x.RemainingCost);
        //    }
        //    if (reliefBills.Count > 0)
        //    {
        //        reliefBillCost = reliefBills.Sum(x => x.ActualPaymentCost);
        //    }

        //    return  bill.Arrears - penddingConfirmCustomerPaymentCost + reliefBillCost;
        //}

        //[HttpGet]
        //public JsonResult GetCustomerBillMaxReliefCost(int billId)
        //{
        //    var maxReliefCost = GetBillMaxRelieCost(billId);

        //    return Json(new { success = true, MaxReliefCost = maxReliefCost }, JsonRequestBehavior.AllowGet);
        //}
    }
}