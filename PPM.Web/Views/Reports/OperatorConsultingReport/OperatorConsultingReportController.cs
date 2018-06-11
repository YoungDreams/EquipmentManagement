using Foundation.Data;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Query;
using PensionInsurance.Web.Views.Account;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PensionInsurance.Entities;

namespace PensionInsurance.Web.Views.Reports.OperatorConsultingReport
{
    public class OperatorConsultingReportController : AuthorizedController
    {
        private readonly IProjectQueryService _projectQueryService;
        private readonly ICommandService _commandService;
        private readonly IOperatorConsultingReportQuery _operatorconsultingreport;

        public OperatorConsultingReportController(ICommandService commandService, IProjectQueryService projectQueryService, IOperatorConsultingReportQuery operatorconsultingreport)
        {
            _commandService = commandService;
            _projectQueryService = projectQueryService;
            _operatorconsultingreport = operatorconsultingreport;
        }

        // GET: SalesPersonDailyReport
        public ActionResult Index(OperatorConsultingReportQurey query = null)
        {
            var viewModel = new IndexViewModel
            {
                ProjectList = _projectQueryService.QueryAllValidByProjectFilter().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                Query = query
            };
            
            if (query?.ReportType != null)
            {
                switch (query.ReportType)
                {
                    case OperatorConsultingReportType.总数据:
                        return RedirectToAction("Total", query);
                    case OperatorConsultingReportType.座席员:
                        return RedirectToAction("Operators", query);
                    case OperatorConsultingReportType.项目:
                        //if (query.ProjectIds != null && query.ProjectIds.Any())
                        //{
                        //    for (int i = 0; i < query.ProjectIds.Count; i++)
                        //    {
                        //       Add($"Query.ProjectIds[{i}]", query.ProjectIds[i]);
                        //    }
                        //}
                        return RedirectToAction("Projects", query);
                }
            }

            return View("~/Views/Reports/OperatorConsultingReport/Index.cshtml", viewModel);
        }

        public ActionResult Total(OperatorConsultingReportQurey query = null)
        {
            var viewModel = new TotalViewModel
            {
                ProjectList = _projectQueryService.QueryAllValidByProjectFilter().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                Report = _operatorconsultingreport.GetTotal(query).ToList(),
                Query = query
            };
            return View("~/Views/Reports/OperatorConsultingReport/Total.cshtml", viewModel);
        }

        public ActionResult Operators(OperatorConsultingReportQurey query = null)
        {
            var viewModel = new OperatorsViewModel
            {
                ProjectList = _projectQueryService.QueryAllValidByProjectFilter().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                Report = _operatorconsultingreport.GetOperators(query).ToList(),
                Query = query
            };
            return View("~/Views/Reports/OperatorConsultingReport/Operators.cshtml", viewModel);
        }

        public ActionResult Projects(OperatorConsultingReportQurey query = null)
        {
            var viewModel = new ProjectsViewModel
            {
                ProjectList = _projectQueryService.QueryAllValidByProjectFilter().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                Report = _operatorconsultingreport.GetProjects(query).ToList(),
                Query = query
            };
            return View("~/Views/Reports/OperatorConsultingReport/Projects.cshtml", viewModel);
        }

        public ActionResult ExportReport(OperatorConsultingReportCommand command)
        {
            var result = _commandService.ExecuteFoResult(command);

            return Json(new
            {
                success = result.IsSucceed,
                redirect = $"{Url.Content("~/Attachments/Report/")}{result.FileName}"
            }, JsonRequestBehavior.AllowGet);
        }
    }


}