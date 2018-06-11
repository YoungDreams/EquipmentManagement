using System;
using System.Linq;
using System.Web.Mvc;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Query;
using PensionInsurance.Shared;
using PensionInsurance.Web.Views.Account;
using IndexViewModel = PensionInsurance.Web.Views.Finance.ConsultingPayment.IndexViewModel;

namespace PensionInsurance.Web.Views.Finance.ConsultingPayment
{
    public class CustomerOrderFoodSerialController : AuthorizedController
    {
        private readonly ICommandService _commandService;
        private readonly IProjectQueryService _projectQueryService;
        private readonly IConsutlingPaymentService _cashPaidOrderFoodSerialService;

        public CustomerOrderFoodSerialController(ICommandService commandService, IProjectQueryService projectQueryService, IConsutlingPaymentService cashPaidOrderFoodSerialService)
        {
            _commandService = commandService;
            _projectQueryService = projectQueryService;
            _cashPaidOrderFoodSerialService = cashPaidOrderFoodSerialService;
        }

        /// <summary>
        /// 现结餐饮账单列表
        /// </summary>
        /// <param name="page">当前页</param>
        /// <param name="pageSize">分页数</param>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public ActionResult Index(int page = 1, int pageSize = Web.Common.PaginationSetttings.PageSize, ConsutlingPaymentQuery query = null)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.现结餐费流水管理, Permission.查看))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var viewModel = new IndexViewModel(Url)
            {
                Query = query,
                CashPaidOrderFoodSerials = _cashPaidOrderFoodSerialService.Query(page, pageSize, query),
                Projects = _projectQueryService.QueryAllValidByProjectFilter().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            return View("~/Views/Finance/ConsultingPayment/Index.cshtml", viewModel);
        }

        /// <summary>
        /// 删除流水
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(DeleteEntityCommand command)
        {
            var cashPaidOrderFoodSerial = _cashPaidOrderFoodSerialService.Get(command.EntityId);
            if (cashPaidOrderFoodSerial == null)
                throw new ApplicationException("cashPaidOrderFoodSerial cannot be found");
            //if (cashPaidOrderFoodSerial.Status)
            //{
            //    throw new ApplicationException("无法删除已经确认的流水");
            //}
            _commandService.Execute(DeleteEntityCommand.Of(cashPaidOrderFoodSerial));

            return RedirectToAction("Index");
        }


        /// <summary>
        /// 确认付款
        /// </summary>
        /// <returns></returns>
        public JsonResult ConfirmPayment(int id)
        {
            var cashPaidOrderFoodSerial = _cashPaidOrderFoodSerialService.Get(id);
            var confirmPaymentViewModel = new ConfirmPaymentViewModel
            {
                CustomerName = cashPaidOrderFoodSerial.User.Username,
                PaymentType = cashPaidOrderFoodSerial.PaymentType.ToString(),
                PayMoney = cashPaidOrderFoodSerial.PayMoney,
                CashPaidOrderFoodSerialId = cashPaidOrderFoodSerial.Id
            };
            return Json(confirmPaymentViewModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 确认付款
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ConfirmPayment(ConfirmCustomerOrderFoodSerialPaymentCommand command)
        {
            var cashPaidOrderFoodSerial = _cashPaidOrderFoodSerialService.Get(command.CashPaidOrderFoodSerialId);
            if (cashPaidOrderFoodSerial == null)
                throw new ApplicationException("无法确认被删除的流水");
            _commandService.Execute(command);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public void ConfirmPayments(ConfirmCustomerOrderFoodSerialPaymentsCommand command)
        {
            command.Confirmor = Shared.WebAppContext.Current.User;
            _commandService.Execute(command);
        }

    }
}