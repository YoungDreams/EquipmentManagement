using Foundation.Core;
using Foundation.Data;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Query;
using PensionInsurance.Web.Views.Account;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using PensionInsurance.Shared;
using PensionInsurance.Web.Views.Customer;

namespace PensionInsurance.Web.Views.Finance.CustomerAccount
{
    public class CustomerAccountController : AuthorizedController
    {

        private readonly ICommandService _commandService;
        private readonly IFetcher _fetcher;
        private readonly ICustomerAccountQueryService _customerAccountService;
        private readonly ICustomerPointQueryService _customerPointQueryService;
        private readonly ICustomerCashFlowQueryService _customerCashFlowQueryService;
        private readonly ICustomerPaymentQueryService _customerPaymentQueryService;
        private readonly IConsultingOrderFoodService _cashPaidOrderFoodService;
        private readonly IServiceRecordQueryService _serviceRecordQueryService;

        public CustomerAccountController(ICommandService commandService, IFetcher fetcher, ICustomerAccountQueryService customerAccountService, ICustomerPaymentQueryService customerPaymentQueryService, ICustomerPointQueryService customerPointQueryService, ICustomerCashFlowQueryService customerCashFlowQueryService, ICustomerPaymentQueryService customerPaymentQueryService1, IConsultingOrderFoodService cashPaidOrderFoodService, IServiceRecordQueryService serviceRecordQueryService)
        {
            _commandService = commandService;
            _fetcher = fetcher;
            _customerAccountService = customerAccountService;
            _customerPointQueryService = customerPointQueryService;
            _customerCashFlowQueryService = customerCashFlowQueryService;
            _customerPaymentQueryService = customerPaymentQueryService1;
            _cashPaidOrderFoodService = cashPaidOrderFoodService;
            _serviceRecordQueryService = serviceRecordQueryService;
        }

        public ActionResult Index(int page = 1, int pageSize = Web.Common.PaginationSetttings.PageSize, CustomerAccountQuery query = null)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.客户账户管理, Permission.查看))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var viewModel = new IndexViewModel(Url)
            {
                Query = query,
                CustomerAccountList = _customerAccountService.Query(page, pageSize, query)
            };
            return View("~/Views/Finance/CustomerAccount/Index.cshtml", viewModel);
        }

        public ActionResult CustomerPoints(int page = 1, int pageSize = Web.Common.PaginationSetttings.PageSize)
        {
            var customerPoints = _fetcher.Query<CustomerPoint>().Where(x => x.Status == CustomerPointStatus.待确认);
            if (!Shared.WebAppContext.Current.User.RoleTypeIs(RoleType.超级管理员))
            {
                customerPoints = customerPoints.Where(x => Shared.WebAppContext.Current.User.Projects.Contains(x.CustomerAccount.Project)).OrderBy(o => o.UseDate);
            }
            var viewModel = new CustomerPointsViewModel(Url)
            {
                CustomerPointList = _fetcher.QueryPaged(customerPoints, page, pageSize)
            };
            return View("~/Views/Finance/CustomerAccount/CustomerPoint.cshtml", viewModel);
        }

        public ActionResult CustomerPayments(int page = 1, int pageSize = Web.Common.PaginationSetttings.PageSize)
        {
            var customerPayments = _fetcher.Query<CustomerPayment>().Where(x => x.Status == CustomerPaymentStatus.待确认);
            if (!Shared.WebAppContext.Current.User.RoleTypeIs(RoleType.超级管理员))
            {
                customerPayments = customerPayments.Where(x => Shared.WebAppContext.Current.User.Projects.Contains(x.CustomerAccount.Project));
            }
            var viewModel = new CustomerPaymentsViewModel(Url)
            {
                CustomerPaymentList = _fetcher.QueryPaged(customerPayments.OrderBy(o => o.PaymentDate), page, pageSize)
            };
            return View("~/Views/Finance/CustomerAccount/CustomerPayment.cshtml", viewModel);
        }

        /// <summary>
        /// 跳转到查看页面
        /// </summary>
        /// <param name="customerAccountId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Detail(int customerAccountId)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.客户账户管理, Permission.查看))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var viewModel = new IndexViewModel(Url)
            {
                CustomerAccountId = customerAccountId,
                CustomerPaymentList =
                    _fetcher.Query<CustomerPayment>()
                        .Where(x => x.CustomerAccount.Id == customerAccountId)
                        .OrderByDescending(x => x.PaymentDate)
                        .ToList(),
                CustomerPointList =
                    _fetcher.Query<CustomerPoint>()
                        .Where(x => x.CustomerAccount.Id == customerAccountId)
                        .OrderByDescending(x => x.UseDate)
                        .ToList()
            };
            return View("~/Views/Finance/CustomerAccount/Detail.cshtml", viewModel);
        }

        /// <summary>
        /// 跳转到缴费编辑页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditCustomerPayment(int id)
        {
            var customerPayment = _fetcher.Get<CustomerPayment>(id);
            var viewModel = new EditCustomerPaymentViewModel
            {
                CustomerPaymentId = customerPayment.Id,
                PaymentDate = customerPayment.PaymentDate,
                PaymentType = customerPayment.PaymentType,
                Payment = customerPayment.Payment,
                CustomerAccountId = customerPayment.CustomerAccount.Id,
                CustomerName = customerPayment.CustomerAccount.Customer.Name,
                PaymentTypeDesc = customerPayment.PaymentType.ToString()
            };
            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SubmitCustomerPayment(SubmitCustomerPaymentCommand command, string returnUrl)
        {
            command.Financer = Shared.WebAppContext.Current.User;
            _commandService.Execute(command);
            return returnUrl.IsNullOrWhiteSpace() ? (ActionResult)RedirectToAction("Index") : Redirect(returnUrl);
        }

        [HttpPost]
        public void DeleteCustomerPayment(DeleteCustomerPaymentCommand command)
        {
            _commandService.Execute(command);
        }

        [HttpGet]
        public PartialViewResult CreatePoint(int customerAccountId)
        {
            var viewModel = new CreateCustomerPointViewModel()
            {
                CustomerAccountId = customerAccountId
            };
            return PartialView("~/Views/Finance/CustomerAccount/_CustomerPoint.CreateForm.cshtml", viewModel);
        }

        [HttpGet]
        public PartialViewResult EditCustomerPoint(int id)
        {
            var customerPoint = _fetcher.Get<CustomerPoint>(id);
            var viewModel = new EditCustomerPointViewModel
            {
                CustomerPointId = customerPoint.Id,
                UseDate = customerPoint.UseDate,
                UsePoint = customerPoint.UsePoint,
                PointType = customerPoint.PointType,
                Remark = customerPoint.Remark,
                PointCategory = customerPoint.PointCategory,
                CustomerAccountId = customerPoint.CustomerAccount.Id,
            };
            return PartialView("~/Views/Finance/CustomerAccount/_CustomerPoint.EditForm.cshtml", viewModel);
        }

        [HttpPost]
        public void CreatePoint(CreateCustomerPointCommand command)
        {
            command.AgentId = Shared.WebAppContext.Current.User.Id;
            command.AgentName = Shared.WebAppContext.Current.User.Username;
            command.Status = CustomerPointStatus.待确认;
            command.PointType = PointType.存入;
            command.PointCategory = PointCategory.充值;
            _commandService.Execute(command);
        }

        [HttpPost]
        public void EditCustomerPoint(EditCustomerPointCommand command)
        {
            _commandService.Execute(command);
        }

        /// <summary>
        /// 积分确认
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitCustomerPoint(SubmitCustomerPointCommand command)
        {
            _commandService.Execute(command);
            return command.ReturnUrl.IsNullOrWhiteSpace() ? (ActionResult)RedirectToAction("Index") : Redirect(command.ReturnUrl);
        }

        /// <summary>
        /// 缴费删除
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeletePoint(DeleteEntityCommand command)
        {
            CustomerPoint customerPoint = _customerPointQueryService.Get(command.EntityId);
            if (customerPoint == null)
                throw new ApplicationException("CustomerPoint cannot be found");

            _commandService.Execute(DeleteEntityCommand.Of(customerPoint));
            return command.ReturnUrl.IsNullOrWhiteSpace() ? (ActionResult)RedirectToAction("Index") : Redirect(command.ReturnUrl);
        }

        /// <summary>
        /// 客户现金流水管理
        /// </summary>
        /// <returns></returns>
        public ActionResult CustomerCashFlow(int page = 1, int pageSize = Web.Common.PaginationSetttings.PageSize, CustomerCashFlowQuery query = null)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.客户现金流水管理, Permission.查看))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var viewModel = new CustomerCashFlowViewModel(Url)
            {
                Items = _customerCashFlowQueryService.Query(page, pageSize, query),
                Query = query,
            };
            return View("~/Views/Finance/CustomerAccount/CustomerCashFlow.cshtml", viewModel);
        }

        [HttpPost]
        public void ConfirmPayments(ConfimPaymentsCommand command)
        {
            command.Financer = Shared.WebAppContext.Current.User;
            _commandService.Execute(command);
        }

        /// <summary>
        /// 导出数据到excel
        /// </summary>
        /// <returns></returns>
        public ActionResult Export(ExportCustomerPaymentCommand command)
        {
            var result = _commandService.ExecuteFoResult(command);

            return Json(new
            {
                success = result.IsSucceed,
                redirect = result.UrlPath
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult VerifyCustomerPayment(VerifyCustomerPaymentCommand command)
        {
            var result = _commandService.ExecuteFoResult(command);
            var customerOrderFoods = _serviceRecordQueryService.GetCustomerServiceRecordByCustomerAccountId(command.CustomerAccountId).Where(m => m.ServiceRecordType == ServiceRecordType.现结 &&m.Total != m.ActualPayment);

            CustomerPayViewModel viewModel = new CustomerPayViewModel
            {
                Bills = result.Bills,
                CustomerAccountId = result.CustomerAccountId,
                CustomerName = result.CustomerName,
                IsSucceed = result.IsSucceed,
                Message = result.Message,
                Money = result.Money,
                TotalPayableCost = result.TotalPayableCost,
                TotalPrePayableCost = result.TotalPrePayableCost,
                TotalRemainingCost = result.TotalRemainingCost,
                PaymentType = command.PaymentType,
                CustomerOrderFoods = customerOrderFoods.ToList(),
            };

            return PartialView("~/Views/Customer/_Customer.Pay.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult CustomerPay(CustomerPayCommand command)
        {
            command.AgentId = Shared.WebAppContext.Current.User.Id;
            var result = _commandService.ExecuteFoResult(command);
            return Json(new { result, success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetPaymentCaption(int id)
        {
            var caption = _customerPaymentQueryService.GetPaymentCaption(id);
            var result = string.Join("<br/>", caption.Select(x => $"{x.CustomerName} - {x.PaymentCaption}：{x.PaymentAmount}"));
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据关键字获取推荐人信息
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public ActionResult GetCustomers(string keyword)
        {
            var customers = _fetcher.Query<Entities.CustomerAccount>()
                .Where(x => x.Customer.Name.Contains(keyword.Trim()))
                .Take(5)
                .ToList();
            return Json(customers.Select(x => new
            {
                Name = x.Customer.Name + "-" + x.Customer.CustomerNo + "-" + x.Project.Name,
                CId = x.Id
            }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCustomerAccounts(string keyword, int projectId, int customerAccountId)
        {
            var customers = new List<Entities.CustomerAccount>();

            if (!keyword.IsNullOrWhiteSpace())
            {
                customers = _fetcher.Query<Entities.CustomerAccount>()
                    .Where(x => x.Customer.Name.Contains(keyword.Trim()) && x.Project.Id == projectId &&
                                x.Id != customerAccountId)
                    .Take(5)
                    .ToList();
            }
            
            return Json(customers.Select(x => new
            {
                Name = x.Customer.Name + "-" + x.Customer.CustomerNo + "-" + x.Project.Name,
                CId = x.Id
            }), JsonRequestBehavior.AllowGet);
        }
    }
}