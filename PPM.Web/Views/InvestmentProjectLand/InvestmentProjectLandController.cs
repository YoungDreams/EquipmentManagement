using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Query;
using PensionInsurance.Web.Views.Account;
using Foundation.Core;

namespace PensionInsurance.Web.Views.InvestmentProjectLand
{
    public class InvestmentProjectLandController : AuthorizedController
    {
        private readonly ICommandService _commandService;
        private readonly IInvestmentProjectLandService _investmentProjectLandService;

        public InvestmentProjectLandController(ICommandService commandService, IInvestmentProjectLandService investmentProjectLandService)
        {
            _commandService = commandService;
            _investmentProjectLandService = investmentProjectLandService;
        }

        /// <summary>
        /// 查询页面
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public ActionResult Index(int page = 1, int pageSize = Web.Common.PaginationSetttings.PageSize, InvestmentProjectLandQuery query = null)
        {
            

            var viewModel = new IndexViewModel(Url)
            {
                Query = query,
                InvestmentProjectLands = _investmentProjectLandService.Query(page, pageSize, query),
            };
            return View("~/Views/InvestmentProjectLand/Index.cshtml", viewModel);
        }

        /// <summary>
        /// 跳转到新增页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create()
        {
            var viewModel = new CreateViewModel()
            {
            };
            return View("~/Views/InvestmentProjectLand/Create.cshtml", viewModel);
        }

        /// <summary>
        /// 房型新增处理
        /// </summary>
        /// <param name="command"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(CreateInvestmentProjectLandCommand command,string returnUrl)
        {
            _commandService.Execute(command);
            return returnUrl.IsNullOrWhiteSpace() ? (ActionResult)RedirectToAction("Index", new { InvestmentProjectId = command.InvestmentProjectId }) : Redirect(returnUrl);
        }

        /// <summary>
        /// 跳转到编辑页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var investmentProjectLand = _investmentProjectLandService.Get(id);
            var viewModel = new EditViewModel
            {
                Id = investmentProjectLand.Id,
                BuildingAcreage=investmentProjectLand.BuildingAcreageGround + investmentProjectLand.BuildingAcreageUnderGround,
                BuildingLandAcreage = investmentProjectLand.BuildingLandAcreage,
                BuildingAcreageGround = investmentProjectLand.BuildingAcreageGround,
                BuildingAcreageUnderGround = investmentProjectLand.BuildingAcreageUnderGround,
                LandCharacteristics = investmentProjectLand.LandCharacteristics,
                LandName = investmentProjectLand.LandName,
                Plotratio = investmentProjectLand.Plotratio,
                Period = investmentProjectLand.Period,
                InvestmentProjectId = investmentProjectLand.InvestmentProject.Id
            };
            return View("~/Views/InvestmentProjectLand/Edit.cshtml", viewModel);
        }

        /// <summary>
        /// 跳转到编辑页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetInvestmentProjectLands(int id)
        {
            var investmentProjectLand = _investmentProjectLandService.Get(id);
            var viewModel = new EditViewModel
            {
                Id = investmentProjectLand.Id,
                BuildingAcreage = investmentProjectLand.BuildingAcreageGround + investmentProjectLand.BuildingAcreageUnderGround,
                BuildingLandAcreage = investmentProjectLand.BuildingLandAcreage,
                BuildingAcreageGround = investmentProjectLand.BuildingAcreageGround,
                BuildingAcreageUnderGround = investmentProjectLand.BuildingAcreageUnderGround,
                LandCharacteristics = investmentProjectLand.LandCharacteristics,
                LandName = investmentProjectLand.LandName,
                Plotratio = investmentProjectLand.Plotratio,
                Period = investmentProjectLand.Period,
                InvestmentProjectId = investmentProjectLand.InvestmentProject.Id
            };
            return Json(viewModel,JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 编辑页面提交
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(EditInvestmentProjectLandCommand command,string returnUrl)
        {
            _commandService.Execute(command);
            return returnUrl.IsNullOrWhiteSpace() ? (ActionResult)RedirectToAction("Index", new { InvestmentProjectId = command.InvestmentProjectId }) : Redirect(returnUrl);
        }


        /// <summary>
        /// 删除房型
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(DeleteEntityCommand command)
        {
            Entities.InvestmentProjectLand investmentProjectLand = _investmentProjectLandService.Get(command.EntityId);
            if (investmentProjectLand == null)
                throw new ApplicationException("InvestmentProjectLand cannot be found");

            _commandService.Execute(DeleteEntityCommand.Of(investmentProjectLand));

            return command.ReturnUrl.IsNullOrWhiteSpace()
                ? (ActionResult)RedirectToAction("Index")
                : Redirect(command.ReturnUrl);
        }

    }
}