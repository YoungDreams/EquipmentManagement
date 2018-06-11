using System.Web.Mvc;
using Foundation.Core;
using Foundation.Data;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Query;
using PensionInsurance.Web.Views.Account;

namespace PensionInsurance.Web.Views.SystemSetting.Bed
{
    public class BedController : AuthorizedController
    {
        private readonly ICommandService _commandService;
        private readonly IFetcher _fetcher;
        private readonly IBedQueryService _bedQueryService;
        public BedController(ICommandService commandService, IFetcher fetcher, IBedQueryService bedQueryService)
        {
            _commandService = commandService;
            _fetcher = fetcher;
            _bedQueryService = bedQueryService;
        }

        /// <summary>
        /// 查询页面
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public ActionResult Index(int page = 1, int pageSize = Web.Common.PaginationSetttings.PageSize, BedQuery query = null)
        {
            var viewModel = new IndexViewModel(Url)
            {
                Query = query,
                Beds = _bedQueryService.Query(page, pageSize, query)
            };
            return View("~/Views/SystemSetting/Bed/Index.cshtml", viewModel);
        }

        /// <summary>
        /// 跳转到新增页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create()
        {
            var viewModel = new CreateViewModel();
            return View("~/Views/SystemSetting/Bed/Create.cshtml", viewModel);
        }

        /// <summary>
        /// 新增处理
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(CreateBedCommand command, string returnUrl)
        {
            _commandService.Execute(command);
            return returnUrl.IsNullOrWhiteSpace() ? (ActionResult)RedirectToAction("Index") : Redirect(returnUrl);
        }

        /// <summary>
        /// 跳转到编辑页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var room = _fetcher.Get<Entities.Bed>(id);
            var viewModel = new EditViewModel
            {
                BedId = room.Id,
                BedNo = room.BedNo,
                Name = room.Name,
                IsEnabled = room.IsEnabled,
                Remark = room.Remark,
                HeaderText = "编辑",
            };
            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 编辑提交
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(EditBedCommand command, string returnUrl)
        {
            var room = _fetcher.Get<Entities.Bed>(command.BedId);
            //command.BedNo = room.BedNo;
           // command.Name = room.Name;
            _commandService.Execute(command);
            return returnUrl.IsNullOrWhiteSpace() ? (ActionResult)RedirectToAction("Index") : Redirect(returnUrl);
        }

        /// <summary>
        /// 删除处理
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(DeleteBedCommand command)
        {
            _commandService.Execute(command);
            return command.ReturnUrl.IsNullOrWhiteSpace() ? (ActionResult)RedirectToAction("Index") : Redirect(command.ReturnUrl);
        }

        /// <summary>
        /// 启用
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ValidBeds(ValidBedCommand command, string returnUrl)
        {
            _commandService.Execute(command);
            return returnUrl.IsNullOrWhiteSpace() ? (ActionResult)RedirectToAction("Index") : Redirect(returnUrl);
        }

        /// <summary>
        /// 失效
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InValidBeds(InvalidBedCommand command, string returnUrl)
        {
            _commandService.Execute(command);
            return returnUrl.IsNullOrWhiteSpace() ? (ActionResult)RedirectToAction("Index") : Redirect(returnUrl);
        }
    }
}