using Foundation.Data;
using Foundation.Messaging;
using PPM.Commands;
using PPM.Entities;

namespace PPM.CommandHandlers
{
    public class EditEquipementCategoryCommandHandler : ICommandHandler<EditEquipmentCategoryCommand>
    {
        private readonly IRepository _repository;
        public EditEquipementCategoryCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public void Handle(EditEquipmentCategoryCommand command)
        {
            var productCategory = _repository.Get<EquipmentCategory>(command.Id);
            command.PopulateEntity(productCategory);
            if (command.ParentId.HasValue && command.ParentId.Value != 0)
            {
                var parentcate = _repository.Get<EquipmentCategory>(command.ParentId.Value);

                productCategory.Layer = parentcate.Layer + 1;
            }
            else
            {
                productCategory.ParentId = 0;
                productCategory.Layer = 1;
            }
            _repository.Update(productCategory);
        }
    }
}