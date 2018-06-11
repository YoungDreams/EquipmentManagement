using System.Linq;
using System.Web.Mvc;
using Foundation.Messaging;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Query;
using PensionInsurance.Shared;
using PensionInsurance.Web.Views.Account;

namespace PensionInsurance.Web.Views.Finance.NCCertificate
{
    public class NCCertificateController : AuthorizedController
    {
        private readonly INCCertificateQueryService _ncCertificateQueryService;
        private readonly IProjectQueryService _projectQueryService;
        private readonly ICommandService _commandService;

        public NCCertificateController(INCCertificateQueryService ncCertificateQueryService,
            IProjectQueryService projectQueryService, ICommandService commandService)
        {
            _ncCertificateQueryService = ncCertificateQueryService;
            _projectQueryService = projectQueryService;
            _commandService = commandService;
        }

        [HttpGet]
        public ActionResult Index(int page = 1, int pageSize = Web.Common.PaginationSetttings.PageSize,
            NCCertificateQuery query = null)
        {
            if (!PensionInsurance.Shared.WebAppContext.Current.User.HasPermission(ModuleType.NC凭证管理, Permission.查看))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var viewModel = new IndexViewModel(Url)
            {
                NCCertificates = _ncCertificateQueryService.Query(page, pageSize, query)
            };
            return View("~/Views/Finance/NCCertificate/Index.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult Entries(int ncCertificateId)
        {
            if (!PensionInsurance.Shared.WebAppContext.Current.User.HasPermission(ModuleType.NC凭证管理, Permission.查看))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var viewModel = new EntriesViewModel
            {
                NCCertificateEntries = _ncCertificateQueryService.QueryEntries(ncCertificateId)
            };
            return View("~/Views/Finance/NCCertificate/Entries.cshtml", viewModel);
        }

        [HttpGet]
        public PartialViewResult Generate(NCCertificateType type)
        {
            GenerateViewModel viewModel = new GenerateViewModel();
            viewModel.NCCertificateType = type;
            if (type == NCCertificateType.月费结转收入)
            {
                viewModel.Title = "生成客户月账单凭证";
            }
            if (type == NCCertificateType.一次性安置费结转收入)
            {
                viewModel.Title = "生成客户一次性安置费账单凭证";
            }
            viewModel.ProjectList = _projectQueryService.QueryAllValidByProjectFilter()
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });

            return PartialView("~/Views/Finance/NCCertificate/_Generate.cshtml", viewModel);
        }
        
        [HttpPost]
        public void Generate(GenerateNCCertificateCommand command)
        {
            _commandService.Execute(command);
        }

        [HttpPost]
        public void Import(ImportNCCommand command)
        {
            _commandService.Execute(command);
        }

        [HttpPost]
        public void Delete(DeleteNCCommand command)
        {
            _commandService.Execute(command);
        }
    }
}