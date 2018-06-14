using System.Collections.Generic;
using System.Linq;
using Foundation.Data;
using Foundation.Messaging;
using PPM.Commands;
using PPM.Entities;

namespace PPM.CommandHandlers
{
    public class EditEquipementCategoryColumnCommandHandler : ICommandHandler<EditEquipmentCategoryColumnCommand>
    {
        private readonly IRepository _repository;
        public EditEquipementCategoryColumnCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public void Handle(EditEquipmentCategoryColumnCommand command)
        {
            var equipmentCategoryColumn = _repository.Get<EquipmentCategoryColumn>(command.Id);
            equipmentCategoryColumn.ColumnName = command.ColumnName;
            equipmentCategoryColumn.ColumnType = command.ColumnType;
            _repository.Update(equipmentCategoryColumn);
        }
    }

    public class DeleteEquipmentCategoryCommandHandler : ICommandHandler<DeleteEquipmentCategoryCommand>
    {
        private readonly IRepository _repository;
        public DeleteEquipmentCategoryCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public void Handle(DeleteEquipmentCategoryCommand command)
        {
            //todo...删除分类下的所有子分类和相关的设备信息及设备动态列。
            var category = _repository.Get<EquipmentCategory>(command.Id);
            var equipmentInfo = _repository.Query<EquipmentInfo>().Where(x => x.EquipmentCategory == category).ToList();
            foreach (var info in equipmentInfo)
            {
                _repository.Execute("Delete from EquipmentInfoColumnValue Where EquipmentInfoId = " + info.Id);
                _repository.Execute("Delete from EquipmentInfo Where Id = " + info.Id);
            }
            _repository.Execute("Delete from EquipmentCategoryColumn Where EquipmentCategoryId = " + category.Id);
            DeleteChildrenCategories(category);
            _repository.Execute("Delete from EquipmentCategory Where Id = " + category.Id);
        }

        private void DeleteChildrenCategories(EquipmentCategory category)
        {
            var categories = GetCategoriesByParentId(category.Id);
            foreach (var equipmentCategory in categories)
            {
                var info = _repository.Query<EquipmentInfo>().Where(x => x.EquipmentCategory == category).ToList();
                foreach (var i in info)
                {
                    _repository.Execute("Delete from EquipmentInfoColumnValue Where EquipmentInfoId = " + i.Id);
                    _repository.Execute("Delete from EquipmentInfo Where Id = " + i.Id);
                }
                _repository.Execute("Delete from EquipmentCategoryColumn Where EquipmentCategoryId = " + equipmentCategory.Id);
                _repository.Execute("Delete from EquipmentCategory Where Id = " + equipmentCategory.Id);
            }
        }

        private List<EquipmentCategory> GetCategoriesByParentId(int parentId)
        {
            List<EquipmentCategory> categories = new List<EquipmentCategory>();
            var category = _repository.Query<EquipmentCategory>().FirstOrDefault(x => x.ParentId == parentId);
            if (category == null) return new List<EquipmentCategory>();
            categories.Add(category);
            if (category.ParentId != null && category.ParentId.Value != 0)
            {
                categories.AddRange(GetCategoriesByParentId(category.Id));
            }
            return categories;
        }
    }
}