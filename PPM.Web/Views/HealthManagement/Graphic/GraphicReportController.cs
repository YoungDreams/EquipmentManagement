using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foundation.Core;
using Foundation.Data;
using PensionInsurance.Entities;
using PensionInsurance.Entities.CustomerHealthItem;
using PensionInsurance.Query;
using PensionInsurance.Web.Common;
using PensionInsurance.Shared;
using PensionInsurance.Web.Views.Account;
using PensionInsurance.Web.Views.HealthManagement.Graphic;

namespace PensionInsurance.Web.Views.HealthManagement.Graphic
{
    public class GraphicReportController : AuthorizedController
    {
        private readonly IFetcher _fetcher;
        private readonly ICustomerQueryService _customerQueryService;
        private readonly ICustomerLeaveQueryService _customerLeaveQueryService;
        private readonly IContractQueryService _contractQueryService;
        private readonly ICustomerHealthQueryService _customerHealthQueryService;
        private readonly ICustomerGraphicReportService _customerGraphicReportService;
        public GraphicReportController(IFetcher fetcher, ICustomerQueryService customerQueryService, ICustomerLeaveQueryService customerLeaveQueryService,
            IContractQueryService contractQueryService, ICustomerHealthQueryService customerHealthQueryService,
            ICustomerGraphicReportService customerGraphicReportService)
        {
            _fetcher = fetcher;
            _customerQueryService = customerQueryService;
            _customerLeaveQueryService = customerLeaveQueryService;
            _contractQueryService = contractQueryService;
            _customerHealthQueryService = customerHealthQueryService;
            _customerGraphicReportService = customerGraphicReportService;
        }

        public ActionResult Index(int customerAccountId, int healthStatus, int? EcgId = null, int? GmdId = null)
        {
            var checkStatus = healthStatus == 0 ? -1 : 4;
            var customer = _customerQueryService.GetCustomerBasicInfo(customerAccountId, checkStatus);
            if (customer == null)
            {
                throw new ApplicationException("客户帐号不存在！");
            }

            var GraphicReportQueryModel = new CustomerGraphicReportQuery()
            {
                CustomerAccountId = customerAccountId,
                EcgId = EcgId,
                GmdId = GmdId
            };

            //心电图
            var ecg = _customerGraphicReportService.EcQueryDefault(GraphicReportQueryModel).FirstOrDefault();
            //骨密度
            var boneDensity = _customerGraphicReportService.BdQueryDefault(GraphicReportQueryModel).FirstOrDefault();

            GraphicViewModel viewModel = new GraphicViewModel(Url);
            if (ecg != null)
            {
                viewModel.ECheckDate = ecg.CheckDate.ToString("yyyy年MM月dd日 HH:mm");
                viewModel.ECreateDate = ecg.CreatedOn.ToString("yyyy年MM月dd日 HH:mm");
                viewModel.EImageData = (Convert.ToBase64String(ecg.EcgPng));
                viewModel.EComFrom = ecg.ComeFrom;
                viewModel.EId = ecg.Id;
            }

            if (boneDensity != null)
            {
                viewModel.BdCheckDate = boneDensity.CheckDate.ToString("yyyy年MM月dd日 HH:mm");
                viewModel.BdCreateDate = boneDensity.CreatedOn.ToString("yyyy年MM月dd日 HH:mm");
                viewModel.BdImageData = (Convert.ToBase64String(boneDensity.Bdpng));
                viewModel.BdComFrom = boneDensity.ComeFrom;
                viewModel.BId = boneDensity.Id;
            }

            viewModel.Status = _customerGraphicReportService.QueryContractStatus(GraphicReportQueryModel);
            viewModel.Ecgs = _customerGraphicReportService
                .QueryEcgOrBoneThan5<HealthManageECG>(GraphicReportQueryModel).ToList();
            viewModel.BoneDensitys = _customerGraphicReportService
                .QueryEcgOrBoneThan5<HealthManageBoneDensity>(GraphicReportQueryModel).ToList();


            var hmViewModel = new HealthMonitoringViewModel
            {
                HealthReportStatus = healthStatus,
                CheckStatus = checkStatus.ToString()
            };
            hmViewModel.CustomerInfo = customer;
            viewModel.hmviewModel = hmViewModel;

            viewModel.CustomerAccountId = customerAccountId;
            return View("~/Views/HealthManagement/Graphic/Index.cshtml", viewModel);
        }
    }
}