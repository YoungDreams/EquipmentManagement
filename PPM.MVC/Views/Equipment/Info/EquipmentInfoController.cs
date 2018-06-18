using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Castle.Windsor.Installer;
using Foundation.Data;
using Foundation.Messaging;
using PPM.Commands;
using PPM.Entities;
using PPM.MVC.Views.Account;
using PPM.Query;
using PPM.Web.Common;

namespace PPM.MVC.Views.Equipment.Info
{
    public class EquipmentInfoController : AuthorizedController
    {
        private readonly ICommandService _commandService;
        private readonly IRepository _repository;
        private readonly IEquipmentInfoQueryService _equipmentInfoQueryService;
        private readonly IEquipmentCategoryQueryService _categoryQueryService;
        private readonly string _host = ConfigurationManager.AppSettings["Host"];

        public EquipmentInfoController(ICommandService commandService, IEquipmentInfoQueryService equipmentInfoQueryService, IEquipmentCategoryQueryService categoryQueryService, IRepository repository)
        {
            _commandService = commandService;
            _equipmentInfoQueryService = equipmentInfoQueryService;
            _categoryQueryService = categoryQueryService;
            _repository = repository;
        }

        public ActionResult Index(int page = 1, int pageSize = PPM.Web.Common.PaginationSetttings.PageSize, EquipmentInfoQuery query = null)
        {
            var categories = _categoryQueryService.QueryAllValid().ToList();
            var viewModel = new IndexViewModel(Url)
            {
                Query = query,
                Items = _equipmentInfoQueryService.Query(page, pageSize, query),
                ProductCategoryTreeView = new EquipmentCategoryTreeView().GetProductCategoryTreeView(categories),
                CategoryText = query.CategoryId.HasValue
                    ? categories.SingleOrDefault(x => x.Id == query.CategoryId).Name
                    : string.Empty,
                CategoryText1 = query.CategoryId1.HasValue
                    ? categories.SingleOrDefault(x => x.Id == query.CategoryId1).Name
                    : string.Empty
            };
            foreach (var equipmentInfo in viewModel.Items)
            {
                equipmentInfo.QrCodeImage = _host + equipmentInfo.QrCodeImage;
            }
            return View("~/Views/Equipment/Info/Index.cshtml", viewModel);
        }

        public ActionResult Create(int? categoryId)
        {
            var categories = _categoryQueryService.QueryAllValid().ToList();
            var viewModel = new CreateViewModel
            {
                Categories = categories
                    .Where(x => x.ParentId == 0)
                    .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                ProductCategoryTreeView = new EquipmentCategoryTreeView().GetProductCategoryTreeView(categories),
            };
            if (categoryId != null)
            {
                var category = _categoryQueryService.Get(categoryId.Value);
                viewModel.EquipmentCategory = category;
                viewModel.CategoryId = categoryId.Value;
                viewModel.CategoryText = category?.Name;
            }
            return View("~/Views/Equipment/Info/Create.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult Create(CreateEquipmentInfoCommand command)
        {
            if (Request.Files.Count > 0)
            {
                command.Files = new List<FileInfo>();
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    if (i == 0)
                    {
                        command.File = new FileInfo
                        {
                            FileBytes = Request.Files[i].ReadBytes(),
                            FileName = Request.Files[i].FileName
                        };
                    }
                    else
                    {
                        command.Files.Add(new FileInfo
                        {
                            FileBytes = Request.Files[i].ReadBytes(),
                            FileName = Request.Files[i].FileName
                        });
                    }
                }
            }
            _commandService.Execute(command);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var equipmentInfo = _equipmentInfoQueryService.Get(id);
            var categories = _categoryQueryService.QueryAllValid().ToList();
            var viewModel = new EditViewModel
            {
                EquipmentInfo = equipmentInfo,
                ProductCategoryTreeView = new EquipmentCategoryTreeView().GetProductCategoryTreeView(categories)
            };
            return View("~/Views/Equipment/Info/Edit.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult Edit(EditEquipmentInfoCommand command)
        {
            if (Request.Files.Count > 0)
            {
                command.Files = new List<FileInfo>();
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    if (i == 0)
                    {
                        command.File = new FileInfo
                        {
                            FileBytes = Request.Files[i].ReadBytes(),
                            FileName = Request.Files[i].FileName
                        };
                    }
                    else
                    {
                        command.Files.Add(new FileInfo
                        {
                            FileBytes = Request.Files[i].ReadBytes(),
                            FileName = Request.Files[i].FileName
                        });
                    }
                }
            }
            _commandService.Execute(command);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Delete(DeleteEquipmentInfoCommand command)
        {
            _commandService.Execute(command);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public PartialViewResult BatchImport()
        {
            return PartialView("~/Views/Equipment/Info/_Batch.Import.cshtml");
        }
        [HttpPost]
        public ActionResult BatchImport(ImportBatchEquipmentInfoCommand command)
        {
            if (Request.Files.Count > 0)
            {
                command.FileBytes = Request.Files[0].ReadBytes();
                command.FileName = Request.Files[0].FileName;
            }
            var result = _commandService.ExecuteFoResult(command);
            return Json(new
            {
                success = result.IsSucceed,
                errors = result.Errors.ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UploadAttachment(UploadEquipmentInfoAttachmentCommand command)
        {
            var result =_commandService.ExecuteFoResult(command);
            return Json(result);
        }

        public ActionResult ViewInWeChat(int id)
        {
            var equipmentInfo = _repository.Get<EquipmentInfo>(id);
            var viewModel = new ViewInWechatViewModel
            {
                EquipmentInfo = equipmentInfo
            };

            viewModel.EquipmentInfo.QrCodeImage = _host + equipmentInfo.QrCodeImage;
            var index = 0;
            foreach (var categoryColumn in viewModel.EquipmentInfo.EquipmentCategory.Columns)
            {
                if (categoryColumn.ColumnType == EquipmentCategoryColumnType.文件.ToString())
                {
                    viewModel.EquipmentInfo.EquipmentInfoColumnValues[index].Value =
                        _host + viewModel.EquipmentInfo.EquipmentInfoColumnValues[index].Value;
                }
                index++;
            }

            return View("~/Views/Equipment/Info/ViewEquipmentInfo.cshtml", viewModel);
        }
    }
}
