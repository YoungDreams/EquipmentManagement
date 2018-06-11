using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foundation.Data;
using Foundation.Messaging;
using PensionInsurance.Calculator;
using PensionInsurance.Commands;
using PensionInsurance.Query;
using PensionInsurance.Web.Views.Account;

namespace PensionInsurance.Web.Views.CustomerPrescription
{
    public class CustomerPrescriptionController : AuthorizedController
    {
        private readonly ICommandService _commandService;
        private readonly IProjectQueryService _projectQueryService;
        private readonly ICustomerMedicalAdviceService _customerMedicalAdviceService;
        private readonly ICustomerPrescriptionService _customerPrescriptionService;

        public CustomerPrescriptionController(ICommandService commandService, IProjectQueryService projectQueryService, ICustomerMedicalAdviceService customerMedicalAdviceService, CustomerPrescriptionGenerate customerPrescriptionGenerate, ICustomerPrescriptionService customerPrescriptionService)
        {
            _commandService = commandService;
            _projectQueryService = projectQueryService;
            _customerMedicalAdviceService = customerMedicalAdviceService;
            _customerPrescriptionService = customerPrescriptionService;
        }

        public ActionResult Index(int page = 1, int pageSize = Web.Common.PaginationSetttings.PageSize, CustomerPrescriptionQuery query = null)
        {
            var viewModel = new IndexViewModel(Url)
            {
                Query = query,
                CustomerPrescriptions = _customerPrescriptionService.Query(page, pageSize, query),
                Projects = _projectQueryService.QueryAllValidByProjectFilter().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            return View("~/Views/CustomerPrescription/Index.cshtml", viewModel);
        }

        [HttpPost]
        public void Delete(DeleteEntityCommand command)
        {
            var customerPrescription = _customerPrescriptionService.Get(command.EntityId);
            if (customerPrescription == null)
                throw new ApplicationException("customerPrescription cannot be found");

            _commandService.Execute(DeleteEntityCommand.Of(customerPrescription));
        }

        [HttpPost]
        public void ConfirmDispensing(ConfirmDispensingCommand command)
        {
            _commandService.Execute(command);
        }

        [HttpPost]
        public void ConfirmPrescriptionOffer(ConfirmPrescriptionOfferCommand command)
        {
            _commandService.Execute(command);
        }
    }
}