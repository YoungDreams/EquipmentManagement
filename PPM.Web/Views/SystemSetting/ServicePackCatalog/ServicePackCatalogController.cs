using System;
using System.Web.Mvc;
using Foundation.Data;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Query;
using PensionInsurance.Web.Views.Account;


namespace PensionInsurance.Web.Views.SystemSetting.ServicePackCatalog
{
    public class ServicePackCatalogController : AuthorizedController
    {
        private readonly ICommandService _commandService;
        private readonly IFetcher _fetcher;
        private readonly IServicePackCatalogQueryService _serviceProjectQueryService;

        public ServicePackCatalogController(ICommandService commandService, IFetcher fetcher, IServicePackCatalogQueryService serviceProjectQueryService)
        {
            _commandService = commandService;
            _fetcher = fetcher;
            _serviceProjectQueryService = serviceProjectQueryService;
        }

        /// <summary>
        /// 服务包查询页面
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public ActionResult Index(int page = 1, int pageSize = Web.Common.PaginationSetttings.PageSize, ServicePackCatalogQuery query = null)
        {
            var viewModel = new IndexViewModel(Url)
            {
                Query = query,
                ServicePackCatalog = _serviceProjectQueryService.Query(page, pageSize, query),
            };
            return View("~/Views/SystemSetting/ServicePackCatalog/Index.cshtml", viewModel);
        }

        /// <summary>
        /// 跳转到新增页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create()
        {
            var viewModel = new CreateViewModel();
            return View("~/Views/SystemSetting/ServicePackCatalog/Create.cshtml", viewModel);
        }

        /// <summary>
        /// 服务包新增处理
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(CreateServicePackCatalogCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 跳转到编辑页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var serviceProjectInfo = _fetcher.Get<Entities.ServicePackCatalog>(id);

            var viewModel = new EditViewModel
            {
                ServicePackCatalogId = serviceProjectInfo.Id,
                ServicePackCatalogNo = serviceProjectInfo.ServicePackCatalogNo,
                Name = serviceProjectInfo.Name,
                Type = serviceProjectInfo.Type,
                UnitPrice = serviceProjectInfo.UnitPrice,
                UnitPriceRemark = serviceProjectInfo.UnitPriceRemark,
                Remark = serviceProjectInfo.Remark,
                HeaderText = "编辑",
            };
            return View("~/Views/SystemSetting/ServicePackCatalog/Edit.cshtml", viewModel);
        }

        /// <summary>
        /// 编辑页面提交
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(EditServicePackCatalogCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 删除服务包
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(DeleteEntityCommand command)
        {
            Entities.ServicePackCatalog servicePackageProject = _serviceProjectQueryService.Get(command.EntityId);
            if (servicePackageProject == null)
                throw new ApplicationException("ServicePackCatalog cannot be found");

            _commandService.Execute(DeleteEntityCommand.Of(servicePackageProject));

            return RedirectToAction("Index");
        }
    }
}