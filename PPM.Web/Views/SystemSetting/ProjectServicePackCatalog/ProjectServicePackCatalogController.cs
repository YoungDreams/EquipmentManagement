using System.Web.Mvc;
using Foundation.Data;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Query;
using PensionInsurance.Web.Views.Account;

namespace PensionInsurance.Web.Views.SystemSetting.ProjectServicePackCatalog
{
    public class ProjectServicePackCatalogController : AuthorizedController
    {
        private readonly ICommandService _commandService;
        private readonly IFetcher _fetcher;
        private readonly IServicePackCatalogQueryService _serviceProjectQueryService;

        public ProjectServicePackCatalogController(ICommandService commandService, IFetcher fetcher, IServicePackCatalogQueryService serviceProjectQueryService)
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
                ProjectServicePackCatalogs = _serviceProjectQueryService.QueryItems(page, pageSize, query),
            };
            return View("~/Views/SystemSetting/ProjectServicePackCatalog/Index.cshtml", viewModel);
        }

        /// <summary>
        /// 跳转到编辑页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var projectServicePackCatalog = _fetcher.Get<Entities.ProjectServicePackCatalog>(id);

            var viewModel = new EditViewModel
            {
                ProjectId = projectServicePackCatalog.Project.Id,
                ProjectServicePackCatalogId = projectServicePackCatalog.Id,
                UnitPrice = projectServicePackCatalog.UnitPrice,
                Remark = projectServicePackCatalog.Remark,
                IsEnabled = projectServicePackCatalog.IsEnabled,
                HeaderText = "编辑",
            };
            return View("~/Views/SystemSetting/ProjectServicePackCatalog/Edit.cshtml", viewModel);
        }

        /// <summary>
        /// 编辑页面提交
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(EditProjectServicePackCatalogCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Index", new { ProjectId = command.ProjectId});
        }
    }
}