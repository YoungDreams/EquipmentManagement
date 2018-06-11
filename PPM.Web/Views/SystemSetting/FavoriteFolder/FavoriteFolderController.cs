using System;
using System.Web.Mvc;
using Foundation.Messaging;
using Foundation.Utils;
using PensionInsurance.Commands;
using PensionInsurance.Query;
using PensionInsurance.Shared;
using PensionInsurance.Web.Views.Account;

namespace PensionInsurance.Web.Views.SystemSetting.FavoriteFolder
{
    public class FavoriteFolderController : AuthorizedController
    {
        private readonly ICommandService _commandService;
        private readonly IFavoriteFolderQueryService _favoriteFolderQuery;

        public FavoriteFolderController(ICommandService commandService, IFavoriteFolderQueryService favoriteFolderQuery)
        {
            _commandService = commandService;
            _favoriteFolderQuery = favoriteFolderQuery;
        }

        public ActionResult Index(int page = 1, int pageSize = Web.Common.PaginationSetttings.PageSize, FavoriteFolderQuery query = null)
        {
            var viewModel = new IndexViewModel(Url)
            {
                Query = query,
                Items = _favoriteFolderQuery.Query(page, pageSize, query),
            };
            return View("~/Views/SystemSetting/FavoriteFolder/Index.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var viewModel = new CreateViewModel();
            return View("~/Views/SystemSetting/FavoriteFolder/Create.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult Create(CreateFavoriteFolderCommand command)
        {
            _commandService.Execute(command);

            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            Entities.FavoriteFolder favoriteFolder = _favoriteFolderQuery.Get(id);
            if (favoriteFolder == null)
                throw new ApplicationException("FavoriteFolder cannot be found");

            var viewModel = new EditViewModel
            {
                FavoriteFolderId = favoriteFolder.Id,
                Name = favoriteFolder.Name,
                Description = favoriteFolder.Description
            };
            
            return View("~/Views/SystemSetting/FavoriteFolder/Edit.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult Edit(EditFavoriteFolderCommand command)
        {
            _commandService.Execute(command);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(DeleteEntityCommand command)
        {
            Entities.FavoriteFolder favoriteFolder = _favoriteFolderQuery.Get(command.EntityId);
            if (favoriteFolder == null)
                throw new ApplicationException("FavoriteFolder cannot be found");

            _commandService.Execute(DeleteEntityCommand.Of(favoriteFolder));

            return RedirectToAction("Index");
        }
    }
}
