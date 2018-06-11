using System.Collections.Generic;
using System.Web.Mvc;
using PPM.Commands;
using PPM.Entities;
using PPM.Web.Common;

namespace PPM.MVC.Views.Equipment.Category
{
    public class EditViewModel: EditEquipmentCategoryCommand
    {
        private readonly UrlHelper _urlHelper;
        public EditViewModel(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }
        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<SelectListItem> ColumnTypes { get; set; }

        public EquipmentCategoryTreeView ProductCategoryTreeView { get; set; }
        public object DeleteCommand(int id)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("DeleteColumn", "EquipmentCategory"),
                Command = new DeleteEquipmentCategoryCommand { Id = id }
            };
        }
    }

    public class EditEquipmentCategoryColumnViewModel : EditEquipmentCategoryColumnCommand
    {
    }
}