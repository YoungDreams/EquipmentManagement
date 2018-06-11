using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foundation.Data;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Query;
using PensionInsurance.Web.Common;
using PensionInsurance.Web.Views.Account;

namespace PensionInsurance.Web.Views.Sales.Budget
{
    public class SalesBudgetController : AuthorizedController
    {
        private readonly IProjectQueryService _projectQueryService;
        private readonly ICommandService _commandService;
        private readonly ISalesBudgetQueryService _budgetQueryService;
        private readonly IFetcher _fetcher;

        public SalesBudgetController(IProjectQueryService projectQueryService, ICommandService commandService, ISalesBudgetQueryService budgetQueryService, IFetcher fetcher)
        {
            _projectQueryService = projectQueryService;
            _commandService = commandService;
            _budgetQueryService = budgetQueryService;
            _fetcher = fetcher;
        }

        // GET: SalesBudget
        public ActionResult Index(SalesBudgetQuery query)
        {
            var viewModel = new IndexViewModel(Url)
            {
                Query = query,

                ProjectList = _projectQueryService.QueryAllValidByProjectFilter().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            if (query.StartYear.HasValue && query.ProjectId.HasValue)
            {
                var startMonth = new DateTime(query.StartYear.Value, 1, 1);
                var endMonth = new DateTime(query.StartYear.Value, 12, 31);

                var months = GetMonths(startMonth, endMonth).ToList();
                viewModel.Months = months;
                var data = _budgetQueryService.Query(query).ToList();
                if (data.Any())
                {
                    viewModel.ProjectId = data.First().Project.Id;
                    viewModel.ProjectName = data.First().Project.Name;
                    viewModel.MonthData = months.ToDictionary(x => x,
                        x => data.FirstOrDefault(s => s.BudgetMonth.Year == x.Year && s.BudgetMonth.Month == x.Month));
                }

            }
            return View("~/views/sales/budget/index.cshtml",viewModel);
        }
        [HttpGet]
        public PartialViewResult Create()
        {
            var viewModel = new CreateViewModel
            {
                ProjectList = _projectQueryService.QueryAllValidByProjectFilter().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            return PartialView("~/Views/sales/Budget/_Budget.Create.cshtml", viewModel);
        }

        [HttpPost]
        public void Create(CreateSalesBudgetCommand command)
        {
            _commandService.Execute(command);
        }

        public ActionResult Export(ExportSalesBudgetCommand command)
        {
            var result = _commandService.ExecuteFoResult(command);

            return Json(new
            {
                success = result.IsSucceed,
                redirect = $"{Url.Content("~/Attachments/Report/")}{result.FileName}"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult Import()
        {
            var viewModel = new ImportViewModel
            {
                ProjectList = _projectQueryService.QueryAllValidByProjectFilter().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            return PartialView("~/Views/sales/Budget/_Budget.Import.cshtml", viewModel);
        }

        [HttpPost]
        public void Import(ImportSalesBudgetCommand command)
        {
            if (Request.Files.Count > 0)
            {
                command.FilePath = Request.Files[0].ReadBytes();
                command.FileName = Request.Files[0].FileName;
            }
            _commandService.Execute(command);
        }

        public List<DateTime> GetMonths(DateTime startTime, DateTime endTime)
        {
            try
            {
                List<DateTime> months = new List<DateTime>();
                DateTime c1 = Convert.ToDateTime(startTime.ToString("yyyy-MM"));
                DateTime c2 = Convert.ToDateTime(endTime.ToString("yyyy-MM"));
                if (c1 > c2)
                {
                    DateTime tmp = c1;
                    c1 = c2;
                    c2 = tmp;
                }
                while (c2 >= c1)
                {
                    months.Add(c1);
                    c1 = c1.AddMonths(1);
                }
                return months;
            }
            catch { return null; }
        }
    }
}