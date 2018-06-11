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
}