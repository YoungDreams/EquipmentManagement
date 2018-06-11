using System;
using System.Linq;
using System.Web.Mvc;
using Foundation.Data;
using Foundation.Messaging;
using PensionInsurance.CommandHandlers.Hack;
using PensionInsurance.Commands;
using PensionInsurance.Commands.ImportCustomerAccount;
using PensionInsurance.Query;
using PensionInsurance.Web.Views.Account;

namespace PensionInsurance.Web.Views.Import
{
    public class ImportController : AuthorizedController
    {
        private readonly ICommandService _commandService;
        private readonly IRepository _repository;
        public ImportController(IRepository repository, IConsultingQueryService consultingQueryService, ICommandService commandService)
        {
            _repository = repository;
            _commandService = commandService;
        }

        public ActionResult Index()
        {
            var viewModel = new IndexViewModel
            {
                //NeedToTrackClients = _clientQueryService.GetNeedToTrackClients()
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult ImportUsers()
        {
            var file = HttpContext.Request.Files[0];
            var filePath = Server.MapPath($"~/upload/{Guid.NewGuid()}.tmp");
            file.SaveAs(filePath);
            var fileContent = System.IO.File.ReadAllBytes(filePath);

            var importUsersCommand = new ImportUsersCommand
            {
                FileName = file.FileName,
                Content = fileContent
            };

            _commandService.Execute(importUsersCommand);

            return View("Success");
        }

        [HttpPost]
        public ActionResult ImportTrackings()
        {
            var file = HttpContext.Request.Files[0];
            var filePath = Server.MapPath($"~/upload/{Guid.NewGuid()}.tmp");
            file.SaveAs(filePath);
            var fileContent = System.IO.File.ReadAllBytes(filePath);

            var importUsersCommand = new ImportConsultingTrackingsCommand
            {
                FileName = file.FileName,
                Content = fileContent
            };

            _commandService.ExecuteSingleCommand(importUsersCommand);

            return View("Success");
        }



        [HttpPost]
        public ActionResult ImportConsultings()
        {
            var file = HttpContext.Request.Files[0];
            var filePath = Server.MapPath($"~/upload/{Guid.NewGuid()}.tmp");
            file.SaveAs(filePath);
            var fileContent = System.IO.File.ReadAllBytes(filePath);

            var importUsersCommand = new ImportConsultingsCommand
            {
                FileName = file.FileName,
                Content = fileContent
            };

            _commandService.ExecuteSingleCommand(importUsersCommand);

            return View("Success");
        }

        public ActionResult CreateCustomerAccount()
        {
            _commandService.Execute(new CreateCustomerAccountCommand());
            
            return View("Success");
        }

        public ActionResult UpdateContractCustomerAccount()
        {
            _commandService.Execute(new UpdateCustomerAccountCommand());
            return View("Success");
        }

        public ActionResult UpdateContrctCostChangeCurrentCost()
        {
            _commandService.Execute(new UpdateContrctCostChangeCurrentCostCommand());
            return View("Success");
        }
        public ActionResult UpdateContrctActualIsCompartment()
        {
            _commandService.Execute(new UpdateContrctActualIsCompartmentCommand());
            return View("Success");
        }

        public ActionResult UpdateCostChangeDiscount()
        {
            _commandService.Execute(new UpdateContrctCostChangeDiscountCommand());
            return View("Success");
        }

        public ActionResult UpdateUserbacklogProject()
        {
            _commandService.Execute(new UpdateUserbacklogProjectCommand());
            return View("Success");
        }
    }
}