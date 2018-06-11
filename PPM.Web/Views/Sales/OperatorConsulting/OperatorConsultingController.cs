using Foundation.Data;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Query;
using PensionInsurance.Shared;
using PensionInsurance.Web.Views.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PensionInsurance.Web.Views.Sales.OperatorConsulting
{
    public class OperatorConsultingController : AuthorizedController
    {
        private readonly ICommandService _commandService;
        private readonly IFetcher _fetcher;
        private readonly IOperatorConsultingService _operatorConsultingService;
        
        private readonly IUserQueryService _userQueryService;
        private readonly IProjectQueryService _projectQueryService;
        public OperatorConsultingController(ICommandService commandService, IFetcher fetcher,
         IUserQueryService userQueryService,
            IProjectQueryService projectQueryService, IOperatorConsultingService operatorConsultingService)
        {
            _commandService = commandService;
            _fetcher = fetcher;
            _userQueryService = userQueryService;
            _projectQueryService = projectQueryService;
            _operatorConsultingService = operatorConsultingService;
        }

        public ActionResult Index(int page = 1, int pageSize = Web.Common.PaginationSetttings.PageSize, OperatorConsultingQuery query = null)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.座席接电管理, Permission.查看))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var viewModel = new IndexViewModel(Url)
            {
                Query = query,
                Items = _operatorConsultingService.Query(page, pageSize, query),
                Sales = _userQueryService.GetSaleAndSaleManagerUsers().ToList(),
                ProjectList = _projectQueryService.QueryAll().Where(x => x.Status == ProjectStatus.有效).Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).ToList(),
            };
            return View("~/Views/Sales/OperatorConsulting/Index.cshtml", viewModel);
        }

        public ActionResult Export(ExportOperatorConsultingCommand command)
        {
            var result = _commandService.ExecuteFoResult(command);

            return Json(new
            {
                success = result.IsSucceed,
                redirect = $"{Url.Content("~/Attachments/Report/")}{result.FileName}"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(OperatorConsultingCreateCommand command)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.座席接电管理, Permission.新增))
            {
                return RedirectToAction("NoPermission", "Home");
            }
            command.UserId = WebAppContext.Current.User.Id;
            command.CreatedBy = WebAppContext.Current.User.Username;

            //var result = _commandService.ExecuteFoResult(command);
            _commandService.Execute(command);
            return RedirectToAction("Index", "OperatorConsulting");
        }
        [HttpPost]
        public ActionResult CheckMoblie()
        {
            string Mobile = Request.Form["Mobile"];
            string Type = Request.Form["Type"];
            int type = 0;
            if (Type == "1")
            {
                type = 1;
            }
            var data = _operatorConsultingService.CheckMoblie(Mobile, type);
            return Json(data);

        }
    }
}