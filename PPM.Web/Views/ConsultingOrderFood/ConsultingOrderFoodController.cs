using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Foundation.Core;
using Foundation.Data;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Entities.DetailViews;
using PensionInsurance.Query;
using PensionInsurance.Shared;
using PensionInsurance.Web.Common;
using PensionInsurance.Web.Views.Account;

namespace PensionInsurance.Web.Views.ConsultingOrderFood
{
    public class ConsultingOrderFoodController : AuthorizedController
    {
        private readonly ICommandService _commandService;
        private readonly IFetcher _fetcher;
        private readonly IServicePackCatalogQueryService _serviceProjectQueryService;
        private readonly IConsultingOrderFoodService _consultingOrderFoodService;
        private readonly IProjectQueryService _projectQueryService;
        private readonly IConsultingQueryService _consultingQueryService;

        public ConsultingOrderFoodController(ICommandService commandService, IServicePackCatalogQueryService serviceProjectQueryService, IConsultingOrderFoodService consultingOrderFoodService, IProjectQueryService projectQueryService, IFetcher fetcher, IConsultingQueryService consultingQueryService)
        {
            _commandService = commandService;
            _serviceProjectQueryService = serviceProjectQueryService;
            _consultingOrderFoodService = consultingOrderFoodService;
            _projectQueryService = projectQueryService;
            _fetcher = fetcher;
            _consultingQueryService = consultingQueryService;
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
            if (!WebAppContext.Current.User.HasPermission(ModuleType.客户餐饮管理, Permission.查看))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var viewModel = new IndexViewModel(Url,_fetcher)
            {
                Query = query,
                CashPaidOrderFoods = _consultingOrderFoodService.Query(page, pageSize, query),
                Projects = _projectQueryService.QueryAllValidByProjectFilter().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            return View("~/Views/ConsultingOrderFood/Index.cshtml", viewModel);
        }

        public ActionResult Create(OrderFoodSourceType sourceType)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.客户餐饮管理, Permission.新增))
            {
                return RedirectToAction("NoPermission", "Home");
            }
            var projectId = WebAppContext.Current.User.Projects.FirstOrDefault() == null
                ? 2
                : WebAppContext.Current.User.Projects.FirstOrDefault().Id;
            var viewModel = new CreateViewModel
            {
                ProjectId = projectId,
                ServiceAmount = 1,
                SourceType = sourceType,
                ProjectServicePackCatalogsByTypes = GetProjectServicePackCatalogsByTypes(projectId)
            };
            return View("~/Views/ConsultingOrderFood/Create.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult Select()
        {
            return PartialView("~/Views/ConsultingOrderFood/_OrderFood.Select.cshtml");
        }

        [HttpPost]
        public ActionResult Select(ConsultingOrderFoodSeclectCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Create", "ConsultingOrderFood" ,new { sourceType =command.SourceType});
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
        public ActionResult Create(CreateConsultingOrderFoodCommand command)
        {
            _commandService.Execute(command);
            return
                new RedirectResult(
                    Url.Action("Index", "ConsultingOrderFood"));
        }

        public ActionResult GetsourceItems(string keyword,int projectId, OrderFoodSourceType sourceType)
        {
            var project = _fetcher.Get<Entities.Project>(projectId);

            if (!keyword.IsNullOrWhiteSpace())
            {
                if (sourceType == OrderFoodSourceType.咨询接待)
                {
                    var query = new ConsultingQuery
                    {
                        ProjectId = projectId,
                        Keywords = keyword.Trim(),
                    };
                    var consultings = _consultingQueryService.QueryConsultingDetails(query)
                        .Take(5)
                        .ToList();
                    return Json(consultings.Select(x => new
                    {
                        Name = x.ConsultingName + "-" + x.ConsultingNo,
                        CId = x.ConsultingId
                    }), JsonRequestBehavior.AllowGet);
                }
                if (sourceType == OrderFoodSourceType.销售渠道)
                {
                    var consultings = _fetcher.Query<Entities.SaleChannel>()
                       .Where(x => x.Name.Contains(keyword.Trim()) && x.ProjectId==projectId)
                       .Take(5)
                       .ToList();
                    return Json(consultings.Select(x => new
                    {
                        Name = x.Name + "-" + x.SaleChannelNo,
                        CId = x.Id
                    }), JsonRequestBehavior.AllowGet);
                }
                if (sourceType == OrderFoodSourceType.老人亲友)
                {
                    var consultings = _fetcher.Query<Entities.CustomerAccount>()
                       .Where(x => x.Customer.Name.Contains(keyword.Trim()) && x.Project.Id == projectId)
                       .Take(5)
                       .ToList();
                    return Json(consultings.Select(x => new
                    {
                        Name = x.Customer.Name + "-" + x.Customer.Consulting.ConsultingNo,
                        CId = x.Id
                    }), JsonRequestBehavior.AllowGet);
                }
            }
            return null;
        }

    }
}