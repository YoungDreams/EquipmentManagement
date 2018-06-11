using Foundation.Core;
using Foundation.Data;
using PensionInsurance.Entities;
using PensionInsurance.Entities.CustomerHealthItem;
using PensionInsurance.Query;
using PensionInsurance.Web.Common;
using PensionInsurance.Shared;
using PensionInsurance.Web.Views.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PensionInsurance.Commands;
using Foundation.Messaging;
using Newtonsoft.Json;
using System.Text;
using PensionInsurance.Entities.DetailViews;
using System.ComponentModel;
using System.Reflection;
using PensionInsurance.CommandHandlers.Extensions;

namespace PensionInsurance.Web.Views.HealthManagement
{
    public class HealthManagementController : AuthorizedController
    {
        private readonly ICommandService _commandService;
        private readonly IFetcher _fetcher;
        private readonly ICustomerQueryService _customerQueryService;
        private readonly ICustomerLeaveQueryService _customerLeaveQueryService;
        private readonly IContractQueryService _contractQueryService;
        private readonly ICustomerHealthQueryService _customerHealthQueryService;
        private readonly IHealthManageReportService _healthManageReportService;
        private readonly IStoreyQueryService _storeyQueryService;
        private const string TimePerMinutes = @"次/分钟";
        private const string Temperature = @"°C";
        private const string BloodPresure = @"mmHg";
        private const string Mol = @"mmol/L";
        private const string Cm = @"cm";
        private const string Kg = @"kg";

        public HealthManagementController(ICommandService commandService, IFetcher fetcher, ICustomerQueryService customerQueryService, ICustomerLeaveQueryService customerLeaveQueryService, 
            IContractQueryService contractQueryService, 
            ICustomerHealthQueryService customerHealthQueryService, 
            IHealthManageReportService healthManageReportService,
            IStoreyQueryService storeyQueryService)
        {
            _fetcher = fetcher;
            _commandService = commandService;
            _customerQueryService = customerQueryService;
            _customerLeaveQueryService = customerLeaveQueryService;
            _contractQueryService = contractQueryService;
            _customerHealthQueryService = customerHealthQueryService;
            _healthManageReportService = healthManageReportService;
            _storeyQueryService = storeyQueryService;
        }
        // GET: HealthManagement
        public ActionResult Index(CustomerHealthQuery query, int page = 1, int pageSize = Web.Common.PaginationSetttings.SmallPageSize)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.健康管理, Permission.查看))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var viewModel = new IndexViewModel(Url)
            {
                Query = query,
                Items = _customerHealthQueryService.Query(page, pageSize, query),
                EvaluateCount = _customerHealthQueryService.QueryEvaluateCount(query)
            };
            return View("~/Views/HealthManagement/Index.cshtml", viewModel);
        }

        [HttpPost]
        public JsonResult GetAllStoreyList()
        {
           return Json(_storeyQueryService.QueryAllStorey().ToList());
        }

        public ActionResult MoreCustomerHealthIndex(MoreCustomerHealthQuery query, int page = 1, int pageSize = Web.Common.PaginationSetttings.SmallPageSize)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.健康管理, Permission.查看更多长者信息))
            {
                return RedirectToAction("NoPermission", "Home");
            }
            var viewModel = new MoreCustomerHealthViewModel(Url)
            {
                Query = query,
                Items = _customerHealthQueryService.MoreHealthQuery(page, pageSize, query)
            };
            return View("~/Views/HealthManagement/MoreCustomerHealthIndex.cshtml", viewModel);
        }


        [HttpGet]
        public ActionResult GetRoomsByProjectId(int? projectId)
        {
            var rooms = _customerHealthQueryService.Query(new CustomerHealthQuery { ProjectId = projectId });
            return Json(rooms.Select(x => new
            {
                Value = x.RoomId,
                Text = x.BuildingName + x.UnitName + x.FloorName + x.RoomName
            }), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 健康检测首页
        /// </summary>
        /// <returns></returns>
        public ActionResult HealthMonitoringIndex(int customerAccountId,int healthStatus)
        {
            return View("~/Views/HealthManagement/HealthMonitoringIndex.cshtml", GetHealthMonitoringViewModel(customerAccountId, healthStatus));
        }
        /// <summary>
        /// 健康报告
        /// </summary>
        /// <param name="customerAccountId"></param>
        /// <returns></returns>
        public ActionResult HealthManageReport(int customerAccountId, int healthStatus,int editStatus=0)
        {
            var viewModel = GetCustomerHealthInfoViewModel(customerAccountId, healthStatus);
            var hmReportList= _healthManageReportService.Query(viewModel.CustomerInfo.CustomerId).AsEnumerable().OrderByDescending(m => m.HReportDate).Take(5).ToList();
            viewModel.EditStatus = editStatus;
            viewModel.HMReportList = hmReportList;
            return View("~/Views/HealthManagement/HealthManageReport.cshtml", viewModel);
        }
        /// <summary>
        /// 根据客户帐号获取客户基本信息
        /// </summary>
        /// <param name="customerAccountId"></param>
        /// <returns></returns>
        private HealthMonitoringViewModel GetHealthMonitoringViewModel(int customerAccountId, int healthStatus)
        {
            var viewModel = GetCustomerHealthInfoViewModel(customerAccountId, healthStatus);
            viewModel.HMVList = GetHealthManageInfoList(viewModel.CustomerInfo.CustomerId, viewModel.CustomerInfo.Sex);
            return viewModel;
        }
        /// <summary>
        /// 根据客户编号获取客户基本信息
        /// </summary>
        /// <param name="customerAccountId"></param>
        /// <returns></returns>
        private HealthMonitoringViewModel GetCustomerHealthInfoViewModel(int customerAccountId, int healthStatus)
        {
            var checkStatus = healthStatus == 0 ? -1 : 4;
            var customer = _customerQueryService.GetCustomerBasicInfo(customerAccountId, checkStatus);
            if (customer == null)
            {
                throw new ApplicationException("客户帐号不存在！");
            }
            var viewModel = new HealthMonitoringViewModel
            {
                HealthReportStatus=healthStatus,
                CheckStatus = checkStatus.ToString()
            };
            viewModel.CustomerInfo = customer;
            return viewModel;
        }
        /// <summary>
        /// 根据客户编号和性别获取对应的健康数据列表
        /// </summary>
        /// <param name="id">客户编号</param>
        /// <param name="sex">性别</param>
        /// <returns></returns>
        private List<HealthManageInfoViewModel> GetHealthManageInfoList(int id, string sex)
        {
            List<HealthManageInfoViewModel> hmivList = new List<HealthManageInfoViewModel>();

            var healthList = _healthManageReportService.GetHealthManageInfo(id).ToList();
            var healthCategory = healthList.Select(x => x.TitleName).Distinct().ToList();
            foreach (var item in healthList)
            {
                HealthManageInfoViewModel hmivModel = new HealthManageInfoViewModel();
                hmivModel.num = item.num;
                hmivModel.TitleName = item.TitleName;
                hmivModel.ColVal = item.ColVal;
                hmivModel.ColName= HealthHeplerExtensions.GetHealthFieldColumnName(item.ColName);
                hmivModel.ColOK = true;
                hmivModel.ColPoint = true;
                hmivModel.HealthStandard = HealthHeplerExtensions.GetHealthStandardValue(hmivModel.ColName);
                HealthPhysicalEnum pEnum;
                int hpStatus = 0;
                if (!string.IsNullOrEmpty(hmivModel.ColVal) && hmivModel.ColVal != "--")
                {
                    if (hmivModel.ColName == "血压")
                    {
                        pEnum = HealthHeplerExtensions.GetHealthPhysicalEnumValue("收缩压");
                        hpStatus = (int)HealthHeplerExtensions.HealthPhysicalResult(item.ColVal.Split('/')[0], pEnum);

                        if (hpStatus == 2 || hpStatus == 4)
                        {
                            hmivModel.ColOK = false;
                        }
                        if (hpStatus > 2)
                        {
                            hmivModel.ColPoint = false;
                        }

                        pEnum = HealthHeplerExtensions.GetHealthPhysicalEnumValue("舒张压");
                        hpStatus = (int)HealthHeplerExtensions.HealthPhysicalResult(item.ColVal.Split('/')[1], pEnum);
                        if (hpStatus == 2 || hpStatus == 4)
                        {
                            hmivModel.ColOK = false;
                        }
                        if (hpStatus > 2)
                        {
                            hmivModel.ColPoint = false;
                        }
                    }
                    else
                    {
                        pEnum = HealthHeplerExtensions.GetHealthPhysicalEnumValue(hmivModel.ColName);
                        if (hmivModel.ColName == "血糖")
                        {
                            //其它血液学检查
                            var bloodSugarCustomer = _fetcher.Query<HealthManageBloodSugar>().AsEnumerable().Where(m => m.Customer != null && m.Customer.Id == id).OrderByDescending(m => m.CheckDate).FirstOrDefault();
                            hmivModel.ColVal = hmivModel.ColVal + "(" + GetBloodSugarType(bloodSugarCustomer.BloodSugarType) + ")";
                            hpStatus = (int)HealthHeplerExtensions.HealthPhysicalResult(item.ColVal, pEnum, bloodSugarCustomer.BloodSugarType);
                        }
                        else if (hmivModel.ColName == "腰臀比"
                            || hmivModel.ColName == "血尿酸"
                            || hmivModel.ColName == "血红蛋白"
                            || hmivModel.ColName == "红细胞比容"
                            || hmivModel.ColName == "脂肪"
                            || hmivModel.ColName == "代谢"
                            || hmivModel.ColName == "水分")
                        {
                            hpStatus = (int)HealthHeplerExtensions.HealthPhysicalResult(item.ColVal, pEnum, (Sex)Enum.Parse(typeof(Sex), sex));
                        }
                        else
                        {
                            hpStatus = (int)HealthHeplerExtensions.HealthPhysicalResult(item.ColVal, pEnum);
                        }

                        if (hpStatus == 2 || hpStatus == 4)
                        {
                            hmivModel.ColOK = false;
                        }
                        if (hpStatus > 2)
                        {
                            hmivModel.ColPoint = false;
                        }
                    }
                    if (hmivModel.num < 2)
                    {
                        hmivModel.ColPoint = false;
                    }
                }
                else
                {
                    hmivModel.ColPoint = false;
                }
                hmivList.Add(hmivModel);
            }
            return hmivList;
        }

        /// <summary>
        /// 健康检测图表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="healthStatus"></param>
        /// <param name="detailType"></param>
        /// <returns></returns>
        public ActionResult HealthDetail(int id, int healthStatus, string ColName)
        {
            var viewModel= GetHealthMonitoringViewModel(id, healthStatus);
            viewModel.DetailTypeName = ColName;
            var result = new HealthDetails();
            result.Healths = GetHealthDetails(viewModel.CustomerInfo.CustomerId, ColName);
            result.MaxValue = Math.Ceiling(result.Healths.Max(m => decimal.Parse(m.Value)));
            result.MinValue = Math.Floor(result.Healths.Min(m => decimal.Parse(m.Value)));
            if (result.MinValue == result.MaxValue)
            {
                result.MaxValue = result.MaxValue + 1;
                result.MinValue = result.MinValue - 1;
            }
            viewModel.HealthDetailInfo = result;
            return View("~/Views/HealthManagement/HealthDetail.cshtml", viewModel);
        }
        /// <summary>
        /// 健康检测图表(血压)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="healthStatus"></param>
        /// <param name="detailType"></param>
        /// <returns></returns>
        public ActionResult HealthDetailForBloodPresure(int id, int healthStatus, string ColName)
        {
            var viewModel = GetHealthMonitoringViewModel(id, healthStatus);
            viewModel.DetailTypeName = ColName;
            var result = new DetailForBloodPresureHistories();
            result.Histories = GetDetailForBloodPresureHistories(viewModel.CustomerInfo.CustomerId);
            decimal highMax= Math.Ceiling(result.Histories.Max(m => decimal.Parse(m.ValueHigh)));
            decimal lowMax = Math.Ceiling(result.Histories.Max(m => decimal.Parse(m.ValueLow)));
            decimal highMin = Math.Ceiling(result.Histories.Min(m => decimal.Parse(m.ValueHigh)));
            decimal lowMin = Math.Ceiling(result.Histories.Min(m => decimal.Parse(m.ValueLow)));
            result.MaxValue = highMax> lowMax ? highMax : lowMax;
            result.MinValue = highMin < lowMin ? highMin : lowMin;
            if (result.MinValue == result.MaxValue)
            {
                result.MaxValue = result.MaxValue + 1;
                result.MinValue = result.MinValue - 1;
            }
            result.Categorys = new List<string>();
            result.Categorys.Add("高血压");
            result.Categorys.Add("低血压");
            viewModel.CustomerHealthForBloodPresureInfo = result;
            return View("~/Views/HealthManagement/HealthDetailForBloodPresure.cshtml", viewModel);
        }
        /// <summary>
        /// 根据客户编号获取血压图表数据源
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private List<DetailHistoryForBloodPresureViewModel> GetDetailForBloodPresureHistories(int id)
        {
            var detailHistoryViewModels = _fetcher.Query<HealthManageBloodPresureAndPulse>()
                    .Where(m => (m.Customer != null && m.Customer.Id == id) || (m.Guest != null && m.Guest.Id == id))
                    .OrderByDescending(m=>m.CheckDate)
                    .Where(x => x.Sbp != null && x.Sbp != "")
                    .Take(5).OrderBy(m=>m.CheckDate).Select(m =>
                        new DetailHistoryForBloodPresureViewModel
                        {
                            ValueHigh = m.Sbp,
                            ValueLow = m.Dbp,
                            CheckDate = m.CheckDate.ToString("yyyy-MM-dd HH:mm"),
                            DetailType = "血压",
                            Unit = Mol
                        }).ToList();



            return detailHistoryViewModels;
        }
        /// <summary>
        /// 根据客户编号和指标项获取数据源
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Detailtype"></param>
        /// <returns></returns>
        private List<HealthDetailViewModel> GetHealthDetails(int id, string ColName)
        {

            var healthDetailViewModelList = new List<HealthDetailViewModel>();

            switch (ColName)
            {
                case "呼吸":
                    //目前没有呼吸的值
                    break;
                case "体温":
                    healthDetailViewModelList = _fetcher.Query<HealthManageTemperature>()
                        .Where(m => (m.Customer != null && m.Customer.Id == id) ||
                                    (m.Guest != null && m.Guest.Id == id))
                                    .OrderByDescending(m => m.CheckDate)
                                    .Where(x => x.Temperature != null && x.Temperature != "")
                                    .Take(5).OrderBy(m => m.CheckDate).Select(m =>
                                new HealthDetailViewModel
                                {
                                    Value = m.Temperature,
                                    CheckDate = m.CheckDate.ToString("yyyy-MM-dd HH:mm"),
                                    DetailType = ColName,
                                    Unit = Temperature
                                }).ToList();
                    break;
                case "脉搏":
                    healthDetailViewModelList = _fetcher.Query<HealthManageBloodPresureAndPulse>()
                        .Where(m => (m.Customer != null && m.Customer.Id == id) ||
                                    (m.Guest != null && m.Guest.Id == id)).OrderByDescending(m => m.CheckDate)
                                    .Where(x => x.Pulse != null && x.Pulse != "")
                                    .Take(5).OrderBy(m => m.CheckDate).Select(m =>
                                new HealthDetailViewModel
                                {
                                    Value = m.Pulse,
                                    CheckDate = m.CheckDate.ToString("yyyy-MM-dd HH:mm"),
                                    DetailType = ColName,
                                    Unit = TimePerMinutes
                                }).ToList();
                    break;
                case "心率":
                    healthDetailViewModelList = _fetcher.Query<HealthManageBloodPresureAndPulse>()
                        .Where(m => (m.Customer != null && m.Customer.Id == id) ||
                                    (m.Guest != null && m.Guest.Id == id)).OrderByDescending(m => m.CheckDate)
                                    .Where(x => x.HeartRate != null && x.HeartRate != "")
                                    .Take(5).OrderBy(m => m.CheckDate).Select(m =>
                                new HealthDetailViewModel
                                {
                                    Value = m.HeartRate,
                                    CheckDate = m.CheckDate.ToString("yyyy-MM-dd HH:mm"),
                                    DetailType = ColName,
                                    Unit = TimePerMinutes
                                }).ToList();
                    break;
                case "左眼视力":
                    healthDetailViewModelList = _fetcher.Query<HealthManageVision>()
                        .Where(m => (m.Customer != null && m.Customer.Id == id) ||
                                    (m.Guest != null && m.Guest.Id == id)).OrderByDescending(m => m.CheckDate)
                                    .Where(x => x.LeftVision != null && x.LeftVision != "")
                                    .Take(5).OrderBy(m => m.CheckDate).Select(m =>
                                new HealthDetailViewModel
                                {
                                    Value = m.LeftVision,
                                    CheckDate = m.CheckDate.ToString("yyyy-MM-dd HH:mm"),
                                    DetailType = ColName,
                                    Unit = ""
                                }).ToList();
                    break;
                case "右眼视力":
                    healthDetailViewModelList = _fetcher.Query<HealthManageVision>()
                        .Where(m => (m.Customer != null && m.Customer.Id == id) ||
                                    (m.Guest != null && m.Guest.Id == id)).OrderByDescending(m => m.CheckDate)
                                    .Where(x => x.RightVision != null && x.RightVision != "")
                                    .Take(5).OrderBy(m => m.CheckDate).Select(m =>
                                new HealthDetailViewModel
                                {
                                    Value = m.RightVision,
                                    CheckDate = m.CheckDate.ToString("yyyy-MM-dd HH:mm"),
                                    DetailType = ColName,
                                    Unit = ""
                                }).ToList();
                    break;
                case "总胆固醇":
                    healthDetailViewModelList = _fetcher.Query<HealthManageBloodFat>()
                        .Where(m => (m.Customer != null && m.Customer.Id == id) ||
                                    (m.Guest != null && m.Guest.Id == id)).OrderByDescending(m => m.CheckDate)
                                    .Where(x => x.TChol != null && x.TChol != "")
                                    .Take(5).OrderBy(m => m.CheckDate).Select(m =>
                              new HealthDetailViewModel
                              {
                                  Value = m.TChol,
                                  CheckDate = m.CheckDate.ToString("yyyy-MM-dd HH:mm"),
                                  DetailType = ColName,
                                  Unit = Mol
                              }).ToList();
                    break;
                case "甘油三酯":
                    healthDetailViewModelList = _fetcher.Query<HealthManageBloodFat>()
                        .Where(m => (m.Customer != null && m.Customer.Id == id) ||
                                    (m.Guest != null && m.Guest.Id == id)).OrderByDescending(m => m.CheckDate)
                                    .Where(x => x.Trig != null && x.Trig != "")
                                    .Take(5).OrderBy(m => m.CheckDate).Select(m =>
                                new HealthDetailViewModel
                                {
                                    Value = m.Trig,
                                    CheckDate = m.CheckDate.ToString("yyyy-MM-dd HH:mm"),
                                    DetailType = ColName,
                                    Unit = Mol
                                }).ToList();
                    break;
                case "高密度脂蛋白胆固醇":
                    healthDetailViewModelList = _fetcher.Query<HealthManageBloodFat>()
                        .Where(m => (m.Customer != null && m.Customer.Id == id) ||
                                    (m.Guest != null && m.Guest.Id == id)).OrderByDescending(m => m.CheckDate)
                                    .Where(x => x.Hdl != null && x.Hdl != "")
                                    .Take(5).OrderBy(m => m.CheckDate).Select(m =>
                                new HealthDetailViewModel
                                {
                                    Value = m.Hdl,
                                    CheckDate = m.CheckDate.ToString("yyyy-MM-dd HH:mm"),
                                    DetailType = ColName,
                                    Unit = Mol
                                }).ToList();
                    break;
                case "低密度脂蛋白胆固醇":
                    healthDetailViewModelList = _fetcher.Query<HealthManageBloodFat>()
                        .Where(m => (m.Customer != null && m.Customer.Id == id) ||
                                    (m.Guest != null && m.Guest.Id == id)).OrderByDescending(m => m.CheckDate)
                                    .Where(x => x.Ldl != null && x.Ldl != "")
                                    .Take(5).OrderBy(m => m.CheckDate).Select(m =>
                                new HealthDetailViewModel
                                {
                                    Value = m.Ldl,
                                    CheckDate = m.CheckDate.ToString("yyyy-MM-dd HH:mm"),
                                    DetailType = ColName,
                                    Unit = Mol
                                }).ToList();
                    break;
                case "尿胆原":
                    healthDetailViewModelList = _fetcher.Query<HealthManageUrine>()
                        .Where(m => (m.Customer != null && m.Customer.Id == id) ||
                                    (m.Guest != null && m.Guest.Id == id)).OrderByDescending(m => m.CheckDate)
                                    .Where(x => x.URO != null && x.URO != "")
                                    .Take(5).OrderBy(m => m.CheckDate).Select(m =>
                                new HealthDetailViewModel
                                {
                                    Value = m.URO,
                                    CheckDate = m.CheckDate.ToString("yyyy-MM-dd HH:mm"),
                                    DetailType = ColName,
                                    Unit = ""
                                }).ToList();
                    break;
                case "尿潜血":
                    healthDetailViewModelList = _fetcher.Query<HealthManageUrine>()
                        .Where(m => (m.Customer != null && m.Customer.Id == id) ||
                                    (m.Guest != null && m.Guest.Id == id)).OrderByDescending(m => m.CheckDate)
                                    .Where(x => x.BLD != null && x.BLD != "")
                                    .Take(5).OrderBy(m => m.CheckDate).Select(m =>
                                new HealthDetailViewModel
                                {
                                    Value = m.BLD,
                                    CheckDate = m.CheckDate.ToString("yyyy-MM-dd HH:mm"),
                                    DetailType = ColName,
                                    Unit = ""
                                }).ToList();
                    break;
                case "胆红素":
                    healthDetailViewModelList = _fetcher.Query<HealthManageUrine>()
                        .Where(m => (m.Customer != null && m.Customer.Id == id) ||
                                    (m.Guest != null && m.Guest.Id == id)).OrderByDescending(m => m.CheckDate)
                                    .Where(x => x.BIL != null && x.BIL != "")
                                    .Take(5).OrderBy(m => m.CheckDate).Select(m =>
                                new HealthDetailViewModel
                                {
                                    Value = m.BIL,
                                    CheckDate = m.CheckDate.ToString("yyyy-MM-dd HH:mm"),
                                    DetailType = ColName,
                                    Unit = ""
                                }).ToList();
                    break;
                case "酮体":
                    healthDetailViewModelList = _fetcher.Query<HealthManageUrine>()
                        .Where(m => (m.Customer != null && m.Customer.Id == id) ||
                                    (m.Guest != null && m.Guest.Id == id)).OrderByDescending(m => m.CheckDate)
                                    .Where(x => x.KET != null && x.KET != "")
                                    .Take(5).OrderBy(m => m.CheckDate).Select(m =>
                                new HealthDetailViewModel
                                {
                                    Value = m.KET,
                                    CheckDate = m.CheckDate.ToString("yyyy-MM-dd HH:mm"),
                                    DetailType = ColName,
                                    Unit = ""
                                }).ToList();
                    break;
                case "尿糖":
                    healthDetailViewModelList = _fetcher.Query<HealthManageUrine>()
                        .Where(m => (m.Customer != null && m.Customer.Id == id) ||
                                    (m.Guest != null && m.Guest.Id == id)).OrderByDescending(m => m.CheckDate)
                                    .Where(x => x.GLU != null && x.GLU != "")
                                    .Take(5).OrderBy(m => m.CheckDate).Select(m =>
                                new HealthDetailViewModel
                                {
                                    Value = m.GLU,
                                    CheckDate = m.CheckDate.ToString("yyyy-MM-dd HH:mm"),
                                    DetailType = ColName,
                                    Unit = ""
                                }).ToList();
                    break;
                case "尿蛋白":
                    healthDetailViewModelList = _fetcher.Query<HealthManageUrine>()
                        .Where(m => (m.Customer != null && m.Customer.Id == id) ||
                                    (m.Guest != null && m.Guest.Id == id)).OrderByDescending(m => m.CheckDate)
                                    .Where(x => x.PRO != null && x.PRO != "")
                                    .Take(5).OrderBy(m => m.CheckDate).Select(m =>
                                new HealthDetailViewModel
                                {
                                    Value = m.PRO,
                                    CheckDate = m.CheckDate.ToString("yyyy-MM-dd HH:mm"),
                                    DetailType = ColName,
                                    Unit = ""
                                }).ToList();
                    break;
                case "酸碱度":
                    healthDetailViewModelList = _fetcher.Query<HealthManageUrine>()
                        .Where(m => (m.Customer != null && m.Customer.Id == id) ||
                                    (m.Guest != null && m.Guest.Id == id)).OrderByDescending(m => m.CheckDate)
                                    .Where(x => x.PH != null && x.PH != "")
                                    .Take(5).OrderBy(m => m.CheckDate).Select(m =>
                                new HealthDetailViewModel
                                {
                                    Value = m.PH,
                                    CheckDate = m.CheckDate.ToString("yyyy-MM-dd HH:mm"),
                                    DetailType = ColName,
                                    Unit = ""
                                }).ToList();
                    break;
                case "亚硝酸盐":
                    healthDetailViewModelList = _fetcher.Query<HealthManageUrine>()
                        .Where(m => (m.Customer != null && m.Customer.Id == id) ||
                                    (m.Guest != null && m.Guest.Id == id)).OrderByDescending(m => m.CheckDate)
                                    .Where(x => x.NIT != null && x.NIT != "")
                                    .Take(5).OrderBy(m => m.CheckDate).Select(m =>
                                new HealthDetailViewModel
                                {
                                    Value = m.NIT,
                                    CheckDate = m.CheckDate.ToString("yyyy-MM-dd HH:mm"),
                                    DetailType = ColName,
                                    Unit = ""
                                }).ToList();
                    break;
                case "尿白细胞":
                    healthDetailViewModelList = _fetcher.Query<HealthManageUrine>()
                        .Where(m => (m.Customer != null && m.Customer.Id == id) ||
                                    (m.Guest != null && m.Guest.Id == id)).OrderByDescending(m => m.CheckDate)
                                    .Where(x => x.LEU != null && x.LEU != "")
                                    .Take(5).OrderBy(m => m.CheckDate).Select(m =>
                                new HealthDetailViewModel
                                {
                                    Value = m.LEU,
                                    CheckDate = m.CheckDate.ToString("yyyy-MM-dd HH:mm"),
                                    DetailType = ColName,
                                    Unit = ""
                                }).ToList();
                    break;
                case "尿比重":
                    healthDetailViewModelList = _fetcher.Query<HealthManageUrine>()
                        .Where(m => (m.Customer != null && m.Customer.Id == id) ||
                                    (m.Guest != null && m.Guest.Id == id)).OrderByDescending(m => m.CheckDate)
                                    .Where(x => x.SG != null && x.SG != "")
                                    .Take(5).OrderBy(m => m.CheckDate).Select(m =>
                                new HealthDetailViewModel
                                {
                                    Value = m.SG,
                                    CheckDate = m.CheckDate.ToString("yyyy-MM-dd HH:mm"),
                                    DetailType = ColName,
                                    Unit = ""
                                }).ToList();
                    break;
                case "隐血":
                    healthDetailViewModelList = _fetcher.Query<HealthManageUrine>()
                        .Where(m => (m.Customer != null && m.Customer.Id == id) ||
                                    (m.Guest != null && m.Guest.Id == id)).OrderByDescending(m => m.CheckDate)
                                    .Where(x => x.BLO != null && x.BLO != "")
                                    .Take(5).OrderBy(m => m.CheckDate).Select(m =>
                                new HealthDetailViewModel
                                {
                                    Value = m.BLO,
                                    CheckDate = m.CheckDate.ToString("yyyy-MM-dd HH:mm"),
                                    DetailType = ColName,
                                    Unit = ""
                                }).ToList();
                    break;
                case "维生素":
                    healthDetailViewModelList = _fetcher.Query<HealthManageUrine>()
                        .Where(m => (m.Customer != null && m.Customer.Id == id) ||
                                    (m.Guest != null && m.Guest.Id == id)).OrderByDescending(m => m.CheckDate)
                                    .Where(x => x.VC != null && x.VC != "")
                                    .Take(5).OrderBy(m => m.CheckDate).Select(m =>
                                new HealthDetailViewModel
                                {
                                    Value = m.VC,
                                    CheckDate = m.CheckDate.ToString("yyyy-MM-dd HH:mm"),
                                    DetailType = ColName,
                                    Unit = ""
                                }).ToList();
                    break;
                case "微白蛋白":
                    healthDetailViewModelList = _fetcher.Query<HealthManageUrine>()
                        .Where(m => (m.Customer != null && m.Customer.Id == id) ||
                                    (m.Guest != null && m.Guest.Id == id)).OrderByDescending(m => m.CheckDate)
                                    .Where(x => x.MAL != null && x.MAL != "")
                                    .Take(5).OrderBy(m => m.CheckDate).Select(m =>
                                new HealthDetailViewModel
                                {
                                    Value = m.MAL,
                                    CheckDate = m.CheckDate.ToString("yyyy-MM-dd HH:mm"),
                                    DetailType = ColName,
                                    Unit = ""
                                }).ToList();
                    break;
                case "肌酐":
                    healthDetailViewModelList = _fetcher.Query<HealthManageUrine>()
                        .Where(m => (m.Customer != null && m.Customer.Id == id) ||
                                    (m.Guest != null && m.Guest.Id == id)).OrderByDescending(m => m.CheckDate)
                                    .Where(x => x.CR != null && x.CR != "")
                                    .Take(5).OrderBy(m => m.CheckDate).Select(m =>
                                new HealthDetailViewModel
                                {
                                    Value = m.CR,
                                    CheckDate = m.CheckDate.ToString("yyyy-MM-dd HH:mm"),
                                    DetailType = ColName,
                                    Unit = ""
                                }).ToList();
                    break;
                case "钙离子":
                    healthDetailViewModelList = _fetcher.Query<HealthManageUrine>()
                        .Where(m => (m.Customer != null && m.Customer.Id == id) ||
                                    (m.Guest != null && m.Guest.Id == id)).OrderByDescending(m => m.CheckDate)
                                    .Where(x => x.UCA != null && x.UCA != "")
                                    .Take(5).OrderBy(m => m.CheckDate).Select(m =>
                                new HealthDetailViewModel
                                {
                                    Value = m.UCA,
                                    CheckDate = m.CheckDate.ToString("yyyy-MM-dd HH:mm"),
                                    DetailType = ColName,
                                    Unit = ""
                                }).ToList();
                    break;
                case "血糖":
                    healthDetailViewModelList = _fetcher.Query<HealthManageBloodSugar>()
                        .Where(m => (m.Customer != null && m.Customer.Id == id) ||
                                    (m.Guest != null && m.Guest.Id == id)).OrderByDescending(m => m.CheckDate)
                                    .Where(x => x.Glu != null && x.Glu != "")
                                    .Take(5).OrderBy(m => m.CheckDate).Select(m =>
                                new HealthDetailViewModel
                                {
                                    Value = m.Glu,
                                    CheckDate = m.CheckDate.ToString("yyyy-MM-dd HH:mm"),
                                    DetailType = ColName,
                                    Unit = Mol
                                }).ToList();
                    break;
                case "血氧饱和度":
                    healthDetailViewModelList = _fetcher.Query<HealthManageBloodOxygen>()
                        .Where(m => (m.Customer != null && m.Customer.Id == id) ||
                                    (m.Guest != null && m.Guest.Id == id)).OrderByDescending(m => m.CheckDate)
                                    .Where(x => x.Bo != null && x.Bo != "")
                                    .Take(5).OrderBy(m => m.CheckDate).Select(m =>
                                new HealthDetailViewModel
                                {
                                    Value = m.Bo,
                                    CheckDate = m.CheckDate.ToString("yyyy-MM-dd HH:mm"),
                                    DetailType = ColName,
                                    Unit = "%"
                                }).ToList();
                    break;
                case "血尿酸":
                    healthDetailViewModelList = _fetcher.Query<HealthManageUA>()
                        .Where(m => (m.Customer != null && m.Customer.Id == id) ||
                                    (m.Guest != null && m.Guest.Id == id)).OrderByDescending(m => m.CheckDate)
                                    .Where(x => x.Ua != null && x.Ua != "")
                                    .Take(5).OrderBy(m => m.CheckDate).Select(m =>
                                new HealthDetailViewModel
                                {
                                    Value = m.Ua,
                                    CheckDate = m.CheckDate.ToString("yyyy-MM-dd HH:mm"),
                                    DetailType = ColName,
                                    Unit = Mol
                                }).ToList();
                    break;
                case "血红蛋白":
                    healthDetailViewModelList = _fetcher.Query<HealthManageHB>()
                        .Where(m => (m.Customer != null && m.Customer.Id == id) ||
                                    (m.Guest != null && m.Guest.Id == id)).OrderByDescending(m => m.CheckDate)
                                    .Where(x => x.Hb != null && x.Hb != "")
                                    .Take(5).OrderBy(m => m.CheckDate).Select(m =>
                                new HealthDetailViewModel
                                {
                                    Value = m.Hb,
                                    CheckDate = m.CheckDate.ToString("yyyy-MM-dd HH:mm"),
                                    DetailType = ColName,
                                    Unit = Mol
                                }).ToList();
                    break;
                case "红细胞比容":
                    healthDetailViewModelList = _fetcher.Query<HealthManageHB>()
                        .Where(m => (m.Customer != null && m.Customer.Id == id) ||
                                    (m.Guest != null && m.Guest.Id == id)).OrderByDescending(m => m.CheckDate)
                                    .Where(x => x.Hct != null && x.Hct != "")
                                    .Take(5).OrderBy(m => m.CheckDate).Select(m =>
                                new HealthDetailViewModel
                                {
                                    Value = m.Hct,
                                    CheckDate = m.CheckDate.ToString("yyyy-MM-dd HH:mm"),
                                    DetailType = ColName,
                                    Unit = ""
                                }).ToList();
                    break;
                case "糖化血红蛋白":
                    healthDetailViewModelList = _fetcher.Query<HealthManageSugarHct>()
                        .Where(m => (m.Customer != null && m.Customer.Id == id) ||
                                    (m.Guest != null && m.Guest.Id == id)).OrderByDescending(m => m.CheckDate)
                                    .Where(x => x.SugarHct != null && x.SugarHct != "")
                                    .Take(5).OrderBy(m => m.CheckDate).Select(m =>
                                new HealthDetailViewModel
                                {
                                    Value = m.SugarHct,
                                    CheckDate = m.CheckDate.ToString("yyyy-MM-dd HH:mm"),
                                    DetailType = ColName,
                                    Unit = ""
                                }).ToList();
                    break;
                case "身高":
                    healthDetailViewModelList = _fetcher.Query<HealthManageHeightWeight>()
                        .Where(m => (m.Customer != null && m.Customer.Id == id) ||
                                    (m.Guest != null && m.Guest.Id == id)).OrderByDescending(m => m.CheckDate)
                                    .Where(x => x.Height != null && x.Height != "")
                                    .Take(5).OrderBy(m => m.CheckDate).Select(m =>
                                new HealthDetailViewModel
                                {
                                    Value = m.Height,
                                    CheckDate = m.CheckDate.ToString("yyyy-MM-dd HH:mm"),
                                    DetailType = ColName,
                                    Unit = Cm
                                }).ToList();
                    break;
                case "体重":
                    healthDetailViewModelList = _fetcher.Query<HealthManageHeightWeight>()
                        .Where(m => (m.Customer != null && m.Customer.Id == id) ||
                                    (m.Guest != null && m.Guest.Id == id)).OrderByDescending(m => m.CheckDate)
                                    .Where(x => x.Weight != null && x.Weight != "")
                                    .Take(5).OrderBy(m => m.CheckDate).Select(m =>
                                new HealthDetailViewModel
                                {
                                    Value = m.Weight,
                                    CheckDate = m.CheckDate.ToString("yyyy-MM-dd HH:mm"),
                                    DetailType = ColName,
                                    Unit = Kg
                                }).ToList();
                    break;
                case "BMI":
                    healthDetailViewModelList = _fetcher.Query<HealthManageHeightWeight>()
                        .Where(m => (m.Customer != null && m.Customer.Id == id) ||
                                    (m.Guest != null && m.Guest.Id == id)).OrderByDescending(m => m.CheckDate)
                                    .Where(x => x.Bmi != null && x.Bmi != "")
                                    .Take(5).OrderBy(m => m.CheckDate).Select(m =>
                                new HealthDetailViewModel
                                {
                                    Value = m.Bmi,
                                    CheckDate = m.CheckDate.ToString("yyyy-MM-dd HH:mm"),
                                    DetailType = ColName,
                                    Unit = "kg/m²"
                                }).ToList();
                    break;
                case "腰围":
                    healthDetailViewModelList = _fetcher.Query<HealthManageWaistHip>()
                        .Where(m => (m.Customer != null && m.Customer.Id == id) ||
                                    (m.Guest != null && m.Guest.Id == id)).OrderByDescending(m => m.CheckDate)
                                    .Where(x => x.WaistLine != null && x.WaistLine != "")
                                    .Take(5).OrderBy(m => m.CheckDate).Select(m =>
                                new HealthDetailViewModel
                                {
                                    Value = m.WaistLine,
                                    CheckDate = m.CheckDate.ToString("yyyy-MM-dd HH:mm"),
                                    DetailType = ColName,
                                    Unit = Cm
                                }).ToList();
                    break;
                case "臀围":
                    healthDetailViewModelList = _fetcher.Query<HealthManageWaistHip>()
                        .Where(m => (m.Customer != null && m.Customer.Id == id) ||
                                    (m.Guest != null && m.Guest.Id == id)).OrderByDescending(m => m.CheckDate)
                                    .Where(x => x.HipLine != null && x.HipLine != "")
                                    .Take(5).OrderBy(m => m.CheckDate).Select(m =>
                                new HealthDetailViewModel
                                {
                                    Value = m.HipLine,
                                    CheckDate = m.CheckDate.ToString("yyyy-MM-dd HH:mm"),
                                    DetailType = ColName,
                                    Unit = Cm
                                }).ToList();
                    break;
                case "腰臀比":
                    healthDetailViewModelList = _fetcher.Query<HealthManageWaistHip>()
                        .Where(m => (m.Customer != null && m.Customer.Id == id) ||
                                    (m.Guest != null && m.Guest.Id == id)).OrderByDescending(m => m.CheckDate)
                                    .Where(x => x.Whr != null && x.Whr != "")
                                    .Take(5).OrderBy(m => m.CheckDate).Select(m =>
                                new HealthDetailViewModel
                                {
                                    Value = m.Whr,
                                    CheckDate = m.CheckDate.ToString("yyyy-MM-dd HH:mm"),
                                    DetailType = ColName,
                                    Unit = "%"
                                }).ToList();
                    break;
                case "肺活量":
                    healthDetailViewModelList = _fetcher.Query<HealthManageBreath>()
                        .Where(m => (m.Customer != null && m.Customer.Id == id) ||
                                    (m.Guest != null && m.Guest.Id == id)).OrderByDescending(m => m.CheckDate)
                                    .Where(x => x.FVC != null && x.FVC != "")
                                    .Take(5).OrderBy(m => m.CheckDate).Select(m =>
                                new HealthDetailViewModel
                                {
                                    Value = m.FVC,
                                    CheckDate = m.CheckDate.ToString("yyyy-MM-dd HH:mm"),
                                    DetailType = ColName,
                                    Unit = ""
                                }).ToList();
                    break;
                case "最大用力呼气流量":
                    healthDetailViewModelList = _fetcher.Query<HealthManageBreath>()
                        .Where(m => (m.Customer != null && m.Customer.Id == id) ||
                                    (m.Guest != null && m.Guest.Id == id)).OrderByDescending(m => m.CheckDate)
                                    .Where(x => x.PEF != null && x.PEF != "")
                                    .Take(5).OrderBy(m => m.CheckDate).Select(m =>
                                new HealthDetailViewModel
                                {
                                    Value = m.PEF,
                                    CheckDate = m.CheckDate.ToString("yyyy-MM-dd HH:mm"),
                                    DetailType = ColName,
                                    Unit = ""
                                }).ToList();
                    break;
                case "第一秒用力呼气量":
                    healthDetailViewModelList = _fetcher.Query<HealthManageBreath>()
                        .Where(m => (m.Customer != null && m.Customer.Id == id) ||
                                    (m.Guest != null && m.Guest.Id == id)).OrderByDescending(m => m.CheckDate)
                                    .Where(x => x.FEV1 != null && x.FEV1 != "")
                                    .Take(5).OrderBy(m => m.CheckDate).Select(m =>
                                new HealthDetailViewModel
                                {
                                    Value = m.FEV1,
                                    CheckDate = m.CheckDate.ToString("yyyy-MM-dd HH:mm"),
                                    DetailType = ColName,
                                    Unit = ""
                                }).ToList();
                    break;
                case "用力肺活量":
                    healthDetailViewModelList = _fetcher.Query<HealthManageBreath>()
                        .Where(m => (m.Customer != null && m.Customer.Id == id) ||
                                    (m.Guest != null && m.Guest.Id == id)).OrderByDescending(m => m.CheckDate)
                                    .Where(x => x.FVC != null && x.FVC != "")
                                    .Take(5).OrderBy(m => m.CheckDate).Select(m =>
                                new HealthDetailViewModel
                                {
                                    Value = m.FVC,
                                    CheckDate = m.CheckDate.ToString("yyyy-MM-dd HH:mm"),
                                    DetailType = ColName,
                                    Unit = ""
                                }).ToList();
                    break;
                case "脂肪":
                    healthDetailViewModelList = _fetcher.Query<HealthManageFat>()
                        .Where(m => (m.Customer != null && m.Customer.Id == id) ||
                                    (m.Guest != null && m.Guest.Id == id)).OrderByDescending(m => m.CheckDate)
                                    .Where(x => x.Fat != null && x.Fat != "")
                                    .Take(5).OrderBy(m => m.CheckDate).Select(m =>
                                new HealthDetailViewModel
                                {
                                    Value = m.Fat,
                                    CheckDate = m.CheckDate.ToString("yyyy-MM-dd HH:mm"),
                                    DetailType = ColName,
                                    Unit = "%"
                                }).ToList();
                    break;
                case "水分":
                    healthDetailViewModelList = _fetcher.Query<HealthManageFat>()
                        .Where(m => (m.Customer != null && m.Customer.Id == id) ||
                                    (m.Guest != null && m.Guest.Id == id)).OrderByDescending(m => m.CheckDate)
                                    .Where(x => x.Water != null && x.Water != "")
                                    .Take(5).OrderBy(m => m.CheckDate).Select(m =>
                                new HealthDetailViewModel
                                {
                                    Value = m.Water,
                                    CheckDate = m.CheckDate.ToString("yyyy-MM-dd HH:mm"),
                                    DetailType = ColName,
                                    Unit = "%"
                                }).ToList();
                    break;
                case "代谢":
                    healthDetailViewModelList = _fetcher.Query<HealthManageFat>()
                        .Where(m => (m.Customer != null && m.Customer.Id == id) ||
                                    (m.Guest != null && m.Guest.Id == id)).OrderByDescending(m => m.CheckDate)
                                    .Where(x => x.Bmr != null && x.Bmr != "")
                                    .Take(5).OrderBy(m => m.CheckDate).Select(m =>
                                new HealthDetailViewModel
                                {
                                    Value = m.Bmr,
                                    CheckDate = m.CheckDate.ToString("yyyy-MM-dd HH:mm"),
                                    DetailType = ColName,
                                    Unit = ""
                                }).ToList();
                    break;
                default:
                    break;

            }

            return healthDetailViewModelList;
        }
        /// <summary>
        /// 根据血糖类型（0、1、2、3）返回对应的文字
        /// </summary>
        /// <param name="bloodSugarValue"></param>
        /// <returns></returns>
        private string GetBloodSugarType(string bloodSugarValue)
        {
            if (string.IsNullOrEmpty(bloodSugarValue))
                return "空腹血糖";
            var result = string.Empty;
            switch (bloodSugarValue)
            {
                case "0":
                case "1":
                    result = "空腹血糖";
                    break;
                case "2":
                    result = "餐后血糖";
                    break;
                case "3":
                    result = "随机血糖";
                    break;
                default:
                    result = "空腹血糖";
                    break;
            }

            return result;
        }
        /// <summary>
        /// 新增和编辑健康报告保存事件
        /// </summary>
        /// <param name="HMRViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveHealthReport(HealthManageReportViewModel HMRViewModel)
        {
            if (string.IsNullOrEmpty(HMRViewModel.Id))
            {
                var command = new CreateHealthManageReportCommand
                {
                    CustomerId = HMRViewModel.customerId,
                    HReport = HMRViewModel.HReport,
                    HGuidance = HMRViewModel.HGuidance,
                    CheckDate = DateTime.Now,
                    HReportDate = DateTime.Now
                };
                _commandService.Execute(command);
            }
            else
            {
                var command = new EditHealthManageReportCommand
                {
                    Id = int.Parse(HMRViewModel.Id),
                    CustomerId = HMRViewModel.customerId,
                    HReport = HMRViewModel.HReport,
                    HGuidance = HMRViewModel.HGuidance,
                    CheckDate = DateTime.Now,
                    HReportDate = DateTime.Now
                };
                _commandService.Execute(command);
            }
            
            return Json(new { success = true, msg = "添加成功" }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 根据健康报告编号获取健康报告
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetHealthReportById(int Id)
        {
            var hmReportModel = _healthManageReportService.Get(Id);
            return Json(new { HReportDate = hmReportModel.HReportDate.ToString("yyyy年MM月dd HH:mm"), HReport = hmReportModel.HReport, HGuidance=hmReportModel.HGuidance }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 根据客户编号获取最近一次的健康报告信息
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetHealthReportByCustomerId(int CustomerId)
        {
            var hmReportModel = _healthManageReportService.Query(CustomerId).OrderByDescending(m => m.HReportDate).FirstOrDefault();
            if(hmReportModel!=null)
            {
                return Json(new { HReportDate = hmReportModel.HReportDate.ToString("yyyy-MM-dd"), HReport = hmReportModel.HReport, HGuidance = hmReportModel.HGuidance }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { HReportDate = "", HReport = "", HGuidance = "" }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除健康报告
        /// </summary>
        /// <param name="Id">健康报告编号</param>
        /// <param name="customerId">客户编号</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteHealthReportById(int Id,int customerId)
        {
            var hmReportList = _healthManageReportService.Query(customerId).Where(x => x.IsDelete == 0).ToList();
           
            int healthStatus = -1;

            if (hmReportList.OrderByDescending(m => m.HReportDate).FirstOrDefault().Id == Id)
            {
                healthStatus = 2;
            }
            //当删除最后一条健康报告时，更新CustomerHealth表HealthStatus为3 （只有去评估）  否则更新成去评估2
            if (hmReportList != null && hmReportList.Count == 1)
            {
                healthStatus = 3;
            }
           
            var command = new DeleteHealthManageReportCommand
            {
                HealthManageReportId = Id,
                CustomerId = customerId,
                CustomerHealthStatus = healthStatus
            };
            //删除是逻辑删除，更改IsDelete为1
            _commandService.Execute(command);
            return Json(new { success = true}, JsonRequestBehavior.AllowGet);
        }
    }
}