using System.Linq;
using System.Web.Mvc;
using PensionInsurance.Query;
using PensionInsurance.Web.Views.Account;
using PensionInsurance.Commands;
using Foundation.Messaging;
using Newtonsoft.Json;
using PensionInsurance.Entities;
using System.Collections.Generic;
using Foundation.Data;
using PensionInsurance.Shared;
using PensionInsurance.Web.Common;

namespace PensionInsurance.Web.Views.CustomerServiceRecord
{
    public class CustomerServiceRecordController : AuthorizedController
    {
        private readonly ICommandService _commandService;
        private readonly IFetcher _fetcher;
        private readonly IServicePackCatalogQueryService _serviceProjectQueryService;
        private readonly IConsumptiveMaterialQuery _consumptiveMaterialQueryService;

        public CustomerServiceRecordController(ICommandService commandService, IFetcher fetcher,  IServicePackCatalogQueryService serviceProjectQueryService, IContractQueryService contractQueryService, IConsumptiveMaterialQuery consumptiveMaterialQueryService)
        {
            _fetcher = fetcher;
            _commandService = commandService;
            _serviceProjectQueryService = serviceProjectQueryService;
            _consumptiveMaterialQueryService = consumptiveMaterialQueryService;
        }

        public ActionResult Edit(int id ,int customerId)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.客户点单, Permission.编辑))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var serviceRecord = _fetcher.Get<ServiceRecord>(id);
            var projectServicePackCatalog = _fetcher.Get<ProjectServicePackCatalog>(serviceRecord.ProjectServicePackCatalogId);
            var viewModel = new EditViewModel
            {
                Id = serviceRecord.Id,
                ServiceDate = serviceRecord.ServiceDate,
                UnitPrice = serviceRecord.UnitPrice,
                UnitPriceRemark = serviceRecord.UnitPrice.ToString(),
                Total = serviceRecord.Total,
                Remark = serviceRecord.Remark,
                ProjectServicePackCatalogId =serviceRecord.ProjectServicePackCatalogId,
                CustomerAccountId = serviceRecord.CustomerAccount.Id,
                CustomerId = customerId,
                ServiceAmount = serviceRecord.ServiceAmount,
                ProjectId = serviceRecord.CustomerAccount.Project.Id,
                ProjectServicePackCatalogs =
                    _serviceProjectQueryService.QueryServicePackCatalogs(new ServicePackCatalogQuery
                    {
                        ProjectId = serviceRecord.CustomerAccount.Project.Id
                    })
            };
            viewModel.ServiceRemark = projectServicePackCatalog.ServicePackCatalog.Remark;
            viewModel.ConsumptiveMaterialServices =
                _fetcher.Query<ConsumptiveMaterialService>().Where(x => x.ServiceRecord == serviceRecord).ToList();
            return View("~/Views/CustomerServiceRecord/Edit.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult Edit(EditServiceRecordCommand command)
        {
            command.ConsumptiveMaterialServiceList = JsonConvert.DeserializeObject<Dictionary<string, string>>(command.ConsumptiveMaterialList);
            _commandService.Execute(command);
            return new RedirectResult(Url.Action("Detail", "Customer", new { customerId = command.CustomerId, customerAccountId = command.CustomerAccountId }) + "#tab_CustomerServiceRecord");
        }
        
        public ActionResult Create(int customerAccountId)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.客户点单, Permission.新增))
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
            var types = queriedData
                .Where(m => m.Type != ServicePackCatalogType.点餐服务 && m.Type != ServicePackCatalogType.餐饮服务 &&
                            m.Type != ServicePackCatalogType.电话服务).Select(x => x.Type).Distinct();
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
            command.ConsumptiveMaterialServiceList =
                JsonConvert.DeserializeObject<Dictionary<string, string>>(command.ConsumptiveMaterialList);
            _commandService.Execute(command);
            return
                new RedirectResult(
                    Url.Action("Detail", "Customer", new {customerAccountId = command.CustomerAccountId}) +
                    "#tab_CustomerServiceRecord");
        }
        
        [HttpPost]
        public void Delete(DeleteCustomerServiceRecordCommand command)
        {
            _commandService.Execute(command);
        }

        [HttpGet]
        public ActionResult GetConsumptiveMaterials(string key)
        {
            var consumptiveMaterials = _consumptiveMaterialQueryService.QueryAll(new ConsumptiveMaterialQuery { Key = key });
            return Json(consumptiveMaterials.Select(x => new
            {
                Id = x.Id,
                Name = x.Name,
                UnitPrice = x.UnitPrice,
            }), JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult GetMaterialServices(int id)
        {
            var materialItems = _fetcher.Query<ConsumptiveMaterialService>().Where(x => x.ServiceRecord.Id == id)
                .ToList();
            
            return Json(materialItems.Select(x => new
            {
                Name = x.ConsumptiveMaterial.Name,
                Amount = x.Amount,
                Price=x.Price
            }), JsonRequestBehavior.AllowGet);
        }
    }
}