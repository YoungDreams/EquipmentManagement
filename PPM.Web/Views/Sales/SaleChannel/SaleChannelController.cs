using System;
using System.Web.Mvc;
using Foundation.Data;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Query;
using System.Linq;
using PensionInsurance.Shared;
using PensionInsurance.Web.Views.Account;

namespace PensionInsurance.Web.Views.Sales.SaleChannel
{
    /// <summary>
    /// 渠道管理
    /// </summary>
    public class SaleChannelController : AuthorizedController
    {
        private readonly ICommandService _commandService;
        private readonly IFetcher _fetcher;
        private readonly ISaleChannelQueryService _saleChannelService;
        private readonly IProjectQueryService _projectQueryService;
        public SaleChannelController(ICommandService commandService, IFetcher fetcher, ISaleChannelQueryService saleChannelQueryService, IProjectQueryService projectQueryService)
        {
            _commandService = commandService;
            _fetcher = fetcher;
            _saleChannelService = saleChannelQueryService;
            _projectQueryService = projectQueryService;
        }
        /// <summary>
        /// 渠道查询列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public ActionResult Index(int page = 1, int pageSize = Web.Common.PaginationSetttings.PageSize, SaleChannelQuery query = null)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.渠道管理, Permission.查看))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var viewModel = new IndexViewModel(Url)
            {
                Query = query,
                Items = _saleChannelService.Query(page,pageSize,query)
            };

            return View("~/Views/Sales/SaleChannel/Index.cshtml", viewModel);
        }

        /// <summary>
        /// 启用渠道 修改状态为有效
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ValidSaleChannels(ValidSaleChannelsCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 失效操作 修改状态为失效
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InvalidSaleChannels(InvalidSaleChannelsCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 获取单条渠道信息
        /// </summary>
        /// <param name="id">渠道编号</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.渠道管理, Permission.编辑))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var saleChannel = _fetcher.Get<Entities.SaleChannel>(id);

            var viewModel = new EditViewModel(Url)
            {
                ProjectList = _projectQueryService.QueryAllValidByProjectFilter().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                SaleChannelContacts = _saleChannelService.GetSaleChannelContacts(id),
                SaleChannelTrackings = _saleChannelService.GetSaleChannelTrackings(id),
                SaleChannelId = saleChannel.Id,
                SaleChannelNo = saleChannel.SaleChannelNo,
                SaleChannelName = saleChannel.Name,
                SaleChannelType = saleChannel.Type,
                ProjectId = saleChannel.ProjectId,
                SaleChannelStatus = saleChannel.Status,
                Description = saleChannel.Description,
                UserId = saleChannel.UserId,
                HeaderText = "编辑",
                
            };
            return View("~/Views/Sales/SaleChannel/Edit.cshtml", viewModel);
        }
        /// <summary>
        /// 创建渠道
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.渠道管理, Permission.新增))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var viewModel = new CreateViewModel
            {
                ProjectList = _projectQueryService.QueryAllValidByProjectFilter().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            return View("~/Views/Sales/SaleChannel/Create.cshtml", viewModel);
        }
        /// <summary>
        /// 执行渠道新增保存信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(CreateSaleChannelCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Index");
        }
        /// <summary>
        /// 执行修改渠道信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(EditSaleChannelCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Index");
        }
        /// <summary>
        /// 添加渠道联系人
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateContact(CreateSaleChannelContactCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Edit",new {id = command.SaleChannelId });
        }
        /// <summary>
        /// 查询渠道联系人信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditContact(int id)
        {
            var saleChannelContact = _fetcher.Get<SaleChannelContact>(id);
            var viewModel = new EditContactViewModel
            {
                SaleChannelId = saleChannelContact.SaleChannel.Id,
                Phone = saleChannelContact.Phone,
                ContactId = saleChannelContact.Id,
                Email = saleChannelContact.Email,
                Phone2 = saleChannelContact.Phone2,
                ContactName = saleChannelContact.Name,
                Address = saleChannelContact.Address,
                PostCode = saleChannelContact.PostCode
            };
            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 修改渠道联系人信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditContact(EditSaleChannelContactCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Edit", new { id = command.SaleChannelId });
        }
        /// <summary>
        /// 删除渠道联系人信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteContact(DeleteSaleChannelContactCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Edit", new { id = command.SaleChannelId });
        }
        /// <summary>
        /// 执行新增渠道跟踪信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateTracking(CreateSaleChannelTrackingCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Edit", new { id = command.SaleChannelId });
        }
        /// <summary>
        /// 查询渠道跟踪信息
        /// </summary>
        /// <param name="id">渠道跟踪编号</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditTracking(int id)
        {
            var saleChannelTracking = _fetcher.Get<SaleChannelTracking>(id);
            var viewModel = new EditTrackingViewModel
            {
                TrackingId = saleChannelTracking.Id,
                TrackingType = saleChannelTracking.TrackingType,
                Description = saleChannelTracking.Description,
                Status = saleChannelTracking.Status,
                StartTime = saleChannelTracking.StartTime,
                EndTime = saleChannelTracking.EndTime
            };
            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 执行修改渠道跟踪信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditTracking(EditSaleChannelTrackingCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Edit", new { id = command.SaleChannelId });
        }
        /// <summary>
        /// 删除渠道跟踪信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteTracking(DeleteSaleChannelTrackingCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Edit", new { id = command.SaleChannelId });
        }
        /// <summary>
        /// 删除渠道信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(DeleteEntityCommand command)
        {
            Entities.SaleChannel saleChannel = _saleChannelService.Get(command.EntityId);
            if (saleChannel == null)
                throw new ApplicationException("SaleChannel cannot be found");

            _commandService.Execute(DeleteEntityCommand.Of(saleChannel));

            return RedirectToAction("Index");
        }
    }
}