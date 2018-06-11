using Foundation.Data;
using Foundation.Messaging;
using PPM.Commands;
using PPM.Entities;

namespace PPM.CommandHandlers
{
    public class CreateEquipementCategoryCommandHandler : ICommandHandler<CreateEquipmentCategoryCommand>
    {
        private readonly IRepository _repository;
        public CreateEquipementCategoryCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public void Handle(CreateEquipmentCategoryCommand command)
        {
            var productCategory = command.MapToEntity<EquipmentCategory>();
            if (command.ParentId.HasValue)
            {
                var parentcate = _repository.Get<EquipmentCategory>(command.ParentId.Value);

                productCategory.Layer = parentcate.Layer + 1;
            }
            else
            {
                productCategory.ParentId = 0;
                productCategory.Layer = 1;
            }
            _repository.Create(productCategory);
        }
    }
}