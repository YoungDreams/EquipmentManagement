using Foundation.Data;
using Foundation.Messaging;
using NHibernate.Util;
using PPM.Commands;
using PPM.Entities;

namespace PPM.CommandHandlers
{
    public class SetEquipmentCategoryColumnsCommandHandler : ICommandHandler<SetEquipmentCategoryColumnsCommand>
    {
        private readonly IRepository _repository;
        public SetEquipmentCategoryColumnsCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public void Handle(SetEquipmentCategoryColumnsCommand command)
        {
            var productCategory = _repository.Get<EquipmentCategory>(command.Id);
            for (var i = 0; i < command.Columns.Count; i++)
            {
                var column = new EquipmentCategoryColumn
                {
                    ColumnName = command.Columns[i],
                    ColumnType = command.ColumnsTypes[i].ToString(),
                    EquipmentCategory = productCategory
                };
                _repository.Create(column);
            }
        }
    }

    public class DeleteEquipmentCategoryColumnsCommandHandler : ICommandHandler<DeleteEquipmentCategoryColumnCommand>
    {
        private readonly IRepository _repository;
        public DeleteEquipmentCategoryColumnsCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public void Handle(DeleteEquipmentCategoryColumnCommand command)
        {
            _repository.Delete<EquipmentCategoryColumn>(command.Id);
        }
    }
}