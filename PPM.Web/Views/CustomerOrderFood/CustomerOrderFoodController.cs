using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Foundation.Data;
using Foundation.Messaging;
using Newtonsoft.Json;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Query;
using PensionInsurance.Shared;
using PensionInsurance.Web.Common;
using PensionInsurance.Web.Views.Account;

namespace PensionInsurance.Web.Views.CustomerOrderFood
{
    public class CustomerOrderFoodController : AuthorizedController
    {
        private readonly ICommandService _commandService;
        private readonly IServicePackCatalogQueryService _serviceProjectQueryService;
        private readonly ICustomerAccountQueryService _customerAccountQueryService;
        private readonly IConsultingOrderFoodService _consultingOrderFoodService;
        private readonly IProjectQueryService _projectQueryService;
        private readonly IFetcher _fetcher;

        public CustomerOrderFoodController(ICommandService commandService, IServicePackCatalogQueryService serviceProjectQueryService, ICustomerAccountQueryService customerAccountQueryService, IConsultingOrderFoodService consultingOrderFoodService, IProjectQueryService projectQueryService, IFetcher fetcher)
        {
            _commandService = commandService;
            _serviceProjectQueryService = serviceProjectQueryService;
            _customerAccountQueryService = customerAccountQueryService;
            _consultingOrderFoodService = consultingOrderFoodService;
            _projectQueryService = projectQueryService;
            _fetcher = fetcher;
        }

        /// <summary>
        /// 现结餐饮账单列表
        /// </summary>
        /// <param name="page">当前页</param>
        /// <param name="pageSize">分页数</param>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public ActionResult Index(int page = 1, int pageSize = Web.Common.PaginationSetttings.PageSize, ConsultingOrderFoodQuery query = null)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.现结餐费账单管理, Permission.查看))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var viewModel = new IndexViewModel(Url, _fetcher)
            {
                Query = query,
                CashPaidOrderFoods = _consultingOrderFoodService.Query(page, pageSize, query),
                Projects = _projectQueryService.QueryAllValidByProjectFilter().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            return View("~/Views/CustomerOrderFood/Index.cshtml", viewModel);
        }

        public PartialViewResult GetPay(int id,int paymentType)
        {
            var consultingOrderFood = _consultingOrderFoodService.Get(id);
            
            PayOrderFoodViewModel ret = new PayOrderFoodViewModel
            {
                Amount = consultingOrderFood.ServiceAmount,
                CustomerName = consultingOrderFood.User.Username,
                ConsultingOrderFoodId = consultingOrderFood.Id,
                UserId = consultingOrderFood.User.Id,
                ProjectName = consultingOrderFood.Project==null?string.Empty: consultingOrderFood.Project.Name,
                PaidMoney =consultingOrderFood.PaidMoney,
                UnPaidMoney = consultingOrderFood.Total-consultingOrderFood.PaidMoney,
                PayMoney = 0,
                PaymentType = (PaymentType)paymentType,
                Remark = consultingOrderFood.Remark,
                ServicePackCatalogName = consultingOrderFood.ProjectServicePackCatalog.ServicePackCatalog.Name,
                ServicePackCatalogId = consultingOrderFood.ProjectServicePackCatalog.ServicePackCatalog.Id,
                UnitPrice = consultingOrderFood.UnitPrice,
                WaitConfirmMoney = consultingOrderFood.ConsutlingPayments.Where(m=> !m.Status).Sum(m=>m.PayMoney),
                Total = consultingOrderFood.Total,
            };
            return PartialView("_CustomerOrderFood.Pay", ret);
        }


        public ActionResult Create(int customerAccountId, ServiceRecordType serviceRecordType)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.客户点餐, Permission.新增))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var customerAccount = _customerAccountQueryService.Get(customerAccountId);
            var viewModel = new CreateViewModel
            {
                CustomerId = customerAccount.Customer.Id,
                CustomerAccountId = customerAccountId,
                ServiceAmount = 1,
                ProjectId = customerAccount.Project.Id,
                ProjectServicePackCatalogs =
                    _serviceProjectQueryService.QueryProjectServicePackCatalogs(new ServicePackCatalogQuery
                    {
                        ProjectId = customerAccount.Project.Id
                    }),
                ProjectServicePackCatalogsByTypes = GetProjectServicePackCatalogsByTypes(customerAccount.Project.Id),
                ServiceRecordType = serviceRecordType
            };
            return View("~/Views/CustomerOrderFood/Create.cshtml", viewModel);
        }

        /// <summary>
        /// 获取项目服务类型和种类，除去点餐服务
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        private Dictionary<string, List<ServiceSelectListItem>> GetProjectServicePackCatalogsByTypes(int projectId)
        {
            var servicePackCatalogsByTypeMap = new Dictionary<string, List<ServiceSelectListItem>>();
            var queriedData = _serviceProjectQueryService.QueryServicePackCatalogs(new ServicePackCatalogQuery
            {
                ProjectId = projectId
            });
            var types = queriedData.Where(m => m.Type == ServicePackCatalogType.点餐服务 || m.Type == ServicePackCatalogType.餐饮服务).Select(x => x.Type).Distinct();
            foreach (var type in types)
            {
                var servicePackCatalogsByType = queriedData.Where(x => x.Type == type)
                    .Select(x => new ServiceSelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString(),
                        Json = x.ToJavascript()
                    }).ToList();
                servicePackCatalogsByTypeMap.Add(type.ToString(), servicePackCatalogsByType);
            }
            return servicePackCatalogsByTypeMap;
        }


        [HttpPost]
        public ActionResult Create(CreateServiceRecordCommand command)
        {
            _commandService.Execute(command);
            return
                new RedirectResult(
                    Url.Action("Detail", "Customer", new { customerAccountId = command.CustomerAccountId }) +
                    "#tab_CustomerServiceRecord");
        }

        /// <summary>
        /// 删除房型
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public void Delete(DeleteEntityCommand command)
        {
            var cashPaidOrderFood = _consultingOrderFoodService.Get(command.EntityId);
            if (cashPaidOrderFood == null)
                throw new ApplicationException("cashPaidOrderFood cannot be found");
            if (cashPaidOrderFood.Status)
            {
                throw new ApplicationException("账单已经结清，无法删除");
            }
            if (cashPaidOrderFood.ConsutlingPayments.Any())
            {
                throw new ApplicationException("账单已经存在流水，无法删除账单");
            }

            _commandService.Execute(DeleteEntityCommand.Of(cashPaidOrderFood));
        }

        /// <summary>
        /// 撤销付款
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CancelPayment(CancelCustomerOrderFoodPaymentCommand command)
        {
            var cashPaidOrderFood = _consultingOrderFoodService.Get(command.CashPaidOrderFoodId);
            if (cashPaidOrderFood.Status)
            {
                throw new ApplicationException("账单已经结清，无法撤销");
            }

            _commandService.Execute(command);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Pay(CreateConsultingOrderFoodSerialCommand command)
        {
            var cashPaidOrderFood = _consultingOrderFoodService.Get(command.ConsultingOrderFoodId);
            if (cashPaidOrderFood.Status)
            {
                throw new ApplicationException("账单已经结清，无法再付款");
            }
            _commandService.Execute(command);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult CompleteBill(CompleteCustomerOrderFoodPaymentCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Index");
        }
    }
}