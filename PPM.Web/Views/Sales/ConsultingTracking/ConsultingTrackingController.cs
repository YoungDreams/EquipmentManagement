using System.Linq;
using System.Web.Mvc;
using Foundation.Messaging;
using PensionInsurance.CommandHandlers;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Query;
using PensionInsurance.Shared;
using PensionInsurance.Web.Views.Account;

namespace PensionInsurance.Web.Views.Sales.ConsultingTracking
{
    public class ConsultingTrackingController : AuthorizedController
    {
        private readonly ICommandService _commandService;
        private readonly IConsultingQueryService _consultingQueryService;
        private readonly IUserQueryService _userQueryService;
        private readonly IProjectQueryService _projectQueryService;

        public ConsultingTrackingController(ICommandService commandService, IConsultingQueryService consultingQueryService, IUserQueryService userQueryService, IConsultingQueryService consultingQueryService1, IProjectQueryService projectQueryService)
        {
            _commandService = commandService;
            _userQueryService = userQueryService;
            _consultingQueryService = consultingQueryService;
            _projectQueryService = projectQueryService;
        }
        /// <summary>
        /// 查询客户跟踪数据
        /// </summary>
        /// <param name="page">当前页</param>
        /// <param name="pageSize">分页数量</param>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public ActionResult Index(int page = 1, int pageSize = Web.Common.PaginationSetttings.PageSize, ConsultingTrackingQuery query = null)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.客户跟踪管理, Permission.查看))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var viewModel = new IndexViewModel(Url)
            {
                Sales = _userQueryService.GetTransmitUsers(WebAppContext.Current.User.Id).ToList(),

                ProjectList = _projectQueryService.QueryAllValid().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                Query = query,
                Items = _consultingQueryService.QueryConsultingTrackings(page, pageSize, query)
            };

            return View("~/Views/Sales/ConsultingTracking/Index.cshtml", viewModel);
        }

        /// <summary>
        /// 创建客户跟踪
        /// </summary>
        /// <param name="clientId">客户Id</param>
        /// <returns></returns>
        //public ActionResult Create(int? clientId)
        //{
        //    if (!WebAppContext.Current.User.HasPermission(ModuleType.客户跟踪管理, Permission.新增))
        //    {
        //        return RedirectToAction("NoPermission", "Home");
        //    }

        //    var viewModel = new CreateViewModel
        //    {

        //        SalesPersonList = _userQueryService.GetSalesUsers().Select(x => new SelectListItem { Text = x.RealName, Value = x.Id.ToString() }),
        //        CreatorName = WebAppContext.Current.User.RealName,
        //        HeaderText = "添加",
        //    };

        //    if (clientId.HasValue)
        //    {
        //        viewModel.ConsultingId = clientId.Value;
        //        viewModel.ClientName = _consultingQueryService.Get(clientId.Value).ConsultingName;
        //    }
        //    else
        //    {
        //        viewModel.ClientList = _consultingQueryService.GetConsultings().Select(x => new SelectListItem { Text = x.ConsultingName, Value = x.Id.ToString() });
        //    }

        //    return View("~/Views/Sales/ConsultingTracking/Create.cshtml", viewModel);
        //}

        /// <summary>
        /// 查询单条跟踪记录
        /// </summary>
        /// <param name="id">跟踪Id</param>
        /// <returns></returns>
        //public ActionResult Edit(int id)
        //{
        //    if (!WebAppContext.Current.User.HasPermission(ModuleType.客户跟踪管理, Permission.编辑))
        //    {
        //        return RedirectToAction("NoPermission", "Home");
        //    }

        //    var tracking = _consultingQueryService.GetConsultingTracking(id);

        //    var viewModel = new CreateViewModel
        //    {
        //        SalesPersonList = _userQueryService.GetSalesUsers().Select(x => new SelectListItem { Text = x.RealName, Value = x.Id.ToString() }),
        //        CreatorName = WebAppContext.Current.User.RealName,
        //        HeaderText = "编辑",
        //        ConsultingTrackingId = id,
        //        ClientName = _consultingQueryService.Get(tracking.Consulting.Id).ConsultingName,
        //        ConsultingId = tracking.Consulting.Id
        //    };

        //    tracking.PopulateEntity(viewModel);
        //    viewModel.Status = tracking.Status;
        //    return View("~/Views/Sales/ConsultingTracking/Create.cshtml", viewModel);
        //}

        /// <summary>
        /// 添加跟踪数据
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(CreateConsultingTrackingCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 编辑跟踪数据
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(EditConsultingTrackingCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 删除跟踪数据
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(DeleteConsultingTrackingCommand command)
        {
            _commandService.Execute(command);

            return RedirectToAction("Index");
        }

        public ActionResult Export(ExportConsultingTrackingCommand command)
        {
            var result = _commandService.ExecuteFoResult(command);

            return Json(new
            {
                success = result.IsSucceed,
                redirect = $"{Url.Content("~/Attachments/Report/")}{result.FileName}"
            }, JsonRequestBehavior.AllowGet);
        }
    }
}