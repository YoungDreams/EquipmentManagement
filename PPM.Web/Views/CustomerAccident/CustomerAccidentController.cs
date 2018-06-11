using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foundation.Core;
using Foundation.Data;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Query;
using PensionInsurance.Web.Views.Account;

namespace PensionInsurance.Web.Views.CustomerAccident
{
    public class CustomerAccidentController : AuthorizedController
    {
        private readonly IFetcher _fetcher;
        private readonly IContractQueryService _contractQueryService;
        private readonly ISettingQueryService _settingQueryService;
        private readonly ICommandService _commandService;
        public CustomerAccidentController(IFetcher fetcher, IContractQueryService contractQueryService, ISettingQueryService settingQueryService, ICommandService commandService)
        {
            _fetcher = fetcher;
            _contractQueryService = contractQueryService;
            _settingQueryService = settingQueryService;
            _commandService = commandService;
        }

        [HttpGet]
        public ActionResult Create(int customerAccountId)
        {
            var customerAccount = _fetcher.Get<Entities.CustomerAccount>(customerAccountId);

            var contracts = _contractQueryService.GetContracts(customerAccountId).ToList();

            var activatedContract = contracts.FirstOrDefault(x => x.ContractStatus == ContractStatus.生效);

            var contractServicePackChange =
                    _fetcher.Query<Entities.ContractServicePackChange>()
                        .FirstOrDefault(x => x.Status == ContractAddtionalStatus.生效 && x.ChangeDate <= DateTime.Now && x.ChangeEndDate >= DateTime.Now && x.Contract.Id == activatedContract.ContractId);

            var contractRoomChanges = _fetcher
                    .Query<Entities.ContractRoomChange>()
                        .FirstOrDefault(x => x.Status == ContractAddtionalStatus.生效 && x.ChangeDate <= DateTime.Now && x.ChangeEndDate >= DateTime.Now && x.Contract.Id == activatedContract.ContractId);

            var viewModel = new CreateViewModel
            {
                CustomerAccountId = customerAccountId,
                CustomerAccount = customerAccount,
                CustomerName = customerAccount.Customer.Name,
                CustomerAge = customerAccount.Customer.Birthday.ToAge(),
                CustomerSex = customerAccount.Customer.Sex,
                ProjectName = customerAccount.Project.Name,
                NervousDiseaseList = _settingQueryService.GetSettingsByType(SettingType.神经系统疾病).Select(x => new SelectListItem { Text = x.Name, Value = x.Name }),
                CardiovascularDiseaseList = _settingQueryService.GetSettingsByType(SettingType.心血管系统疾病).Select(x => new SelectListItem { Text = x.Name, Value = x.Name }),
                EndocrineDiseaseList = _settingQueryService.GetSettingsByType(SettingType.内分泌系统疾病).Select(x => new SelectListItem { Text = x.Name, Value = x.Name }),
                RespiratoryDiseaseList = _settingQueryService.GetSettingsByType(SettingType.呼吸系统疾病).Select(x => new SelectListItem { Text = x.Name, Value = x.Name }),
                UrinaryDiseaseList = _settingQueryService.GetSettingsByType(SettingType.泌尿系统疾病).Select(x => new SelectListItem { Text = x.Name, Value = x.Name }),
                DigestiveDiseaseList = _settingQueryService.GetSettingsByType(SettingType.消化系统疾病).Select(x => new SelectListItem { Text = x.Name, Value = x.Name }),
                SensoryDiseaseList = _settingQueryService.GetSettingsByType(SettingType.感官系统疾病).Select(x => new SelectListItem { Text = x.Name, Value = x.Name }),
                MovementDiseaseList = _settingQueryService.GetSettingsByType(SettingType.运动系统疾病).Select(x => new SelectListItem { Text = x.Name, Value = x.Name }),
            };

            if (contractServicePackChange != null)
                viewModel.Nursinglevel = contractServicePackChange.ConcernType;
            if (contractRoomChanges != null)
            {
                viewModel.Room = contractRoomChanges.NewRoom;
                viewModel.Bed = contractRoomChanges.NewBed;
                viewModel.BedId = viewModel.Bed.Id;
                viewModel.RoomId = viewModel.Room.Id;
            }
            return View("~/views/CustomerAccident/Create.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult Create(CreateCustomerAccidentCommand command)
        {
            var result = _commandService.ExecuteFoResult(command);

            return RedirectToAction("Edit", new { id = result.CustomerAccidentId });
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var accident = _fetcher.Get<Entities.CustomerAccident>(id);

            var viewModel = new EditViewModel
            {
                Id = accident.Id,
                CustomerName = accident.CustomerAccount.Customer.Name,
                CustomerSex = accident.CustomerAccount.Customer.Sex,
                CustomerAge = accident.CustomerAccount.Customer.Birthday.ToAge(),
                ProjectName = accident.CustomerAccount.Project.Name,
                Nursinglevel = accident.Nursinglevel,
                AccidentTime = accident.AccidentTime,
                AccidentScene = accident.AccidentScene,
                AccidentType = accident.AccidentType,
                OtherDiseases = accident.OtherDiseases,
                OtherAccidentType = accident.OtherAccidentType,
                OtherDiagnosisResults = accident.OtherDiagnosisResults,
                OtherAccidentInjury = accident.OtherAccidentInjury,
                OtherAccidentlocation = accident.OtherAccidentlocation,
                Accidentlocation = accident.Accidentlocation,
                AccidentInjury = accident.AccidentInjury,
                AccidentReason = accident.AccidentReason,
                IsSeeingTheDoctor=accident.IsSeeingTheDoctor,
                DiagnosisResults=accident.DiagnosisResults,
                Description=accident.Description,
                ContactFamilyResults=accident.ContactFamilyResults,
                
                Room = accident.Room,
                Bed = accident.Bed,
                NervousDiseaseList = _settingQueryService.GetSettingsByType(SettingType.神经系统疾病).Select(x => new SelectListItem { Text = x.Name, Value = x.Name }),
                CardiovascularDiseaseList = _settingQueryService.GetSettingsByType(SettingType.心血管系统疾病).Select(x => new SelectListItem { Text = x.Name, Value = x.Name }),
                EndocrineDiseaseList = _settingQueryService.GetSettingsByType(SettingType.内分泌系统疾病).Select(x => new SelectListItem { Text = x.Name, Value = x.Name }),
                RespiratoryDiseaseList = _settingQueryService.GetSettingsByType(SettingType.呼吸系统疾病).Select(x => new SelectListItem { Text = x.Name, Value = x.Name }),
                UrinaryDiseaseList = _settingQueryService.GetSettingsByType(SettingType.泌尿系统疾病).Select(x => new SelectListItem { Text = x.Name, Value = x.Name }),
                DigestiveDiseaseList = _settingQueryService.GetSettingsByType(SettingType.消化系统疾病).Select(x => new SelectListItem { Text = x.Name, Value = x.Name }),
                SensoryDiseaseList = _settingQueryService.GetSettingsByType(SettingType.感官系统疾病).Select(x => new SelectListItem { Text = x.Name, Value = x.Name }),
                MovementDiseaseList = _settingQueryService.GetSettingsByType(SettingType.运动系统疾病).Select(x => new SelectListItem { Text = x.Name, Value = x.Name }),
            };

            viewModel.NervousDiseases = accident.NervousDiseases.SplitToList<string>(',').Select(x => (string)x).ToList();
            viewModel.CardiovascularDiseases = accident.CardiovascularDiseases.SplitToList<string>(',').Select(x => (string)x).ToList();
            viewModel.EndocrineDiseases = accident.EndocrineDiseases.SplitToList<string>(',').Select(x => (string)x).ToList();
            viewModel.RespiratoryDiseases = accident.RespiratoryDiseases.SplitToList<string>(',').Select(x => (string)x).ToList();
            viewModel.UrinaryDiseases = accident.UrinaryDiseases.SplitToList<string>(',').Select(x => (string)x).ToList();
            viewModel.DigestiveDiseases = accident.DigestiveDiseases.SplitToList<string>(',').Select(x => (string)x).ToList();
            viewModel.SensoryDiseases = accident.SensoryDiseases.SplitToList<string>(',').Select(x => (string)x).ToList();
            viewModel.MovementDiseases = accident.MovementDiseases.SplitToList<string>(',').Select(x => (string)x).ToList();
            
            return View("~/views/CustomerAccident/Edit.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult Edit(EditCustomerAccidentCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Edit", new { id = command.Id });
        }


    }
}