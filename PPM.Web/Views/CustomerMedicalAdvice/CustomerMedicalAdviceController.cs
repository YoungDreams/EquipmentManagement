using System;
using System.Linq;
using System.Web.Mvc;
using Foundation.Core;
using Foundation.Data;
using Foundation.Messaging;
using PensionInsurance.Calculator;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Query;
using PensionInsurance.Web.Views.Account;
using PensionInsurance.Web.Views.Customer;
using PensionInsurance.Workflows;
using IndexViewModel = PensionInsurance.Web.Views.SystemSetting.Employee.IndexViewModel;

namespace PensionInsurance.Web.Views.CustomerMedicalAdvice
{
    public class CustomerMedicalAdviceController : AuthorizedController
    {
        private readonly ICommandService _commandService;
        private readonly IProjectQueryService _projectQueryService;
        private readonly IFetcher _fetcher;
        private readonly ICustomerMedicalAdviceService _customerMedicalAdviceService;
        private readonly CustomerPrescriptionGenerate _customerPrescriptionGenerate;

        public CustomerMedicalAdviceController(ICommandService commandService, IFetcher fetcher, IProjectQueryService projectQueryService, CustomerPrescriptionGenerate customerPrescriptionGenerate, ICustomerMedicalAdviceService customerMedicalAdviceService)
        {
            _commandService = commandService;
            _fetcher = fetcher;
            _projectQueryService = projectQueryService;
            _customerPrescriptionGenerate = customerPrescriptionGenerate;
            _customerMedicalAdviceService = customerMedicalAdviceService;
        }
        public ActionResult Index(int page = 1, int pageSize = Web.Common.PaginationSetttings.PageSize, CustomerMedicalAdviceQuery query = null)
        {
            var viewModel = new IndexViewModel(Url)
            {
                Query = query,
                CustomerMedicalAdvices = _customerMedicalAdviceService.Query(page, pageSize, query),
                Projects = _projectQueryService.QueryAllValidByProjectFilter().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            return View("~/Views/CustomerMedicalAdvice/Index.cshtml", viewModel);
        }

        [HttpGet]
        public PartialViewResult Create(MedicalAdviceType medicalAdviceType,int customerAccountId)
        {
            var customerAccount = _fetcher.Get<Entities.CustomerAccount>(customerAccountId);
            var  viewModel = new CreateViewModel
            {
                CustomerAccountId = customerAccount.Id,
                MedicalAdviceType = medicalAdviceType,
                Customer = customerAccount.Customer,
                StartTime = DateTime.Now,
            };
           return PartialView("~/Views/CustomerMedicalAdvice/_MedicalAdvice.Create.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult Create(CreateCustomerMedicalAdviceCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Detail","Customer", new { customerAccountId = command.CustomerAccountId });
        }

        [HttpGet]
        public PartialViewResult Edit(int id)
        {
            var customerMedicalAdvice = _fetcher.Get<Entities.CustomerMedicalAdvice>(id);
            var viewModel = new EditViewModel
            {
                CustomerMedicalAdviceId = customerMedicalAdvice.Id,
                Dose = customerMedicalAdvice.Dose,
                Frequency = customerMedicalAdvice.Frequency,
                IsDrugs = customerMedicalAdvice.IsDrugs,
                MedicalAdviceContent = customerMedicalAdvice.MedicalAdviceContent,
                MedicalAdviceType = customerMedicalAdvice.MedicalAdviceType,
                MedicationTime = customerMedicalAdvice.MedicationTime,
                RouteOfAdministration = customerMedicalAdvice.RouteOfAdministration,
                StartDoctor = customerMedicalAdvice.StartDoctor,
                EndTime = customerMedicalAdvice.EndTime,
                StartTime = customerMedicalAdvice.StartTime,
                CustomerAccountId = customerMedicalAdvice.CustomerAccount.Id,
            };
            return PartialView("~/Views/CustomerMedicalAdvice/_MedicalAdvice.Edit.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult Edit(EditCustomerMedicalAdviceCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Detail", "Customer", new { customerAccountId = command.CustomerAccountId });
        }

        [HttpPost]
        public void NurseConfirmedMedicalAdvice(NurseConfirmedMedicalAdviceCommand command)
        {
            _commandService.Execute(command);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public void Delete(DeleteEntityCommand command)
        {
            var customerMedicalAdvice = _fetcher.Get<Entities.CustomerMedicalAdvice>(command.EntityId);
            if (customerMedicalAdvice == null)
                throw new ApplicationException("customerMedicalAdvice cannot be found");

            _commandService.Execute(DeleteEntityCommand.Of(customerMedicalAdvice));
        }

        /// <summary>
        /// 作废
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult Discard(int id)
        {
            var customerMedicalAdvice = _fetcher.Get<Entities.CustomerMedicalAdvice>(id);
            
            DiscardMedicalAdviceViewModel viewModel = new DiscardMedicalAdviceViewModel
            {
                MedicalAdviceContent=customerMedicalAdvice.MedicalAdviceContent,
                MedicalAdviceType =  customerMedicalAdvice.MedicalAdviceType.ToString(),
                StartDoctor = customerMedicalAdvice.StartDoctor.RealName,
                CustomerMedicalAdviceId = id
            };
            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 作废
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public void Discard(DiscardMedicalAdviceCommand command)
        {
            var customerMedicalAdvice = _fetcher.Get<Entities.CustomerMedicalAdvice>(command.CustomerMedicalAdviceId);
            if (customerMedicalAdvice == null)
                throw new ApplicationException("customerMedicalAdvice cannot be found");

            _commandService.Execute(command);
        }

        /// <summary>
        /// 作废确认
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public void DiscardConfirm(NurseConfirmedDiscardMedicalAdviceCommand command)
        {
            var customerMedicalAdvice = _fetcher.Get<Entities.CustomerMedicalAdvice>(command.CustomerMedicalAdviceId);
            if (customerMedicalAdvice == null)
                throw new ApplicationException("customerMedicalAdvice cannot be found");

            _commandService.Execute(command);
        }

        /// <summary>
        /// 生成用药单
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public void GenerateDrugsOrderMedicalAdvice(DeleteEntityCommand command)
        {
            var customerMedicalAdvice = _fetcher.Get<Entities.CustomerMedicalAdvice>(command.EntityId);
            if (customerMedicalAdvice == null)
                throw new ApplicationException("customerMedicalAdvice cannot be found");

            _customerPrescriptionGenerate.GenerateCustomerPrescription(customerMedicalAdvice);
        }
        
    }
}