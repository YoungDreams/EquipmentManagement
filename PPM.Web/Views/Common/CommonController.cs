using System.Linq;
using System.Web.Mvc;
using Foundation.Messaging;
using PensionInsurance.Query;
using PensionInsurance.Web.Views.Account;

namespace PensionInsurance.Web.Views.Common
{
    public class CommonController : AuthorizedController
    {
        private readonly IAreaQueryService _areaQueryService;
        public CommonController(ICommandService commandService, IAreaQueryService areaQueryService)
        {
            _areaQueryService = areaQueryService;
        }

        public ActionResult NotFound()
        {
            return View("NotFound");
        }

        [HttpGet]
        public JsonResult GetProvinces()
        {
            var provinces = _areaQueryService.QueryProvinces().Select(x => new {x.Id, x.Name}).ToArray();

            return Json(provinces, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCities(int provinceId)
        {
            var cities = _areaQueryService.QueryCities(provinceId)
                .Select(x => new {x.Id, x.Name})
                .ToArray();

            return Json(cities, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDistricts(int cityId)
        {
            var districts = _areaQueryService.QueryDistricts(cityId)
                .Select(x => new {x.Id, x.Name}).ToArray();

            return Json(districts, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTowns(int districtId)
        {
            var towns = _areaQueryService.QueryTowns(districtId)
                .Select(x => new { x.Id, x.Name }).ToArray();

            return Json(towns, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAreasByParentAreaId(int? parentAreaId)
        {
            var areas = _areaQueryService.QueryAreasByParentId(parentAreaId)
                .Select(x => new {code = x.Id, address = x.Name}).ToArray();

            return Json(areas, JsonRequestBehavior.AllowGet);
        }
    }
}