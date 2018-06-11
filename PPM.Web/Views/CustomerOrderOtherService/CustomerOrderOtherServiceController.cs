using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foundation.Data;
using Foundation.Messaging;
using PensionInsurance.Entities;
using PensionInsurance.Query;
using PensionInsurance.Shared;
using PensionInsurance.Web.Common;
using PensionInsurance.Web.Views.CustomerServiceRecord;

namespace PensionInsurance.Web.Views.CustomerOrderOtherService
{
    public class CustomerOrderOtherServiceController : Controller
    {
        private readonly ICommandService _commandService;
        private readonly IFetcher _fetcher;
        private readonly IServicePackCatalogQueryService _serviceProjectQueryService;
        private readonly IConsumptiveMaterialQuery _consumptiveMaterialQueryService;

        public CustomerOrderOtherServiceController(ICommandService commandService, IFetcher fetcher, IServicePackCatalogQueryService serviceProjectQueryService, IContractQueryService contractQueryService, IConsumptiveMaterialQuery consumptiveMaterialQueryService)
        {
            _fetcher = fetcher;
            _commandService = commandService;
            _serviceProjectQueryService = serviceProjectQueryService;
            _consumptiveMaterialQueryService = consumptiveMaterialQueryService;
        }


        public ActionResult Create(int customerAccountId)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.其他护理服务, Permission.新增))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var customerAccount = _fetcher.Get<CustomerAccount>(customerAccountId);
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
                ProjectServicePackCatalogsByTypes = GetProjectServicePackCatalogsByTypes(customerAccount.Project.Id)
            };
            return View("~/Views/CustomerServiceRecord/Create.cshtml", viewModel);
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
            var types = queriedData.Where(m => m.Type == ServicePackCatalogType.电话服务).Select(x => x.Type).Distinct();
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

    }
}