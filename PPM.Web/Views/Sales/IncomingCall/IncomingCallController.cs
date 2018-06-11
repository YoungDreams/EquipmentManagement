using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Query;
using PensionInsurance.Shared;
using PensionInsurance.Web.Common;
using PensionInsurance.Web.Views.Account;

namespace PensionInsurance.Web.Views.Sales.IncomingCall
{
    public class IncomingCallController: AuthorizedController
    {
        private readonly IIncomingCallQueryService _incomingCallQueryService;
        private readonly ICommandService _commandService;
        public IncomingCallController(IIncomingCallQueryService incomingCallQueryService, ICommandService commandService)
        {
            _incomingCallQueryService = incomingCallQueryService;
            _commandService = commandService;
        }
        public ActionResult Index(int page = 1, int pageSize = Web.Common.PaginationSetttings.PageSize,IncomingQuery query = null)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.电话管理, Permission.查看))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var viewModel = new IndexViewModel
            {
                Query = query,
                Items = _incomingCallQueryService.Query(page, pageSize, query)
            };

            return View("~/Views/Sales/IncomingCall/Index.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult Import(ImportIncomingCallCommand command)
        {
            if (Request.Files.Count > 0)
            {
                command.FileBytes = Request.Files[0].ReadBytes();
                command.FileName = Request.Files[0].FileName;
            }

            _commandService.Execute(command);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(DeleteIncomingCallCommand command)
        {
            _commandService.Execute(command);

            return RedirectToAction("Index");
        }
        public ActionResult Export(ExportIncomingCallCommand command)
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