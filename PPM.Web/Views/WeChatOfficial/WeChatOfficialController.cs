using System.Web.Mvc;
using PensionInsurance.Entities;
using PensionInsurance.Query;
using PensionInsurance.Shared;

namespace PensionInsurance.Web.Views.WeChatOfficial
{
    public class WeChatOfficialController : Controller
    {

        private readonly IWeChatOfficialVisitService _weChatOfficialVisitService;
        private readonly IWeChatOfficialOpinionService _chatOfficialOpinionService;

        public WeChatOfficialController(IWeChatOfficialVisitService weChatOfficialVisitService, IWeChatOfficialOpinionService chatOfficialOpinionService)
        {
            _weChatOfficialVisitService = weChatOfficialVisitService;
            _chatOfficialOpinionService = chatOfficialOpinionService;
        }
        /// <summary>
        /// 预约访问
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Visit()
        {
            return View();
        }

        /// <summary>
        /// 一键导航
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Navigation()
        {
            return View();
        }

        /// <summary>
        /// 微官网
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult MicroOfficialWeb()
        {
            return View();
        }

        /// <summary>
        /// 在线服务
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult OnlineService()
        {
            return View();
        }


        /// <summary>
        /// 部门列表查询
        /// </summary>
        /// <param name="page">当前页</param>
        /// <param name="pageSize">分页数</param>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public ActionResult VisitList(int page = 1, int pageSize = Web.Common.PaginationSetttings.PageSize, WeChatOfficialVisitQuery query = null)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.预约访问, Permission.查看))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var viewModel = new IndexViewModel
            {
                Query = query,
                WeChatOfficialVisits = _weChatOfficialVisitService.Query(page, pageSize, query),

            };
            return View("~/Views/WeChatOfficial/VisitList.cshtml", viewModel);
        }

        /// <summary>
        /// 部门列表查询
        /// </summary>
        /// <param name="page">当前页</param>
        /// <param name="pageSize">分页数</param>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public ActionResult OpinionList(int page = 1, int pageSize = Web.Common.PaginationSetttings.PageSize, WeChatOfficialOpinionQuery query = null)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.预约访问, Permission.查看))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var viewModel = new IndexViewModel
            {
                OpinionQuery = query,
                WeChatOfficialOpinions = _chatOfficialOpinionService.Query(page, pageSize, query),

            };
            return View("~/Views/WeChatOfficial/OpinionList.cshtml", viewModel);
        }

    }
}