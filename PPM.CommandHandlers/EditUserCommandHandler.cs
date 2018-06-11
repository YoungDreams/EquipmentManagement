using System.Collections.Generic;
using Foundation.Data;
using Foundation.Messaging;
using PPM.Commands;
using PPM.Entities;

namespace PPM.CommandHandlers
{
    public class EditUserCommandHandler : ICommandHandler<EditUserCommand>
    {
        private readonly IRepository _repository;

        public EditUserCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public void Handle(EditUserCommand command)
        {
            var user = _repository.Get<User>(command.UserId);
            command.PopulateEntity(user);
            //user.Roles = new List<Role>();
            //foreach (var roleId in command.RoleIds)
            //{
            //    user.Roles.Add(_repository.Get<Role>(roleId));
            //}
            _repository.Update(user);
        }
    }
}