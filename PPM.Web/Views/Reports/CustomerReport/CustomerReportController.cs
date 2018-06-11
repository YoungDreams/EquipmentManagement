using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PensionInsurance.Entities;
using PensionInsurance.Query;
using PensionInsurance.Web.Views.Account;

namespace PensionInsurance.Web.Views.Reports.CustomerReport
{
    public class CustomerReportController : AuthorizedController
    {
        private readonly IReportQueryService _customerReportQueryService;
        private readonly IProjectQueryService _projectQueryService;
        public CustomerReportController(IReportQueryService reportQueryService, IProjectQueryService projectQueryService)
        {
            _customerReportQueryService = reportQueryService;
            _projectQueryService = projectQueryService;
        }

        public ActionResult Index()
        {
            IndexViewModel viewModel = new IndexViewModel
            {
                ProjectIds = new List<int>(),
                ProjectList = _projectQueryService.QueryValidProjectsByType(ProjectType.CB).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            return View("~/Views/Reports/CustomerReport/Index.cshtml",viewModel);
        }

        /// <summary>
        /// 年龄结构饼图
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAgeReportAll(ReportQuery query)
        {
            var report = _customerReportQueryService.GetAgeReportAll(query);
            return Json(new []
            {
                new
                {
                    name = "60以下",
                    value = report.AgeBelow60
                },
                new
                {
                    name = "61-65",
                    value = report.AgeBetween61And65
                },
                new
                {
                    name = "66-70",
                    value = report.AgeBetween66And70
                },
                new
                {
                    name = "71-75",
                    value = report.AgeBetween71And75
                },
                new
                {
                    name = "76-80",
                    value = report.AgeBetween76And80
                },
                new
                {
                    name = "81-85",
                    value = report.AgeBetween81And85
                },
                new
                {
                    name = "85以上",
                    value = report.AgeOver85
                }
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 年龄结构饼图(独立)
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAgeReportIndependence(ReportQuery query)
        {
            var report = _customerReportQueryService.GetAgeReportIndependence(query);
            return Json(new[]
            {
                new
                {
                    name = "60以下",
                    value = report.AgeBelow60
                },
                new
                {
                    name = "61-65",
                    value = report.AgeBetween61And65
                },
                new
                {
                    name = "66-70",
                    value = report.AgeBetween66And70
                },
                new
                {
                    name = "71-75",
                    value = report.AgeBetween71And75
                },
                new
                {
                    name = "76-80",
                    value = report.AgeBetween76And80
                },
                new
                {
                    name = "81-85",
                    value = report.AgeBetween81And85
                },
                new
                {
                    name = "85以上",
                    value = report.AgeOver85
                }
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 年龄结构饼图(生活照料)
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAgeReportLife(ReportQuery query)
        {
            var report = _customerReportQueryService.GetAgeReportLife(query);
            return Json(new[]
            {
                new
                {
                    name = "60以下",
                    value = report.AgeBelow60
                },
                new
                {
                    name = "61-65",
                    value = report.AgeBetween61And65
                },
                new
                {
                    name = "66-70",
                    value = report.AgeBetween66And70
                },
                new
                {
                    name = "71-75",
                    value = report.AgeBetween71And75
                },
                new
                {
                    name = "76-80",
                    value = report.AgeBetween76And80
                },
                new
                {
                    name = "81-85",
                    value = report.AgeBetween81And85
                },
                new
                {
                    name = "85以上",
                    value = report.AgeOver85
                }
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 年龄结构饼图(失智照护)
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAgeReportDementia(ReportQuery query)
        {
            var report = _customerReportQueryService.GetAgeReportDementia(query);
            return Json(new[]
            {
                new
                {
                    name = "60以下",
                    value = report.AgeBelow60
                },
                new
                {
                    name = "61-65",
                    value = report.AgeBetween61And65
                },
                new
                {
                    name = "66-70",
                    value = report.AgeBetween66And70
                },
                new
                {
                    name = "71-75",
                    value = report.AgeBetween71And75
                },
                new
                {
                    name = "76-80",
                    value = report.AgeBetween76And80
                },
                new
                {
                    name = "81-85",
                    value = report.AgeBetween81And85
                },
                new
                {
                    name = "85以上",
                    value = report.AgeOver85
                }
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 护理级别结构
        /// </summary> 
        /// <returns></returns>
        public ActionResult GetNursingLevelReport(ReportQuery query)
        {
            var report = _customerReportQueryService.GetNursingLevelReport(query);
            return Json(new[]
            {
                new
                {
                    name = "独立型",
                    value = report.IndependenceNum
                },
                new
                {
                    name = "生活照料型",
                    value = report.LifeNum
                },
                new
                {
                    name = "失智照护型",
                    value = report.DementiaNum
                }
            }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 失智照护
        /// </summary>
        /// <returns></returns>
        public ActionResult GetDementiaCare(ReportQuery query)
        {
            var report = _customerReportQueryService.GetDementiaCareReport(query);
            return Json(new[]
            {
                new
                {
                    name = "失智照护1级",
                    value = report.MC_INum
                },
                new
                {
                    name = "失智照护2级",
                    value = report.MC_IINum
                },
                new
                {
                    name = "失智照护3级",
                    value = report.MC_IIINum
                },
                new
                {
                    name = "失智照护4级",
                    value = report.MC_IVNum
                }
            }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 生活照料
        /// </summary>
        /// <returns></returns>
        public ActionResult GetLifeCare(ReportQuery query)
        {
            var report = _customerReportQueryService.GetLifeCareReport(query);
            return Json(new[]
            {
                new
                {
                    name = "生活照料1级",
                    value = report.AL_INum
                },
                new
                {
                    name = "生活照料2级",
                    value = report.AL_IINum
                },
                new
                {
                    name = "生活照料3级",
                    value = report.AL_IIINum
                },
                new
                {
                    name = "生活照料4级",
                    value = report.AL_IVNum
                }
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 独身（丧偶离异等）独自入住和年龄交叉结构
        /// </summary>
        /// <returns></returns>
        public ActionResult GetSingleReport(ReportQuery query)
        {
            var report = _customerReportQueryService.GetSingleReport(query);
            return Json(new[]
            {
                new
                {
                    name = "60以下",
                    value = report.AgeBelow60
                },
                new
                {
                    name = "61-65",
                    value = report.AgeBetween61And65
                },
                new
                {
                    name = "66-70",
                    value = report.AgeBetween66And70
                },
                new
                {
                    name = "71-75",
                    value = report.AgeBetween71And75
                },
                new
                {
                    name = "76-80",
                    value = report.AgeBetween76And80
                },
                new
                {
                    name = "81-85",
                    value = report.AgeBetween81And85
                },
                new
                {
                    name = "85以上",
                    value = report.AgeOver85
                }
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 与子女同住结构
        /// </summary>
        /// <returns></returns>
        public ActionResult GetLivingTheirChildrenReport(ReportQuery query)
        {
            var report = _customerReportQueryService.GetLivingTheirChildrenReport(query);
            return Json(new[]
            {
                new
                {
                    name = "同住",
                    value = report.LiveNum
                },
                new
                {
                    name = "不同住（同城）",
                    value = report.NotLiveCityNum
                },
                new
                {
                    name = "不同住（非同城）",
                    value = report.NotLiveNotCityNum
                },
                new
                {
                    name = "不同住（国外）",
                    value = report.NotLiveAbroadNum
                },
                new
                {
                    name = "无子女",
                    value = report.ChildlessNum
                }
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 客户来源结构
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCustomerSourceReport(ReportQuery query)
        {
            var report = _customerReportQueryService.GetCustomerSourceReport(query);
            return Json(report.Results.Select(x => new
            {
                name = x.Key,
                value = x.Value
            }), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 费用支出人结构
        /// </summary>
        /// <returns></returns>
        public ActionResult GetExpensePaymentReport(ReportQuery query)
        {
            var report = _customerReportQueryService.GetExpensePaymentReport(query);
            return Json(new[]
            {
                new
                {
                    name = "自己",
                    value = report.OwnNum
                },
                new
                {
                    name = "子女",
                    value = report.ChildrenNum
                },
                new
                {
                    name = "共同负担",
                    value = report.SharedNum
                }
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 房产是否出租结构
        /// </summary>
        /// <returns></returns>
        public ActionResult GetPremisesOutReport(ReportQuery query)
        {
            var report = _customerReportQueryService.GetPremisesOutReport(query);
            return Json(new[]
            {
                new
                {
                    name = "是",
                    value = report.YesNum
                },
                new
                {
                    name = "否",
                    value = report.NoNum
                }
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 退休前从事行业结构
        /// </summary>
        /// <returns></returns>
        public ActionResult GetWorkIndustryReport(ReportQuery query)
        {
            var report = _customerReportQueryService.GetWorkIndustryReport(query);
            return Json(new[]
            {
                new
                {
                    name = "政府",
                    value = report.GovernmentNum
                },
                new
                {
                    name = "金融",
                    value = report.FinanceNum
                },
                new
                {
                    name = "医药",
                    value = report.MedicineNum
                },
                new
                {
                    name = "科研",
                    value = report.ScientificNum
                },
                new
                {
                    name = "教育",
                    value = report.EducationNum
                },
                new
                {
                    name = "其他",
                    value = report.OtherNum
                },
                new
                {
                    name = "未知",
                    value = report.UnknownNum
                }
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 受教育水平结构
        /// </summary>
        /// <returns></returns>
        public ActionResult GetEducationReport(ReportQuery query)
        {
            var report = _customerReportQueryService.GetEducationReport(query);
            return Json(new[]
            {
                new
                {
                    name = "高中及以下",
                    value = report.HighSchoolNum
                },
                new
                {
                    name = "本科及以上",
                    value = report.BachelorNum
                }
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 入住客户渠道结构
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCustomerTypeReport(ReportQuery query)
        {
            var report = _customerReportQueryService.GetCustomerTypeReport(query);
            return Json(new[]
            {
                new
                {
                    name = "400来电",
                    value = report.PhoneCallNum
                },
                new
                {
                    name = ConsultingType.新开发.ToString(),
                    value = report.DevelopmentNum
                },
                new
                {
                    name = ConsultingType.直接来访.ToString(),
                    value = report.DirectVisitNum
                },
                new
                {
                    name = ConsultingType.已入住客户介绍.ToString(),
                    value = report.IntroduceNum
                },
                new
                {
                    name = ConsultingType.项目座机来电.ToString(),
                    value = report.ProjectPhoneCallNum
                }
            }, JsonRequestBehavior.AllowGet);
        }
    }
}