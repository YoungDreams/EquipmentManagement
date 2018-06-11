using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Foundation.Core;
using Foundation.Data;
using PensionInsurance.Entities;
using PensionInsurance.Query;
using PensionInsurance.Web.Views.Account;

namespace PensionInsurance.Web.Views.Sales.SalesRoomChart
{
    public class SalesRoomChartController : AuthorizedController
    {
        private readonly IFetcher _fetcher;
        private readonly IProjectQueryService _projectQueryService;
        private readonly ISalesRoomChartDetailQueryService _salesRoomChartDetailQueryService;
        // GET: PinControlChart
        public SalesRoomChartController(IFetcher fetcher, IProjectQueryService projectQueryService, ISalesRoomChartDetailQueryService salesRoomChartDetailQueryService)
        {
            _fetcher = fetcher;
            _projectQueryService = projectQueryService;
            _salesRoomChartDetailQueryService = salesRoomChartDetailQueryService;
        }

        public ActionResult Index()
        {
            var viewModel = new IndexViewModel
            {
                SalesRoomChartDetails = _salesRoomChartDetailQueryService.Query()
            };

            if (viewModel.SalesRoomChartDetails.Any() && viewModel.SalesRoomChartDetails.Count() == 1)
            {
                return RedirectToAction("Detail", new { id = viewModel.SalesRoomChartDetails.First().ProjectId });
            }

            return View("~/Views/Sales/SalesRoomChart/Index.cshtml", viewModel);
        }

        public ActionResult Detail(int id)
        {
            var project = _projectQueryService.Get(id);
            var rooms = _fetcher.Query<Room>().Where(x => x.FId != null && x.FId != "0" && x.Floor.Unit.Building.Project.Id == project.Id);
            var viewModel = new DetailViewModel
            {
                FreeRoomFids = rooms.Where(x => x.Status == RoomStatus.空房).Select(s => s.FId).ToArray(),
                OccupyRoomFids = rooms.Where(x => x.Status == RoomStatus.已住人).Select(s => s.FId).ToArray(),
                AllOccupyRoomFids = rooms.Where(x => x.Status == RoomStatus.全满).Select(s => s.FId).ToArray()
            };
            var mapCount = _fetcher.Query<FengMap>().Count(x => x.Project == project);

            viewModel.IsAreaMap = mapCount > 0;

            if (project != null)
            {
                viewModel.MapId = project.FmapId;
                viewModel.ProjectId = project.Id;
            }
            return View("~/Views/Sales/SalesRoomChart/Detail.cshtml", viewModel);
        }

        [HttpGet]
        public ViewResult MapDetail(int id, string fId)
        {
            var project = _projectQueryService.Get(id);
            var rooms = _fetcher.Query<Room>().Where(x => x.FId != null && x.FId != "0" && x.Floor.Unit.Building.Project.Id == project.Id);
            var fengMap = _fetcher.Query<FengMap>().FirstOrDefault(x => x.Project == project && x.FId == fId);
           
            var viewModel = new FengMapDetailViewMdoel
            {
                ProjectId = project.Id,
                FreeRoomFids = rooms.Where(x => x.Status == RoomStatus.空房).Select(s => s.FId).ToArray(),
                OccupyRoomFids = rooms.Where(x => x.Status == RoomStatus.已住人).Select(s => s.FId).ToArray(),
                AllOccupyRoomFids = rooms.Where(x => x.Status == RoomStatus.全满).Select(s => s.FId).ToArray()
            };
            if (fengMap != null)
            {
                viewModel.MapId = fengMap.MapId;
            }
            return View("~/Views/Sales/SalesRoomChart/FengMap.cshtml", viewModel);
        }

        /// <summary>
        /// 获取房间信息
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        public ActionResult GetRommInfo(string fid)
        {
            var fmapRomm =
                _fetcher.Query<Entities.Room>().FirstOrDefault(x => x.FId == fid);

            Result result = new Result();
            if (fmapRomm != null)
            {
                result.RommName = fmapRomm.Name;
                result.RommType = fmapRomm.Type;
                result.Beds = fmapRomm.Beds.Count();
                result.BedCost = fmapRomm.RoomType.ActualRoomCost(null).LongTermCost;

                var contractRoomChanges = _fetcher.Query<Entities.ContractRoomChange>()
                    .Where(x => x.NewRoom.Id == fmapRomm.Id && x.Contract.Status == ContractStatus.生效 && x.Contract.ActualEndTime.Value == x.ChangeEndDate);

                List<FmCustomer> lists = new List<FmCustomer>();
                foreach (var item in contractRoomChanges)
                {
                    FmCustomer fmCustomer = new FmCustomer();
                    fmCustomer.BedName = item.NewBed.Name;
                    fmCustomer.Name = item.Contract.CustomerAccount.Customer.Name;
                    fmCustomer.Sex = item.Contract.CustomerAccount.Customer.Sex;

                    fmCustomer.StarTime = QueryReNewContract(item.Contract).StartTime.Value.ToString("yyyy-MM-dd");

                    var contractserviceChange =
                        _fetcher
                            .Query<Entities.ContractServicePackChange>(
                                )
                            .FirstOrDefault(x => x.Contract.Id == item.Contract.Id &&
                                    x.Contract.ActualEndTime.Value == x.ChangeEndDate);
                    if (contractserviceChange != null)
                    {
                        fmCustomer.NursingType = contractserviceChange.ConcernType;
                        //床位费 + 餐费 + 基础服务费 + 打包服务费 + 失智照护费
                        fmCustomer.Cost = item.LongRoomRate + item.LongMeals + contractserviceChange.LongServiceMonthlyAmount +
                                          contractserviceChange.LongConcernAmount;
                    }
                    //取年龄
                    try
                    {
                        int age = item.Contract.CustomerAccount.Customer.Birthday.ToAge();
                        fmCustomer.Age = age;
                    }
                    catch
                    {
                        fmCustomer.Age = 0;
                    }
                    if (item.NewIsCompartment)
                    {
                        fmCustomer.IsCompartment = "包房";
                        lists.Add(fmCustomer);
                        break;
                    }
                    else
                    {
                        fmCustomer.IsCompartment = "单床";
                        lists.Add(fmCustomer);
                    }
                }
                result.FmCustomers = lists.OrderBy(x => x.BedName).ToList();
                result.resultCode = "200";
            }
            else
            {
                result.resultCode = "404";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public Entities.Contract QueryReNewContract(Entities.Contract contract)
        {
            var reNewContract = _fetcher.Query<Entities.Contract>()
                .FirstOrDefault(
                    x =>
                        x.CustomerAccount.Id == contract.CustomerAccount.Id &&
                        x.Status == ContractStatus.失效 &&
                        x.ActualEndTime == contract.StartTime.Value.AddSeconds(-1));
            if (reNewContract != null)
            {
                return QueryReNewContract(reNewContract);
            }
            return contract;
        }


        public class Result
        {
            public string resultCode { get; set; }
            //房间号，房型，床位数量
            public string RommName { get; set; }
            public string RommType { get; set; }
            public int Beds { get; set; }
            public decimal BedCost { get; set; }
            public List<FmCustomer> FmCustomers { get; set; }
        }
        public class FmCustomer
        {
            public string BedName { get; set; }
            public string Name { get; set; }
            public string Sex { get; set; }
            public int Age { get; set; }
            public string StarTime { get; set; }
            public string NursingType { get; set; }
            public decimal Cost { get; set; }
            public string IsCompartment { get; set; }
        }
    }
}